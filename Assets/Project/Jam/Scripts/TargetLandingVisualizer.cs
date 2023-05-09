using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class TargetLandingVisualizer : MonoBehaviour
    {
        #region Inspector

        public LayerMask hitLayer;
        public GameObject hitObjectPrefab;

        #endregion

        private GameObject instatiatedPrefab;

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, hitLayer))
            {

                if (!instatiatedPrefab)
                    instatiatedPrefab = Instantiate(hitObjectPrefab, hit.point, Quaternion.identity, transform.root);

                Transform instanceTransform = instatiatedPrefab.transform;
                instanceTransform.position = hit.point;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!instatiatedPrefab)
                return;

            Destroy(instatiatedPrefab);
            this.enabled = false;
        }
    }
}