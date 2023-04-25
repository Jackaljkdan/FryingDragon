using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [Serializable]
    public class Recipe
    {
        public List<IngredientTypeValue> ingredients;
        public Recipe(List<IngredientTypeValue> ingredients)
        {
            this.ingredients = new List<IngredientTypeValue>(ingredients);
        }
    }
}