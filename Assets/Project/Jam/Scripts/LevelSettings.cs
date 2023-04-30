using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class LevelSettings : MonoBehaviour
    {
        #region Inspector

        public int level = 1;

        public int boxesTodo = 12;

        public float minutes = 5;

        public float secondsUntilSleep = 20;

        public float packingSeconds = 10;

        public int minEggPerRecipe = 2;
        public int maxEggPerRecipe = 3;


        #endregion
    }
}