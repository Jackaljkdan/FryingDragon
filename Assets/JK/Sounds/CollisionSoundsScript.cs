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
    public class CollisionSoundsScript : MonoBehaviour
    {
        #region Inspector

        public float maxVolumeSpeed = 1f;

        public float minSpeedSecondsBetweenClips = 1f;
        public float maxSpeedSecondsBetweenClips = 0.2f;

        public List<AssetReferenceT<AudioClip>> clipAssets = new List<AssetReferenceT<AudioClip>>();

        public AudioSource audioSource;

        [RuntimeField]
        public float lastClipTime;

        private void Reset()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            PlayClipIfAppropriate(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            PlayClipIfAppropriate(collision);
        }

        private void PlayClipIfAppropriate(Collision collision)
        {
            float relativeSpeed = collision.relativeVelocity.magnitude;

            float volumeScale = Mathf.Clamp01(relativeSpeed / maxVolumeSpeed);

            float secondsBetweenClips = Mathf.Lerp(minSpeedSecondsBetweenClips, maxSpeedSecondsBetweenClips, volumeScale);

            //Debug.Log($"rv: {relativeSpeed:0.000} vol: {volumeScale:0.000} s: {secondsBetweenClips:0.0}");

            if (Time.time - lastClipTime < secondsBetweenClips)
                return;

            audioSource.PlayAssetSafelyAsyncAs(RandomUtils.Choose(clipAssets), oneShot: true, volumeScale);

            lastClipTime = Time.time;
        }
    }
}