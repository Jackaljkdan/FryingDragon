using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items.Ingredients
{
    [Serializable]
    public struct IngredientLostSignal
    {
        public Ingredient ingredient;
        public List<Ingredient> availableIngredients;
    }

    [Serializable]
    public struct IngredientTakenSignal
    {
        public Ingredient ingredient;
    }
}