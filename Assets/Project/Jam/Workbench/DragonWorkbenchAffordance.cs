using JK.Injection;
using JK.Interaction;
using JK.Observables;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam.Workbench
{
    [DisallowMultipleComponent]
    public class DragonWorkbenchAffordance : AbstractAffordance
    {
        #region Inspector

        public Packager packager;

        public Outline workbenchOutline;
        public Outline farmerOutline;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        private void Reset()
        {
            packager = GetComponentInParent<Packager>();
        }

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

        protected override void Start()
        {
            base.Start();
            StopHighlightProtected();
        }

        private void OnEnable()
        {
            packager.isSleeping.onChange.AddListener(OnSleepingChange);
            dragonItemHolder.heldItem.onChange.AddListener(OnHeldItemChange);
            StopHighlightProtected();
        }

        private void OnDisable()
        {
            packager.isSleeping.onChange.RemoveListener(OnSleepingChange);
            dragonItemHolder.heldItem.onChange.RemoveListener(OnHeldItemChange);
            StopHighlightProtected();
        }

        private void OnSleepingChange(ObservableProperty<bool>.Changed arg)
        {
            if (!IsHighlighting)
                return;

            RefreshHighlighting();
        }

        public void RefreshHighlighting()
        {
            if (dragonItemHolder.heldItem.Value != null)
            {
                workbenchOutline.enabled = true;
                farmerOutline.enabled = false;
            }
            else
            {
                workbenchOutline.enabled = !packager.isSleeping.Value;
                farmerOutline.enabled = packager.isSleeping.Value;
            }
        }

        private void OnHeldItemChange(ObservableProperty<GameObject>.Changed arg)
        {
            if (!IsHighlighting)
                return;

            RefreshHighlighting();
        }

        protected override void StartHighlightProtected(RaycastHit hit)
        {
            RefreshHighlighting();
        }

        protected override void StopHighlightProtected()
        {
            workbenchOutline.enabled = false;
            farmerOutline.enabled = false;
        }
    }
}