using JK.Injection;
using JK.Interaction;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking
{
    [DisallowMultipleComponent]
    public class BrazierFireInteractable : AbstractInteractable
    {
        #region Inspector

        public Brazier brazier;

        public Cooking cooking;

        [Injected]
        public Animator dragonAnimator;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        private void Reset()
        {
            brazier = GetComponent<Brazier>();
            cooking = GetComponent<Cooking>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonAnimator = context.Get<Animator>(this, "dragon");
            dragonItemHolder = context.Get<DragonItemHolder>(this);
        }

        private void Awake()
        {
            Inject();
        }

        protected override void InteractProtected(RaycastHit hit)
        {
            if (brazier.bowl == null)
                return;

            if (cooking.IsCooking)
                return;

            if (dragonItemHolder.holdedItem != null)
                return;

            dragonAnimator.CrossFade("Attack FireBall", 0.1f);

            cooking.StartCooking();
        }
    }
}