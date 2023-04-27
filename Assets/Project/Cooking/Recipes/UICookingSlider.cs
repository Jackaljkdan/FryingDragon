using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class UICookingSlider : MonoBehaviour
    {
        #region Inspector

        public Slider slider;
        public Image sliderBackgroundArea;
        public Image sliderFillArea;

        public Color backgroundColor = new Color(63, 63, 63);
        public Color fillingColor = Color.white;
        public Color overFillingColor = Color.red;

        private void Reset()
        {
            slider = GetComponent<Slider>();
        }

        #endregion

        private Tween tween;

        private void Awake()
        {
            GetComponent<RectTransform>().localScale = Vector3.zero;
        }

        public Tween DOFill(float fillTime)
        {
            SetFillingColors();
            tween = slider.DOValue(1f, fillTime).SetEase(Ease.Linear);
            return tween;
        }

        public Tween DOFillWithScale(float fillTime)
        {
            SetFillingColors();
            slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            tween = slider.DOValue(1f, fillTime).SetEase(Ease.Linear);
            return tween;
        }

        public Tween DOOverfill(float fillTime)
        {
            SetOverfillingColors();
            tween = slider.DOValue(1f, fillTime).SetEase(Ease.Linear);
            return tween;
        }

        public Tween DOScaledown()
        {
            tween?.Kill(false);
            tween = slider.transform.DOScale(0, 1f).SetEase(Ease.InBack);
            return tween;
        }

        public Tween DoScaleup()
        {
            tween = slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            return tween;
        }

        private void SetFillingColors()
        {
            slider.value = 0;
            sliderBackgroundArea.color = backgroundColor;
            sliderFillArea.color = fillingColor;
        }

        private void SetOverfillingColors()
        {
            slider.value = 0;
            sliderBackgroundArea.color = fillingColor;
            sliderFillArea.color = overFillingColor;
        }
    }
}