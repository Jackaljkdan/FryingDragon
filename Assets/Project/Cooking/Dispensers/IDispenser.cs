using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dispensers
{
    public interface IDispenser
    {
        IngredientTypeValue IngredientType { get; }
    }
}