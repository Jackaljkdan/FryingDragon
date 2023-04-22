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

        public float cookingTime = 1f;
        public Slider cookingSlider;

        #endregion

        public void StartCooking()
        {
            cookingSlider.value = 0;
            cookingSlider.transform.DOScale(1, 1).SetEase(Ease.OutBounce);
            cookingSlider.DOValue(1, cookingTime);
        }

        public void StopCooking()
        {
            cookingSlider.transform.DOScale(0, 1).SetEase(Ease.InBounce);
        }
    }
}