using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [Serializable]
    public struct NewRecipeSignal
    {
        public Recipe recipe;
    }
}