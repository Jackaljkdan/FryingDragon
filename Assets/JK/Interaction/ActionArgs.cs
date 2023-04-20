using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [Serializable]
    public struct ActionArgs
    {
        public Vector3 raycastOrigin;
        public Vector3 raycastDirection;
        public bool hasHit;
        public RaycastHit hit;

        public ActionArgs(Vector3 raycastOrigin, Vector3 raycastDirection, bool hasHit, RaycastHit hit)
        {
            this.raycastOrigin = raycastOrigin;
            this.raycastDirection = raycastDirection;
            this.hasHit = hasHit;
            this.hit = hit;
        }
    }
}