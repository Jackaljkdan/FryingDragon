using DG.Tweening;
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

        public void StartCooking()
        {
            particles.gameObject.SetActive(true);
            slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            slider.DOValue(1f, cookingTime).SetEase(Ease.Linear);
        }

        public void StopCooking()
        {
            particles.Stop();
        }
    }
}