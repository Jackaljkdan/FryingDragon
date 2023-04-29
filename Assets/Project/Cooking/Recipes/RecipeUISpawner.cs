using DG.Tweening;
using JK.Injection;
using JK.Utils;
using Project.Items.Ingredients;
using Project.Jam;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class RecipeUISpawner : MonoBehaviour
    {
        #region Inspector

        public RecipesVisualizer prefab;

        public RectTransform parent;

        public List<RecipesVisualizer> spawnedRecipes;

        [Injected]
        private SignalBus signalBus;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);
        }

        #endregion

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            signalBus.AddListener<NewRecipeSignal>(OnNewRecipe);
            signalBus.AddListener<CookingStartedSignal>(OnCookingStarted);
            signalBus.AddListener<CookingInterruptedSignal>(OnCookingInterrupted);
            signalBus.AddListener<OrderFulfilledSignal>(OnCookingStartedSignal);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<NewRecipeSignal>(OnNewRecipe);
            signalBus.RemoveListener<CookingStartedSignal>(OnCookingStarted);
            signalBus.RemoveListener<CookingInterruptedSignal>(OnCookingInterrupted);
            signalBus.RemoveListener<OrderFulfilledSignal>(OnCookingStartedSignal);
        }

        private void OnCookingStartedSignal(OrderFulfilledSignal signal)
        {
            foreach (RecipesVisualizer recipesVisualizer in spawnedRecipes)
            {
                if (recipesVisualizer.IsRecipeFulfilled(signal.ingredients))
                {
                    recipesVisualizer.DOExit().OnComplete(() =>
                    {
                        spawnedRecipes.Remove(recipesVisualizer);
                        Destroy(recipesVisualizer.gameObject);
                    });
                    break;
                }
            }
        }

        private void OnNewRecipe(NewRecipeSignal arg)
        {
            RecipesVisualizer spawned = Instantiate(prefab, parent);
            spawnedRecipes.Add(spawned);
            spawned.DOEnter();
            spawned.ShowRecipe(arg.recipe);
        }

        private void OnCookingStarted(CookingStartedSignal arg)
        {
            foreach (RecipesVisualizer recipesVisualizer in spawnedRecipes)
            {
                if (recipesVisualizer.IsRecipeCooking(arg.ingredients, arg.cookingTime))
                    break;
            }
        }

        private void OnCookingInterrupted(CookingInterruptedSignal args)
        {

            foreach (RecipesVisualizer recipesVisualizer in spawnedRecipes)
            {
                if (recipesVisualizer.ShouldRecipeStop(args.ingredients))
                    break;
            }
        }
    }
}