using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace JK.Procedural
{
    public abstract class ZoneFitter : MonoBehaviour
    {
        #region Inspector

        public LayerMask mask;

        [RuntimeHeader]

        [FormerlySerializedAs("room")]
        public Zone zone;

        [DebugHeader]

        [ReadOnly]
        public bool _isFittingInEditMode;

        [ContextMenu("Log is fitting")]
        public void LogIsFittingInEditMode()
        {
            Awake();
            Debug.Log("is fitting: " + IsFitting());
        }

        [ContextMenu("Validate")]
        private void Validate()
        {
            if (!Application.isPlaying)
                Init();

            _isFittingInEditMode = IsFitting();
        }

        #endregion

        protected virtual void Awake()
        {
            /// devo prepararmi affinché <see cref="IsFitting"/> possa essere chiamato prima di Start
            Init();
        }

        public virtual void Init()
        {
            if (!PlatformUtils.IsEditor || Application.isPlaying)
            {
                zone = GetComponentInParent<Zone>();
                name = $"{zone.name} {name}";
            }
        }

        public abstract bool IsFitting();

        public abstract IEnumerable<Component> EnumerateColliders();
    }
}