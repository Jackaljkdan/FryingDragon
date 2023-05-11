using DG.Tweening;
using JK.Actuators;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterEntrance : MonoBehaviour
    {
        #region Inspector

        public IkAnchors ikAnchors;

        public IkApplier ikApplier;

        public Animator animator;

        public FirefighterInput input;
        public FirefighterMovement movement;

        [RuntimeField]
        public float ikWeight;

        private void Reset()
        {
            animator = GetComponent<Animator>();
            input = GetComponent<FirefighterInput>();
        }

        #endregion

        private void Start()
        {
            input.enabled = false;
            movement.enabled = false;

            ikWeight = 1;

            transform.DOMoveY(0, 1f).SetEase(Ease.InSine).OnComplete(() =>
            {
                animator.CrossFade("Land", 0.1f);
                DOIkWeight(0, 0.75f);
                Invoke(nameof(GivePlayerControl), 1);
            });
        }

        private void OnAnimatorMove()
        {
            transform.rotation *= animator.deltaRotation;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            ikAnchors.SetIKWithWeight(animator, ikWeight);
        }

        private Tween DOIkWeight(float endValue, float seconds)
        {
            var tween = DOTween.To(
                () => ikWeight,
                val => ikWeight = val,
                endValue,
                seconds
            );

            tween.SetTarget(this);

            return tween;
        }

        private void GivePlayerControl()
        {
            input.enabled = true;
            movement.enabled = true;
            ikApplier.enabled = true;
            ikApplier.ikWeight = 0;
            ikApplier.DOIkWeight(1, 0.5f);
            enabled = false;
        }
    }
}