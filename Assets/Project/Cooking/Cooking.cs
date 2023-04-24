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
        public ParticleSystem readyParticles;

        public Slider slider;
        public Image sliderBackgroundArea;
        public Image sliderFillArea;

        public Color backgroundColor = new Color(63, 63, 63);
        public Color cookingColor = Color.white;
        public Color overcookingColor = Color.red;

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
            readyParticles.gameObject.SetActive(false);
            SetCookingColors();
            particles.gameObject.SetActive(true);
            slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            cookingTween = slider.DOValue(1f, cookingTime).SetEase(Ease.Linear);

            cookingTween.OnComplete(() =>
            {
                readyParticles.gameObject.SetActive(true);
                readyParticles.Play();
                SetOvercookingColors();
                slider.DOValue(1f, cookingTime).SetEase(Ease.Linear);
            });
        }

        public void StartOvercooking()
        {

        }

        public void StopCooking()
        {
            particles.Stop();
        }

        private void SetCookingColors()
        {
            slider.value = 0;
            sliderBackgroundArea.color = backgroundColor;
            sliderFillArea.color = cookingColor;
        }

        private void SetOvercookingColors()
        {
            slider.value = 0;
            sliderBackgroundArea.color = cookingColor;
            sliderFillArea.color = overcookingColor;
        }
    }
}