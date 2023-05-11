using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class IkApplier : MonoBehaviour
    {
        #region Inspector

        public IkAnchors ikAnchors;

        public float ikWeight;

        public Animator animator;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        #endregion

        private void Start()
        {

        }

        private void OnAnimatorIK(int layerIndex)
        {
            ikAnchors.SetIKWithWeight(animator, ikWeight);
        }

        private Tween ikWeightTween;

        public Tween DOIkWeight(float endValue, float seconds)
        {
            ikWeightTween?.Kill();

            ikWeightTween = DOTween.To(
                () => ikWeight,
                val => ikWeight = val,
                endValue,
                seconds
            );

            ikWeightTween.SetTarget(this);

            return ikWeightTween;
        }
    }
}