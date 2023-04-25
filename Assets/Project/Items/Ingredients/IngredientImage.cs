using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Items.Ingredients
{
    [DisallowMultipleComponent]
    public class IngredientImage : MonoBehaviour
    {
        #region Inspector

        public RawImage image;
        public GameObject checkedImage;

        public float animationInSeconds = 1f;
        public float animationOutSeconds = .3f;

        public List<Texture2D> ingredientsImages = new();

        #endregion

        private bool active = false;

        private Tween tween;

        private IngredientTypeValue currentIngredient;

        private void Awake()
        {
            checkedImage.transform.localScale = Vector3.zero;
        }

        public void SetImage(IngredientTypeValue ingredient)
        {
            image.texture = ingredientsImages[(int)ingredient];
            currentIngredient = ingredient;
        }

        public bool ShowChecked(IngredientTypeValue ingredient)
        {
            if (ingredient != currentIngredient || active)
                return false;

            active = true;
            tween?.Kill();
            tween = checkedImage.transform.DOScale(Vector3.one, animationInSeconds).SetEase(Ease.OutBounce);
            return true;

        }
        public bool HideChecked(IngredientTypeValue ingredient)
        {

            if (ingredient != currentIngredient || !active)
                return false;

            HideChecked();
            return true;

        }

        public void HideChecked()
        {
            active = false;
            tween?.Kill();
            tween = checkedImage.transform.DOScale(Vector3.zero, animationOutSeconds).SetEase(Ease.Linear);
        }

    }
}