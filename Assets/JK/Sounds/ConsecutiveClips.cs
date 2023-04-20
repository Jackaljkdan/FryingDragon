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
    public class ConsecutiveClips : MonoBehaviour
    {
        #region Inspector

        public AudioSource audioSource;

        public List<AssetReferenceT<AudioClip>> clipAssets = new List<AssetReferenceT<AudioClip>>();

        public bool avoidConsecutiveRepetition = false;

        public float averageSecondsBetweenClips = 0;
        public float deltaSecondsBetweenClips = 0;

        public bool oneShot = true;

        public UnityEvent onPlay = new UnityEvent();

        private void Reset()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        private Coroutine coroutine;

        private int lastChosenIndex = -1;

        private void Start()
        {
            audioSource.loop = false;
        }

        private void OnEnable()
        {
            coroutine = StartCoroutine(PlayClipsCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(coroutine);
            CancelEnable();
        }

        private IEnumerator PlayClipsCoroutine()
        {
            while (true)
            {
                var randomClipAsset = avoidConsecutiveRepetition
                    ? RandomUtils.ChooseExcept(clipAssets, lastChosenIndex, out lastChosenIndex)
                    : RandomUtils.Choose(clipAssets)
                ;

                yield return randomClipAsset.LoadAssetAsyncIfNecessaryT().WaitUntilDone();

                var randomClip = randomClipAsset.Cast();

                audioSource.PlayAs(randomClip, oneShot);

                onPlay.Invoke();

                float betweenSeconds = RandomUtils.TimeUntilNextEvent(averageSecondsBetweenClips, deltaSecondsBetweenClips);

                yield return new WaitForSeconds(randomClip.length + betweenSeconds);
            }
        }

        public void EnableAfterSeconds(float seconds)
        {

            Invoke(nameof(Enable), seconds);
        }

        public void EnableAfterBetweenSeconds(float additionalSeconds = 0)
        {
            float betweenSeconds = RandomUtils.TimeUntilNextEvent(averageSecondsBetweenClips, deltaSecondsBetweenClips);
            Invoke(nameof(Enable), betweenSeconds + additionalSeconds);
        }

        public void CancelEnable()
        {
            CancelInvoke(nameof(Enable));
        }

        private void Enable()
        {
            enabled = true;
        }
    }
}