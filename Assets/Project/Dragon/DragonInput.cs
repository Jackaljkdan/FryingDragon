using JK.Utils;
using Project.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    [EarlyExecutionOrder]
    public class DragonInput : MonoBehaviour
    {
        #region Inspector

        public AxisAsButtonClass dropButton = new AxisAsButtonClass("Drop");

        public DragonMovement dragonMovement;
        public DragonItemHolder dragonItemHolder;

        private void Reset()
        {
            dragonMovement = GetComponent<DragonMovement>();
            dragonItemHolder = GetComponent<DragonItemHolder>();
        }

        #endregion

        private void Update()
        {
            dragonMovement.Move(new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            ));

            if (dropButton.GetAxisDown())
                dragonItemHolder.DropItem();
        }
    }
}