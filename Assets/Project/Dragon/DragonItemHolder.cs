using JK.Interaction;
using Project.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class DragonItemHolder : MonoBehaviour
    {
        #region Inspector

        public Transform itemPositionAnchor;
        public GameObject holdedItem;

        #endregion

        public void TryAddItem(GameObject objectToAdd)
        {
            if (holdedItem != null)
                return;

            SpawnItem(objectToAdd);
        }

        private void SpawnItem(GameObject item)
        {
            holdedItem = Instantiate(item, itemPositionAnchor.position, itemPositionAnchor.rotation, itemPositionAnchor);
        }


    }
}