using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking
{

    [Serializable]
    public struct CookingStartedSignal
    {
        public float cookingTime;
        public List<IngredientTypeValue> ingredients;
    }
}