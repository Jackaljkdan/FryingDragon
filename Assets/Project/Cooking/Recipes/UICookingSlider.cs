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

        private void Awake()
        {
            GetComponent<RectTransform>().localScale = Vector3.zero;
        }

        public Tween DOFill(float fillTime)
        {
            SetFillingColors();
            return slider.DOValue(1f, fillTime).SetEase(Ease.Linear);
        }

        public Tween DOFillWithScale(float fillTime)
        {
            SetFillingColors();
            slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            return slider.DOValue(1f, fillTime).SetEase(Ease.Linear);
        }

        public Tween DOOverfill(float fillTime)
        {
            SetOverfillingColors();
            return slider.DOValue(1f, fillTime).SetEase(Ease.Linear);
        }

        public Tween DOScaledown()
        {
            return slider.transform.DOScale(0, 1f).SetEase(Ease.InBack);
        }

        public Tween DoScaleup()
        {
            return slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
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