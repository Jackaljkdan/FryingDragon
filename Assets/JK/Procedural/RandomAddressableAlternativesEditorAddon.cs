using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RandomAddressableAlternatives))]
    [ExecuteAlways]
    public class RandomAddressableAlternativesEditorAddon : MonoBehaviour
    {
        #region Inspector

        public bool showFirstInEditor = true;

        #endregion

        private void Awake()
        {
            if (!PlatformUtils.IsEditor || Application.isPlaying)
                return;

            if (PrefabUtils.IsPrefabMode())
                return;

            if (showFirstInEditor)
                GetComponent<RandomAddressableAlternatives>().SpawnFirstInEditMode();
        }
    }
}