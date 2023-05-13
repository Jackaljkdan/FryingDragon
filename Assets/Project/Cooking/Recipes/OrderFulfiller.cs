using JK.Injection;
using JK.Utils;
using Project.Dispensers;
using Project.Items.Ingredients;
using Project.Jam;
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

        public UnityEvent onRecipeFulfilled;

        [DebugField]
        public List<IngredientTypeValue> availableIngredients = new();

        [DebugField]
        public int minIngredients = 2;

        [DebugField]
        public int maxIngredients = 5;

        [Injected]
        private SignalBus signalBus;

        [Injected]
        public LevelSettings levelSettings;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);
            levelSettings = context.GetOptional<LevelSettings>();
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            signalBus.AddListener<OrderFulfilledSignal>(FulfillRecipe);

            foreach (IDispenser dispenser in transform.root.GetComponentsInChildren<IDispenser>())
                availableIngredients.Add(dispenser.IngredientType);

            if (availableIngredients.Count == 0)
                Debug.LogWarning("no available ingredients found");

            if (levelSettings != null)
            {
                minIngredients = levelSettings.minEggPerRecipe;
                maxIngredients = levelSettings.maxEggPerRecipe;
            }
            else
            {
                minIngredients = 2;
                maxIngredients = 3;
            }
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<OrderFulfilledSignal>(FulfillRecipe);
        }

        private void FulfillRecipe(OrderFulfilledSignal signal)
        {
            if (!recipes.Contains(signal.recipe))
                return;

            onRecipeFulfilled.Invoke();
            recipes.Remove(signal.recipe);
        }

        public void TryRequestNewRecipe(Func<Recipe> recipeFn)
        {
            if (recipes.Count >= maxRecipes)
                return;

            Recipe newRecipe = recipeFn();
            recipes.Add(newRecipe);
            signalBus.Invoke(new NewRecipeSignal() { recipe = newRecipe });
        }

        private List<IngredientTypeValue> GetRandomIngredients(int count)
        {
            List<IngredientTypeValue> ingredients = new();

            for (int i = 0; i < count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableIngredients.Count);
                ingredients.Add(availableIngredients[randomIndex]);
            }

            return ingredients;
        }
    }
}