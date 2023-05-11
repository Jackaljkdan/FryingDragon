using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Project/Recipe")]
    public class RecipeScriptable : ScriptableObject
    {
        #region Inspector fields

        public Recipe recipe;

        #endregion
    }
}