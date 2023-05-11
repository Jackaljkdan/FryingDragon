using JK.Actuators;
using JK.Actuators.Input;
using JK.Injection;
using JK.UI;
using JK.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    [EarlyExecutionOrder]
    public class DragonInput : AbsoluteMovementInput
    {
        #region Inspector

        public AxisAsButtonClass dropButton = new AxisAsButtonClass("Drop");

        public DragonItemHolder dragonItemHolder;
        public DragonInteractore dragonInteractore;

        protected override void Reset()
        {
            base.Reset();
            dragonItemHolder = GetComponent<DragonItemHolder>();
        }

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            dragonInteractore.enabled = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dragonInteractore.enabled = false;
        }

        private new void Update()
        {
            if (dropButton.GetAxisDown())
                dragonItemHolder.DropItem();

            base.Update();
        }
    }
}