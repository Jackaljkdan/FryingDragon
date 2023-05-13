using JK.Dev.SceneSetup;
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
    public class RandomIngredientsTimedOrderRequester : AbstractTimedOrderRequester, IEditorSceneSetup
    {
        #region Inspector

        public List<IngredientTypeValue> availableIngredients = null;

        [DebugField]
        public int minIngredients = 2;

        [DebugField]
        public int maxIngredients = 5;

        [Injected]
        public LevelSettings levelSettings;

        #endregion

        private bool AreIngredientsInitialized => availableIngredients != null && availableIngredients.Count > 0;

        public override void Inject()
        {
            base.Inject();
            levelSettings = context.GetOptional<LevelSettings>();
        }

        protected override void Start()
        {
            base.Start();

            if (levelSettings != null)
            {
                minIngredients = levelSettings.minEggPerRecipe;
                maxIngredients = levelSettings.maxEggPerRecipe;
            }
            else
            {
                minIngredients = 2;
                maxIngredients = 3;

                Debug.LogWarning("no level settings in scene");
            }
        }

        private void InitIngredients()
        {
            foreach (IDispenser dispenser in transform.root.GetComponentsInChildren<IDispenser>())
                availableIngredients.Add(dispenser.IngredientType);

            if (availableIngredients.Count == 0)
                Debug.LogWarning("no available ingredients found");
        }

        protected override Recipe GetRecipeForOrder()
        {
            if (!AreIngredientsInitialized)
                InitIngredients();

            int randomIngredientsCount = UnityEngine.Random.Range(minIngredients, maxIngredients + 1);

            List<IngredientTypeValue> ingredients = new();

            for (int i = 0; i < randomIngredientsCount; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableIngredients.Count);
                ingredients.Add(availableIngredients[randomIndex]);
            }

            return new Recipe(ingredients);
        }

        public void EditorSceneSetup()
        {
            InitIngredients();
        }
    }
}