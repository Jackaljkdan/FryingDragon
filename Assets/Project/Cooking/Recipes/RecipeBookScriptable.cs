using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [CreateAssetMenu(fileName = "RecipeBook", menuName = "Project/RecipeBook")]
    public class RecipeBookScriptable : ScriptableObject
    {
        #region Inspector fields

        public List<RecipeScriptable> recipes = new();

        #endregion

        public Recipe GetRandomRecipe()
        {
            int randomIndex = UnityEngine.Random.Range(0, recipes.Count);
            return recipes[randomIndex].recipe;
        }
    }
}