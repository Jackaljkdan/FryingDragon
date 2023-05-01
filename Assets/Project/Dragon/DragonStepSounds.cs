using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class DragonStepSounds : MonoBehaviour
    {
        #region Inspector

        public List<int> triggerFrames;

        public int totalFrames;

        public List<AssetReferenceT<AudioClip>> clipAssets;

        public Animator animator;

        public AudioSource audioSource;

        [RuntimeField]
        public float lastPlayNormalizedTime;

        [DebugField]
        public float stateNormalizedTime;

        [DebugField]
        public float nextNormalizedTime;

        private void Reset()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        private int moveHash;
        private int xHash;
        private int zHash;

        private void Start()
        {
            moveHash = Animator.StringToHash("Move");
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");
            triggerFrames.Sort();
        }

        private void OnEnable()
        {
            lastPlayNormalizedTime = 0;
        }

        private bool IsThereEnoughInput()
        {
            float threshold = 0.06f;
            return Mathf.Abs(animator.GetFloat(xHash)) >= threshold || Mathf.Abs(animator.GetFloat(zHash)) >= threshold;
        }

        private void Update()
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);

            stateNormalizedTime = state.normalizedTime;

            if (state.shortNameHash != moveHash || !IsThereEnoughInput())
            {
                lastPlayNormalizedTime = state.normalizedTime;
                return;
            }

            foreach (int frame in triggerFrames)
            {
                float frameNormalizedTimeInLoop = ((float)frame) / totalFrames;
                float frameNormalizedTime = frameNormalizedTimeInLoop + (int)state.normalizedTime;

                if (frameNormalizedTime > state.normalizedTime)
                {
                    nextNormalizedTime = frameNormalizedTime;
                    break;
                }

                if (frameNormalizedTime <= lastPlayNormalizedTime)
                    continue;

                lastPlayNormalizedTime = frameNormalizedTime;
                audioSource.PlayAssetSafelyAsyncAs(RandomUtils.Choose(clipAssets), oneShot: true);
                break;
            }
        }
    }
}