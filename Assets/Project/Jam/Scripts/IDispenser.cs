using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    public interface IDispenser
    {
        IngredientTypeValue ingredientType { get; }
    }
}