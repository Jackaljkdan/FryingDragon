using DG.Tweening;
using JK.Injection;
using JK.Utils;
using Project.Dragon;
using Project.Items;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class RecipesVisualizer : MonoBehaviour
    {
        #region Inspector

        public List<IngredientImage> imagesList = new();
        public List<IngredientTypeValue> neededValueList = new();

        public new RectTransform animation;
        public Slider slider;
        public Image sliderBackgroundArea;
        public Image sliderFillArea;

        public Image bgImage;

        public Color bgColor = Color.white;
        public Color cookingBgColor = new Color(246, 158, 71);

        public Color sliderBackgroundColor = new Color(63, 63, 63);
        public Color sliderCookingColor = Color.white;
        public Color sliderOvercookingColor = Color.red;

        [RuntimeField]
        public List<IngredientTypeValue> availableIngredients = new();

        [Injected]
        private SignalBus signalBus;

        [Injected]
        public DragonItemHolder holder;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);
            holder = context.Get<DragonItemHolder>(this);
        }

        #endregion

        private void Awake()
        {
            Inject();
            slider.GetComponent<RectTransform>().localScale = Vector3.zero;
        }

        private void Start()
        {
            signalBus.AddListener<IngredientTakenSignal>(OnIngredientTaken);
            signalBus.AddListener<IngredientLostSignal>(OnIngredientLost);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<IngredientTakenSignal>(OnIngredientTaken);
            signalBus.RemoveListener<IngredientLostSignal>(OnIngredientLost);
        }

        public void ShowRecipe(Recipe recipe)
        {
            List<IngredientTypeValue> ingredients = recipe.ingredients;
            neededValueList.AddRange(ingredients);
            GetComponentsInChildren(includeInactive: true, imagesList);

            for (int i = 0; i < ingredients.Count; i++)
            {
                IngredientTypeValue ingredient = ingredients[i];
                imagesList[i].SetImage(ingredient);
            }

            GetStartingIngredients();
        }

        private void GetStartingIngredients()
        {
            if (!holder.holdedItem)
                return;

            if (holder.holdedItem.TryGetComponentInChildren<Bowl>(out Bowl bowl))
                foreach (Ingredient ingredient in bowl.ingredients)
                {
                    ShowCheckmark(ingredient.ingredientTypeValue);
                }
        }

        private void OnIngredientTaken(IngredientTakenSignal arg)
        {
            Ingredient ingredient = arg.ingredient;

            ShowCheckmark(ingredient.ingredientTypeValue);
        }

        private void ShowCheckmark(IngredientTypeValue type)
        {
            for (int i = 0; i < imagesList.Count; i++)
            {
                if (imagesList[i].ShowChecked(type))
                    break;
            }
        }

        private void OnIngredientLost(IngredientLostSignal args)
        {
            RemoveAllCheckmarks();

            foreach (Ingredient ingredient in args.availableIngredients)
            {
                ShowCheckmark(ingredient.ingredientTypeValue);
            }
        }

        private void RemoveAllCheckmarks()
        {
            for (int i = imagesList.Count - 1; i >= 0; i--)
            {
                imagesList[i].HideChecked();
            }
        }

        public Tween DOEnter()
        {
            animation.position = animation.position.WithY(animation.position.y + 20);
            return animation.DOLocalMoveY(0, 1).SetEase(Ease.OutElastic);
        }

        public Tween DOStartCookingAnimation()
        {
            bgImage.color = cookingBgColor;
            return animation.DOLocalMoveY(50, 1).SetEase(Ease.InElastic).OnComplete(
                () =>
                {
                    slider.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);

                });
        }

        private void StartOvercooking(float cookingTime)
        {
            slider.value = 0;
            sliderBackgroundArea.color = sliderCookingColor;
            sliderFillArea.color = sliderOvercookingColor;
            slider.DOValue(1f, cookingTime).SetEase(Ease.Linear);
        }

        private void StartCooking(float cookingTime)
        {
            slider.value = 0;
            sliderBackgroundArea.color = sliderBackgroundColor;
            sliderFillArea.color = sliderCookingColor;
            slider.DOValue(1f, cookingTime).SetEase(Ease.Linear).OnComplete(() => StartOvercooking(cookingTime));
        }

        public bool IsRecipeCooking(List<IngredientTypeValue> ingredients, float recipeCookingTime)
        {
            if (ingredients.Count != neededValueList.Count)
                return false;

            bool areEqual = ingredients.OrderBy(x => x).SequenceEqual(neededValueList.OrderBy(x => x));

            if (!areEqual)
                return false;

            StartCooking(recipeCookingTime);
            DOStartCookingAnimation();

            return true;
        }
    }
}