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

        public bool TryAddItem(GameObject itemToadd)
        {
            if (holdedItem != null)
                return false;

            SpawnItem(itemToadd);
            return true;
        }

        private void SpawnItem(GameObject item)
        {
            holdedItem = Instantiate(item, itemPositionAnchor.position, itemPositionAnchor.rotation, itemPositionAnchor);
        }
    }
}