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
    public class FootstepsOnAnimatorEventScript : MonoBehaviour
    {
        #region Inspector

        public List<AssetReferenceT<AudioClip>> clipAssets;

        public AudioSource leftSource;
        public AudioSource rightSource;

        public float minSecondsBetweenClips = 0.1f;

        [RuntimeField]
        public float lastFootstepTime;

        #endregion

        private void OnEnable()
        {
            lastFootstepTime = -minSecondsBetweenClips;
        }

        public void OnRightFootstep()
        {
            if (!enabled)
                return;

            if (Time.time - lastFootstepTime < minSecondsBetweenClips)
                return;

            lastFootstepTime = Time.time;
            rightSource.PlayAssetAsyncAs(RandomUtils.Choose(clipAssets), oneShot: true);
        }

        public void OnLeftFootstep()
        {
            if (!enabled)
                return;

            if (Time.time - lastFootstepTime < minSecondsBetweenClips)
                return;

            lastFootstepTime = Time.time;
            leftSource.PlayAssetAsyncAs(RandomUtils.Choose(clipAssets), oneShot: true);
        }
    }
}