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
            Invoke(nameof(RequestRecipeAndReschedule), 2f);
            orderFulfiller.onRecipeFulfilled.AddListener(ScheduleNextRequest);
        }

        private void OnDestroy()
        {
            orderFulfiller.onRecipeFulfilled.RemoveListener(ScheduleNextRequest);
        }

        private void ScheduleNextRequest()
        {
            CancelInvoke(nameof(RequestRecipeAndReschedule));
            ScheduleRepeatingRequestRecipe();
        }

        private void RequestRecipeAndReschedule()
        {
            orderFulfiller.TryRequestNewRecipe();
            ScheduleRepeatingRequestRecipe();
        }

        private void ScheduleRepeatingRequestRecipe()
        {
            nextRecipeTime = UnityEngine.Random.Range(minWaitSeconds, maxWaitSeconds);
            Invoke(nameof(RequestRecipeAndReschedule), nextRecipeTime);
        }
    }
}