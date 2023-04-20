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
    public class MovementSounds : MonoBehaviour
    {
        #region Inspector

        public float threshold = 0.1f;

        public float normalizedOverlap = 0.5f;

        public Transform movingTarget;

        public List<AssetReferenceT<AudioClip>> clipAssets = new List<AssetReferenceT<AudioClip>>();

        public AudioSource audioSource;

        [DebugField]
        public float movement;

        private void Reset()
        {
            movingTarget = transform;
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        private Vector3 lastPosition;

        private Coroutine playSoundCoroutine;

        private bool IsPlaying => playSoundCoroutine != null;

        private void Start()
        {
            lastPosition = movingTarget.position;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Update()
        {
            if (!IsPlaying)
            {
                movement = Vector3.Distance(lastPosition, movingTarget.position);

                if (movement >= threshold)
                    PlayRandomSound(volumeScale: Mathf.InverseLerp(threshold, threshold * 10, movement));
            }

            lastPosition = movingTarget.position;
        }

        private void PlayRandomSound(float volumeScale)
        {
            playSoundCoroutine = StartCoroutine(coroutine());

            IEnumerator coroutine()
            {
                var randomClipAsset = RandomUtils.Choose(clipAssets);
                yield return randomClipAsset.LoadAssetAsyncIfNecessaryT().WaitUntilDone();

                var randomClip = randomClipAsset.Cast();

                if (randomClip == null || audioSource == null)
                    yield break;

                audioSource.PlayOneShot(randomClip, volumeScale);

                yield return new WaitForSeconds(randomClip.length * (1 / audioSource.pitch) * normalizedOverlap);

                playSoundCoroutine = null;
            }
        }

        public void PlayIndependentRandomSound()
        {
            PlayIndependentRandomSound(volumeScale: 1);
        }

        public void PlayIndependentRandomSound(float volumeScale)
        {
            audioSource.PlayAssetAsyncAs(RandomUtils.Choose(clipAssets), oneShot: true, volumeScale);
        }
    }
}