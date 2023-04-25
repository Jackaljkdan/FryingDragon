using JK.Injection;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class RecipeUISpawner : MonoBehaviour
    {
        #region Inspector

        public GameObject recipeUIElement;
        public List<Transform> recipeAnchors;

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
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<NewRecipeSignal>(OnNewRecipe);
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

            GameObject spawned = Instantiate(recipeUIElement, recipesAnchor.position, Quaternion.identity, recipesAnchor);
            spawned.GetComponent<RecipesVisualizer>().ShowRecipe(arg.recipe);
        }
    }
}