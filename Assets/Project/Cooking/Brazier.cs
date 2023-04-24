using DG.Tweening;
using JK.Injection;
using JK.Interaction;
using Project.Dragon;
using Project.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking
{
    [DisallowMultipleComponent]
    public class Brazier : AbstractInteractable
    {
        #region Inspector

        public Transform bowlAnchor;

        public Bowl bowl;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonItemHolder = context.Get<DragonItemHolder>(this);
        }

        private void Awake()
        {
            Inject();
        }

        protected override void InteractProtected(RaycastHit hit)
        {
            if (dragonItemHolder.holdedItem == bowl)
                return;

            if (bowl != null)
            {
                RetrieveBowl();
                return;
            }

            Transform heldTransform = dragonItemHolder.holdedItem.transform;

            if (!heldTransform.TryGetComponent(out bowl))
                return;

            bowl.GlueIngredients();

            dragonItemHolder.AnimatePutItem(onPutItemRelease: () =>
            {
                bowl.enabled = false;
                heldTransform.SetParent(dragonItemHolder.transform.parent, worldPositionStays: true);
                heldTransform.DOMove(bowlAnchor.position, 0.2f);
                heldTransform.DORotate(bowlAnchor.eulerAngles, 0.2f);
                dragonItemHolder.holdedItem = null;
            });
        }

        private void RetrieveBowl()
        {
            dragonItemHolder.AnimateRetriveItem(
                onRetrieveItemRelease: () =>
                {
                    bowl.enabled = true;
                    bowl.transform.SetParent(transform.root, worldPositionStays: true);
                    bowl.transform.position = bowlAnchor.position;
                    bowl.transform.rotation = bowlAnchor.rotation;
                    dragonItemHolder.holdedItem = bowl.gameObject;
                },
                onRetrieveEnd: () =>
                {
                    bowl.UnGlueIngredients();
                    bowl = null;
                });
        }
    }
}