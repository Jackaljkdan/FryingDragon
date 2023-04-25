using DG.Tweening;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items
{
    [DisallowMultipleComponent]
    public class ItemDisplay : MonoBehaviour
    {
        #region Inspector

        public GameObject itemToDisplay;
        public Transform spawnAnchor;



        [ContextMenu("SetItem")]
        public void SetItemFromInspector()
        {
            if (transform.TryGetComponent<ItemDispenser>(out ItemDispenser it))
                itemToDisplay = it.grabbableItem;
        }
        #endregion
        private readonly float floatDistance = 0.2f;

        private void Start()
        {
            if (!itemToDisplay)
                return;

            GameObject spawn = Instantiate(itemToDisplay, spawnAnchor.position, spawnAnchor.rotation, spawnAnchor);

            spawn.TryGetComponent<MeshRenderer>(out MeshRenderer renderer);
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            if (spawn.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
                spawn.transform.DOLocalRotate(new Vector3(0, 360, 0), 5, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart);

                spawn.transform.DOLocalMoveY(transform.localPosition.y + floatDistance, 2)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}