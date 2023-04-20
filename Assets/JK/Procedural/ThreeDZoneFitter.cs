using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class ThreeDZoneFitter : ZoneFitter
    {
        #region Inspector



        #endregion

        [NonSerialized]
        public List<BoxCollider> colliders;

        public override void Init()
        {
            base.Init();
            colliders = new List<BoxCollider>(8);
            GetComponentsInChildren(colliders);
        }

        private static Collider[] overlapBuffer = new Collider[2];

        public override bool IsFitting()
        {
            foreach (var collider in colliders)
            {
                Transform colliderTransform = collider.transform;

                int overlapping = Physics.OverlapBoxNonAlloc(
                    colliderTransform.position,
                    Vector3.Scale(collider.size / 2, colliderTransform.lossyScale.Abs()),
                    overlapBuffer,
                    colliderTransform.rotation,
                    mask,
                    QueryTriggerInteraction.Ignore
                );

                // overlappo sempre almeno con me stesso
                if (overlapping > 1)
                    return false;
            }

            return true;
        }

        public override IEnumerable<Component> EnumerateColliders()
        {
            return colliders;
        }
    }
}