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
        public new DragonFireAnimation animation;

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
            animation = context.Get<DragonFireAnimation>(this);
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

            animation.PlayFireAnimation();

            cooking.StartCooking();
        }
    }
}