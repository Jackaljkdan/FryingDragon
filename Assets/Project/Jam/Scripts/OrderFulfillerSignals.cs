using Project.Cooking.Recipes;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [Serializable]
    public struct OrderFulfilledSignal
    {
        public Recipe recipe;
        public List<IngredientTypeValue> actualIngredients;
    }
}