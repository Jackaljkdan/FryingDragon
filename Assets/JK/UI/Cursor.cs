using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class Cursor : MonoBehaviour
    {
        #region Inspector

        public Image image;

        public float showSeconds = 0.2f;
        public float hideSeconds = 0.2f;

        [RuntimeField]
        public Color defaultColor;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        #endregion

        private Tween tween;

        private void Start()
        {
            defaultColor = image.color;
            image.color = image.color.WithAlpha(0);
        }

        public void Show()
        {
            Show(defaultColor);
        }

        public void Show(Color color)
        {
            tween?.Kill();
            tween = image.DOColor(color, showSeconds);
        }

        public void Hide()
        {
            tween?.Kill();
            tween = image.DOFade(0, hideSeconds);
        }
    }
}