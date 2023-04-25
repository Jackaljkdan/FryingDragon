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
        public Transform recipesAnchor;
        public List<IngredientImage> imagesList = new();

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

        private void OnNewRecipe(NewRecipeSignal arg)
        {
            List<IngredientTypeValue> ingredients = arg.recipe.ingredients;

            GameObject spawned = Instantiate(recipeUIElement, recipesAnchor.position, Quaternion.identity, transform);
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

            for (int i = 0; i < imagesList.Count; i++)
            {
                if (imagesList[i].ShowChecked(ingredient.ingredientTypeValue))
                    break;
            }
        }

        private void OnIngredientLost(IngredientLostSignal arg)
        {
            Ingredient ingredient = arg.ingredient;

            for (int i = imagesList.Count - 1; i >= 0; i--)
            {
                if (imagesList[i].HideChecked(ingredient.ingredientTypeValue))
                    break;
            }
        }

    }
}