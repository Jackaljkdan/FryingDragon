using DG.Tweening;
using JK.Injection;
using JK.Interaction;
using Project.Dragon;
using Project.Items;
using Project.Jam.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class Packager : AbstractInteractable
    {
        #region Inspector

        public Transform bowlAnchor;

        public Bowl bowl;

        public UnityEvent onBowlRemoved;

        public GameObject coverParticles;
        public FarmerAnimator farmerAnimator;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        [Injected]
        private SignalBus signalBus;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonItemHolder = context.Get<DragonItemHolder>(this);
            signalBus = context.Get<SignalBus>(this);
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
                signalBus.Invoke(new ItemRemovedSignal());
                bowl.enabled = false;
                heldTransform.SetParent(dragonItemHolder.transform.parent, worldPositionStays: true);
                heldTransform.DOMove(bowlAnchor.position, 0.2f);
                heldTransform.DORotate(bowlAnchor.eulerAngles, 0.2f);
                dragonItemHolder.holdedItem = null;
                StartPacking();
            });
        }

        private void StartPacking()
        {
            farmerAnimator.PlayPack(5f);
            coverParticles.SetActive(true);
        }

        private void RetrieveBowl()
        {
            dragonItemHolder.AnimateRetriveItem(
                onRetrieveItemRelease: () =>
                {
                    onBowlRemoved.Invoke();
                    bowl.enabled = true;
                    bowl.transform.SetParent(transform.root, worldPositionStays: true);
                    bowl.transform.position = bowlAnchor.position;
                    bowl.transform.rotation = bowlAnchor.rotation;
                    dragonItemHolder.holdedItem = bowl.gameObject;
                    signalBus.Invoke(new ItemAddedSignal());
                },
                onRetrieveEnd: () =>
                {
                    bowl.UnGlueIngredients();
                    bowl = null;
                });
        }
    }
}