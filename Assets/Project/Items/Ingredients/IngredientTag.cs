using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items.Ingredients
{

    public enum IngredientTypeValue
    {
        Apple,
        Sandwich,
        Steak,
        DragonEgg,
        Potato,
        Fries,
        Burned = 999,
        box = 1000,
    }

    [DisallowMultipleComponent]
    public class IngredientTag : MonoBehaviour
    {
        public IngredientTypeValue ingredientTag;
    }
}