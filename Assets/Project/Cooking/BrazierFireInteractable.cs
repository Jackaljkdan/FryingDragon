using JK.Injection;
using JK.Interaction;
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

            dragonAnimator.Play("Attack FireBall");

            cooking.StartCooking();
        }
    }
}