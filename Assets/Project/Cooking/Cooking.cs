using DG.Tweening;
using JK.Utils.DGTweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Cooking
{
    [DisallowMultipleComponent]
    public class Cooking : MonoBehaviour
    {
        #region Inspector

        public float cookingTime = 10f;

        public ParticleSystem particles;
        public ParticleSystem smokeParticles;

        public Slider slider;

        [ContextMenu("Stop")]
        private void StopInEditMode()
        {
            if (Application.isPlaying)
                particles.Stop();
        }

        [ContextMenu("Start smoke")]
        private void StartSmokeInEditMode()
        {
            if (Application.isPlaying)
            {
                smokeParticles.gameObject.SetActive(true);
                smokeParticles.Play();
            }
        }

        [ContextMenu("Stop smoke")]
        private void StopSmokeInEditMode()
        {
            if (Application.isPlaying)
                smokeParticles.Stop();
        }


        #endregion

        public bool IsCooking => cookingTween.IsActiveAndPlaying();

        private Tween cookingTween;

        public void StartCooking()
        {
            particles.gameObject.SetActive(true);
            slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            cookingTween = slider.DOValue(1f, cookingTime).SetEase(Ease.Linear);
        }

        public void StopCooking()
        {
            particles.Stop();
        }
    }
}