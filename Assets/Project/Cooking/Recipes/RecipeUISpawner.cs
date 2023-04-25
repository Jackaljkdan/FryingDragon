using DG.Tweening;
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
    public class RecipeUISpawner : MonoBehaviour
    {
        #region Inspector

        public RecipesVisualizer prefab;

        public RectTransform parent;

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

        private void OnNewRecipe(NewRecipeSignal arg)
        {
            RecipesVisualizer spawned = Instantiate(prefab, parent);
            spawned.DOEnter();
            spawned.ShowRecipe(arg.recipe);
        }
    }
}