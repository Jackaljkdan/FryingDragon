using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class RecipeRequester : MonoBehaviour
    {
        #region Inspector

        public float minWaitSeconds = 5f;
        public float maxWaitSeconds = 20f;

        public OrderFulfiller orderFulfiller;

        [RuntimeField]
        public float nextRecipeTime;

        #endregion


        private void Start()
        {
            ScheduleNextRequest();
            orderFulfiller.onRecipeFulfilled.AddListener(ScheduleNextRequest);
        }

        private void OnDestroy()
        {
            orderFulfiller.onRecipeFulfilled.RemoveListener(ScheduleNextRequest);
        }

        private void ScheduleNextRequest()
        {
            nextRecipeTime = Time.time + UnityEngine.Random.Range(minWaitSeconds, maxWaitSeconds);
        }

        private void Update()
        {
            if (Time.time < nextRecipeTime)
                return;

            ScheduleNextRequest();

            orderFulfiller.TryRequestNewRecipe();
        }

    }
}