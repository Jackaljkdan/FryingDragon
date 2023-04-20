using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace JK.Utils.Addressables
{
    [Serializable]
    public class ClipAssetPlayer
    {
        public AudioSource audioSource;

        public bool IsLoading { get; private set; }

        public bool IsLoadingOrPlaying => IsLoading || audioSource.isPlaying;

        public ClipAssetPlayer() { }

        public ClipAssetPlayer(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }

        public void PlayAsync(AssetReferenceT<AudioClip> asset)
        {
            IsLoading = true;

            asset.LoadAssetAsyncIfNecessaryT().Completed += handle =>
            {
                IsLoading = false;

                audioSource.clip = asset.Cast();
                audioSource.Play();
            };
        }
    }
}