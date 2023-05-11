using Project.Items;
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
        public Bowl bowl;
    }

    public struct CookingFinishedSignal
    {
        public Bowl bowl;
    }

    public struct CookingBurnedSignal
    {
        public Bowl bowl;
    }

    public struct CookingInterruptedSignal
    {
        public float cookingTime;
        public Bowl bowl;
    }
}