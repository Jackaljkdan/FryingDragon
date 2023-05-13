using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class RecipeBookTimedOrderRequester : AbstractTimedOrderRequester
    {
        #region Inspector

        public RecipeBookScriptable recipeBook;

        #endregion

        protected override Recipe GetRecipeForOrder()
        {
            return recipeBook.GetRandomRecipe();
        }
    }
}