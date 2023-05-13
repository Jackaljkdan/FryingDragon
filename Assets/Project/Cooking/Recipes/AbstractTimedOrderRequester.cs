using JK.Injection;
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
    public abstract class AbstractTimedOrderRequester : MonoBehaviour
    {
        #region Inspector

        public float minWaitSeconds = 1f;
        public float maxWaitSeconds = 3f;

        [RuntimeField]
        public float nextRecipeSeconds;

        [Injected]
        public OrderFulfiller orderFulfiller;

        [ContextMenu("Request Recipe")]
        public void RequestRecipeFromInspector()
        {
            if (Application.isPlaying)
                orderFulfiller.TryRequestNewRecipe(GetRecipeForOrder);
        }

        [ContextMenu("Request All Recipes")]
        public void RequestAllRecipesFromInspector()
        {
            if (!Application.isPlaying)
                return;

            for (int i = 0; i < orderFulfiller.maxRecipes; i++)
                orderFulfiller.TryRequestNewRecipe(GetRecipeForOrder);
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            orderFulfiller = context.Get<OrderFulfiller>(this);
        }

        private void Awake()
        {
            Inject();
        }

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
            orderFulfiller.TryRequestNewRecipe(GetRecipeForOrder);
            ScheduleRepeatingRequestRecipe();
        }

        private void ScheduleRepeatingRequestRecipe()
        {
            nextRecipeSeconds = UnityEngine.Random.Range(minWaitSeconds, maxWaitSeconds);
            Invoke(nameof(RequestRecipeAndReschedule), nextRecipeSeconds);
        }

        protected abstract Recipe GetRecipeForOrder();
    }
}