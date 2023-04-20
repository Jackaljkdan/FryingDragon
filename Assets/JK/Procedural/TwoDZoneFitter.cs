using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class TwoDZoneFitter : ZoneFitter
    {
        #region Inspector



        #endregion

        [NonSerialized]
        public List<Collider2D> colliders;

        public override void Init()
        {
            base.Init();
            colliders = new List<Collider2D>(8);
            GetComponentsInChildren(colliders);
        }

        private static Collider2D[] overlapBuffer = new Collider2D[1];

        public override bool IsFitting()
        {
            foreach (var collider in colliders)
            {
                int overlapping = collider.OverlapCollider(
                    new ContactFilter2D()
                    {
                        useLayerMask = true,
                        layerMask = mask,
                    },
                    overlapBuffer
                );

                if (overlapping > 0)
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