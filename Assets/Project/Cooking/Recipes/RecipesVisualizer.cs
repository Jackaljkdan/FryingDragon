using JK.Injection;
using JK.Utils;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class RecipesVisualizer : MonoBehaviour
    {
        #region Inspector

        public List<IngredientImage> imagesList = new();
        public List<IngredientTypeValue> neededValueList = new();

        [RuntimeField]
        public List<IngredientTypeValue> availableIngredients = new();

        [Injected]
        private SignalBus signalBus;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);
        }

        private void Awake()
        {
            Inject();
        }

        #endregion

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
            IngredientImage[] ingredientImages = GetComponentsInChildren<IngredientImage>(true);
            imagesList.AddRange(ingredientImages);

            for (int i = 0; i < ingredients.Count; i++)
            {
                IngredientTypeValue ingredient = ingredients[i];
                ingredientImages[i].SetImage(ingredient);

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
    }
}