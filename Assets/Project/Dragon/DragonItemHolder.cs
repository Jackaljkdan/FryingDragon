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

        public Animator animator;

        public Transform itemPositionAnchor;
        public GameObject holdedItem;

        #endregion

        public void TryAddItem(GameObject objectToAdd)
        {
            if (holdedItem != null)
                return;

            AnimateRetriveItem(
                onRetrieveItemRelease: () => { SpawnItem(objectToAdd); }, onRetrieveEnd: () => { });
        }

        private void SpawnItem(GameObject item)
        {
            holdedItem = Instantiate(item, itemPositionAnchor.position, itemPositionAnchor.rotation, transform.root);
        }

        private UnityAction onPutItemRelease;

        public void AnimatePutItem(UnityAction onPutItemRelease)
        {
            this.onPutItemRelease = onPutItemRelease;
            animator.CrossFade("Project Attack Bite L", 0.1f);
        }

        private UnityAction onRetrieveItemRelease;
        private UnityAction onRetrieveEnd;

        public void AnimateRetriveItem(UnityAction onRetrieveEnd, UnityAction onRetrieveItemRelease)
        {
            this.onRetrieveEnd = onRetrieveEnd;
            this.onRetrieveItemRelease = onRetrieveItemRelease;
            animator.CrossFade("Project Attack Bite L", 0.1f);
        }

        public void OnPutItemRelease()
        {
            onPutItemRelease?.Invoke();
            onPutItemRelease = null;
        }

        public void OnRetriveItemRelease()
        {
            onRetrieveItemRelease?.Invoke();
            onRetrieveItemRelease = null;
        }

        public void OnRetriveEnd()
        {
            onRetrieveEnd?.Invoke();
            onRetrieveEnd = null;
        }

        public void DropItem()
        {
            if (holdedItem == null)
                return;

            holdedItem.transform.SetParent(transform.parent, worldPositionStays: true);

            if (holdedItem.TryGetComponent(out Bowl bowl))
            {
                bowl.Drop();
            }
            else if (holdedItem.TryGetComponent(out Rigidbody rb))
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.AddForce(transform.TransformDirection(Vector3.forward) * 3, ForceMode.Impulse);
            }

            holdedItem = null;
        }
    }
}