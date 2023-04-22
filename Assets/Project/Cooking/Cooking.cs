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
        public Slider slider;

        #endregion

        public void StartCooking()
        {
            slider.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            slider.DOValue(1f, cookingTime).SetEase(Ease.Linear);
        }
    }
}