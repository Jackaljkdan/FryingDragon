using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking.Recipes
{
    [DisallowMultipleComponent]
    public class UIBurned : MonoBehaviour
    {
        #region Inspector

        public RectTransform myRectTransform;

        private void Reset()
        {
            myRectTransform = GetComponent<RectTransform>();
        }

        #endregion


        private void Awake()
        {
            myRectTransform.localScale = Vector3.zero;
        }

        public Tween DOShow()
        {
            return myRectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }

        public Tween DOHide()
        {
            return myRectTransform.DOScale(0f, 0.5f).SetEase(Ease.InBack);
        }
    }
}