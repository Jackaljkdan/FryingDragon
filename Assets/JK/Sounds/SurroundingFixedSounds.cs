using JK.Injection;
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
    public class SurroundingFixedSounds : MonoBehaviour
    {
        #region Inspector

        public float averageSecondsBetweenClips = 10;

        public float minSecondsBetweenClips = 2;

        public RangeStruct xRange = new RangeStruct(-10, 10);
        public RangeStruct zRange = new RangeStruct(-10, 10);
        public RangeStruct yRange = new RangeStruct(0.5f, 1.8f);

        public List<AssetReferenceT<AudioClip>> clipAssets;

        public List<AudioSource> audioSources;

        public UnityEvent onPlay = new UnityEvent();

        [RuntimeField]
        public float nextClipTime;

        [Injected]
        public Transform player;

        private void Reset()
        {
            GetComponentsInChildren(audioSources);
        }

        [ContextMenu("Play Now Ignoring Schedule")]
        private void PlayNowIgnoringScheduleInEditMode()
        {
            if (Application.isPlaying)
                TryPlayRandomAtRandomPosition();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            player = context.Get<Transform>(this, "player");
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            ScheduleNextClip();
        }

        private void ScheduleNextClip()
        {
            nextClipTime = Time.time + minSecondsBetweenClips + RandomUtils.TimeUntilNextEvent(averageSecondsBetweenClips - minSecondsBetweenClips);
        }

        private bool TryGetAvailableAudioSource(out AudioSource available)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    available = audioSource;
                    return true;
                }
            }

            available = null;
            return false;
        }

        private void Update()
        {
            if (Time.time < nextClipTime)
                return;

            ScheduleNextClip();

            TryPlayRandomAtRandomPosition();
        }

        public void TryPlayRandomAtPosition(Vector3 position)
        {
            var randomClip = RandomUtils.Choose(clipAssets);
            randomClip.LoadAssetAsyncIfNecessary().Completed += _ =>
            {
                TryPlayAtPosition(randomClip.Cast(), position);
            };
        }

        public void TryPlayRandomAtPlayerRelativePosition(Vector3 position)
        {
            TryPlayRandomAtPosition(player.TransformPoint(position));
        }

        public void TryPlayRandomAtRandomPosition()
        {
            Vector3 randomOffset = new Vector3(
                xRange.RandomlySample(),
                yRange.RandomlySample(),
                zRange.RandomlySample()
            );
            TryPlayRandomAtPlayerRelativePosition(randomOffset);
        }

        public void TryPlayAtPlayerRelativePosition(AudioClip clip, Vector3 position)
        {
            TryPlayAtPosition(clip, player.TransformPoint(position));
        }

        public bool TryPlayAtPosition(AudioClip clip, Vector3 position)
        {
            if (!TryGetAvailableAudioSource(out AudioSource audioSource))
                return false;

            audioSource.transform.position = position;
            audioSource.PlayAs(clip, oneShot: false);

            onPlay.Invoke();

            return true;
        }
    }
}