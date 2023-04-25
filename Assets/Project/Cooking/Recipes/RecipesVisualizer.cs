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

        public GameObject recipeUIElement;
        public List<Transform> recipeAnchors;
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
            signalBus.AddListener<NewRecipeSignal>(OnNewRecipe);
            signalBus.AddListener<IngredientTakenSignal>(OnIngredientTaken);
            signalBus.AddListener<IngredientLostSignal>(OnIngredientLost);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<NewRecipeSignal>(OnNewRecipe);
            signalBus.RemoveListener<IngredientTakenSignal>(OnIngredientTaken);
            signalBus.RemoveListener<IngredientLostSignal>(OnIngredientLost);
        }

        private Transform FindEmptyAnchor()
        {
            for (int i = 0; i < recipeAnchors.Count; i++)
            {
                if (recipeAnchors[i].childCount == 0)
                    return recipeAnchors[i];
            }
            return null;
        }

        private void OnNewRecipe(NewRecipeSignal arg)
        {
            Transform recipesAnchor = FindEmptyAnchor();

            if (!recipesAnchor)
                return;

            List<IngredientTypeValue> ingredients = arg.recipe.ingredients;
            neededValueList.AddRange(ingredients);

            GameObject spawned = Instantiate(recipeUIElement, recipesAnchor.position, Quaternion.identity, recipesAnchor);
            IngredientImage[] ingredientImages = spawned.GetComponentsInChildren<IngredientImage>(true);
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

        private void RequestHideCheckmark(IngredientTypeValue toHide)
        {
            for (int i = imagesList.Count - 1; i >= 0; i--)
            {
                if (imagesList[i].HideChecked(toHide))
                    break;
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