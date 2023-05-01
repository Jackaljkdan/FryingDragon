using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace JK.Sounds
{
    [DisallowMultipleComponent]
    public class RandomClipsPlayer : MonoBehaviour
    {
        #region Inspector

        public bool oneShot = true;

        public List<AssetReferenceT<AudioClip>> clipAssets;

        public AudioSource audioSource;

        private void Reset()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        public void PlayRandomClip()
        {
            if (clipAssets == null || clipAssets.Count == 0 || audioSource == null)
                return;

            audioSource.PlayAssetSafelyAsyncAs(RandomUtils.Choose(clipAssets), oneShot);
        }
    }
}