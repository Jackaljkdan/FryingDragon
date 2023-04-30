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

        public UICookingSlider cookingSlider;
        public UIBurned burnedImg;

        public Image bgImage;

        public Color bgColor = Color.white;
        public Color cookingBgColor = new Color(246, 158, 71);

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

        private Tween tween;

        private void Awake()
        {
            Inject();

            foreach (IngredientImage image in imagesList)
            {
                image.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            signalBus.AddListener<ItemAddedSignal>(OnItemAdded);
            signalBus.AddListener<IngredientTakenSignal>(OnIngredientTaken);
            signalBus.AddListener<IngredientLostSignal>(OnIngredientLost);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<ItemAddedSignal>(OnItemAdded);
            signalBus.RemoveListener<IngredientTakenSignal>(OnIngredientTaken);
            signalBus.RemoveListener<IngredientLostSignal>(OnIngredientLost);
        }

        public bool IsRecipeFulfilled(List<IngredientTypeValue> ingredients)
        {
            return ingredients.OrderBy(x => x).SequenceEqual(neededValueList.OrderBy(x => x));
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
                imagesList[i].gameObject.SetActive(true);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(animation);
            //LayoutRebuilder.ForceRebuildLayoutImmediate(animation.parent as RectTransform);
            (animation.parent as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (animation as RectTransform).rect.width);

            GetStartingIngredients();
        }

        private void OnItemAdded(ItemAddedSignal arg)
        {
            GetStartingIngredients();
        }

        private void GetStartingIngredients()
        {
            if (!holder.heldItem.Value)
                return;

            if (holder.heldItem.Value.TryGetComponentInChildren<Bowl>(out Bowl bowl))
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

        public Tween DOExit()
        {
            return animation.DOLocalMoveY(20, 1).SetEase(Ease.InElastic);
        }

        public Tween DOStartCookingAnimation()
        {
            bgImage.color = cookingBgColor;
            return animation.DOLocalMoveY(50, 1).SetEase(Ease.InElastic).OnComplete(
                () =>
                {
                    cookingSlider.DoScaleup();

                });

        }

        private Tween StartOvercooking(float cookingTime)
        {
            return cookingSlider.DOOverfill(cookingTime).OnComplete(StartBurn);
        }

        private Tween StartCooking(float cookingTime)
        {
            return cookingSlider.DOFill(cookingTime).OnComplete(() => StartOvercooking(cookingTime));
        }

        private void StopCooking()
        {
            tween?.Kill();
            cookingSlider.DOScaledown().OnComplete(() =>
            {
                bgImage.color = bgColor;
                animation.DOLocalMoveY(0, 1).SetEase(Ease.OutElastic);
            });

        }

        private void StartBurn()
        {
            tween = burnedImg.DOShow();
        }

        public bool ShouldRecipeStop(List<IngredientTypeValue> ingredients)
        {
            if (ingredients.Count != neededValueList.Count)
                return false;
            bool areEqual = ingredients.OrderBy(x => x).SequenceEqual(neededValueList.OrderBy(x => x));

            if (!areEqual)
                return false;

            StopCooking();
            return true;
        }

        public bool IsRecipeCooking(List<IngredientTypeValue> ingredients, float recipeCookingTime)
        {
            if (ingredients.Count != neededValueList.Count)
                return false;

            bool areEqual = ingredients.OrderBy(x => x).SequenceEqual(neededValueList.OrderBy(x => x));

            if (!areEqual)
                return false;

            tween?.Kill();
            tween = StartCooking(recipeCookingTime);
            DOStartCookingAnimation();

            return true;
        }
    }
}