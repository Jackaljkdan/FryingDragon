using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace JK.Utils.Addressables
{
    public static class AudioSourceAddressablesUtils
    {
        public static void PlayAssetAsyncAs(this AudioSource self, AssetReferenceT<AudioClip> clipAsset, bool oneShot, float oneShotVolumeScale = 1)
        {
            clipAsset.GetAsync(() => self.PlayAs(clipAsset.Cast(), oneShot, oneShotVolumeScale));
        }

        public static void PlayAssetSafelyAsyncAs(this AudioSource self, AssetReferenceT<AudioClip> clipAsset, bool oneShot, float oneShotVolumeScale = 1)
        {
            clipAsset.GetSafelyAsync(self, () => self.PlayAs(clipAsset.Cast(), oneShot, oneShotVolumeScale));
        }

        public static IEnumerator PlayAssetAsCoroutine(this AudioSource self, AssetReferenceT<AudioClip> clipAsset, bool oneShot, float oneShotVolumeScale = 1)
        {
            yield return clipAsset.LoadAssetAsyncIfNecessary().WaitUntilDone();
            self.PlayAs(clipAsset.Cast(), oneShot, oneShotVolumeScale);
        }
    }
}