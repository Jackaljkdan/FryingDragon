using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items
{
    [DisallowMultipleComponent]
    public class ItemDispenser : MonoBehaviour
    {
        #region Inspector

        public GameObject grabbableItem;

        #endregion
        private DragonItemHolder itemHolder;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemHolder != null)
                {
                    itemHolder.TryAddItem(grabbableItem);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<DragonItemHolder>(out DragonItemHolder item))
            {
                itemHolder = item;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<DragonItemHolder>(out _))
            {
                itemHolder = null;
            }
        }


    }
}