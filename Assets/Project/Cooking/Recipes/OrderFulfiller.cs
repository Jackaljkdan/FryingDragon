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
    public class OrderFulfiller : MonoBehaviour
    {
        #region Inspector

        public int maxRecipes = 3;
        public List<Recipe> recipes = new();

        [Injected]
        private SignalBus signalBus;

        [ContextMenu("Request Recipe")]
        public void RequestRecipeFromInspector()
        {
            RequestNewRecipe();
        }

        [ContextMenu("Request All Recipes")]
        public void RequestAllRecipesFromInspector()
        {
            for (int i = 0; i < maxRecipes; i++)
            {
                RequestNewRecipe();
            }
        }
        #endregion

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

        public void TryRequestNewRecipe()
        {
            if (recipes.Count < maxRecipes)
                RequestNewRecipe();
        }

        private void RequestNewRecipe()
        {
            if (recipes.Count >= maxRecipes)
                return;

            Recipe newRecipe = new Recipe(GetRandomIngredients(3));
            recipes.Add(newRecipe);
            signalBus.Invoke(new NewRecipeSignal() { recipe = newRecipe });

        }

        private List<IngredientTypeValue> GetRandomIngredients(int count)
        {
            List<IngredientTypeValue> ingredients = new();

            for (int i = 0; i < count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(IngredientTypeValue)).Length);
                IngredientTypeValue randomIngredient = (IngredientTypeValue)randomIndex;
                ingredients.Add(randomIngredient);
            }

            return ingredients;
        }
    }
}