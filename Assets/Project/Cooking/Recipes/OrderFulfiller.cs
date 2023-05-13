using JK.Injection;
using JK.Utils;
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

        [Injected]
        private SignalBus signalBus;

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

        private void Start()
        {
            signalBus.AddListener<OrderFulfilledSignal>(FulfillRecipe);
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
    }
}