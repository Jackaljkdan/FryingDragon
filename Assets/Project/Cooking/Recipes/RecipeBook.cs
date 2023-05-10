using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class RecipeBook : MonoBehaviour
    {
        #region Inspector

        public List<Recipe> recipes = new();

        #endregion

        public Recipe GetRandomRecipe()
        {
            int randomIndex = Random.Range(0, recipes.Count);
            return recipes[randomIndex];
        }
    }
}