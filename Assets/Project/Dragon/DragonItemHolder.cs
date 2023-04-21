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
        public GameObject item;

        #endregion

        public bool TryAddItem(GameObject itemToadd)
        {
            if (item != null)
                return false;

            item = itemToadd;
            SpawnItem();
            return true;
        }

        private void SpawnItem()
        {
            Instantiate(item, itemPositionAnchor.position, itemPositionAnchor.rotation, transform);
        }
    }
}