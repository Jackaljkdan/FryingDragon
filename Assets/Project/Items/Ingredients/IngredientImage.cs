using DG.Tweening;
using JK.Injection;
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
        public List<IngredientInfo> ingredientInfos = new();

        [Injected]
        private SignalBus signalBus;

        #endregion

        private bool active = false;

        private Tween tween;

        private IngredientTypeValue currentIngredient;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);

        }


        private void Awake()
        {
            checkedImage.transform.localScale = Vector3.zero;
            Inject();
        }

        private void Start()
        {
            signalBus.AddListener<ItemRemovedSignal>(OnItemRemoved);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<ItemRemovedSignal>(OnItemRemoved);
        }

        public void SetImage(IngredientTypeValue ingredient)
        {
            foreach (var info in ingredientInfos)
            {
                if (info.type == ingredient)
                {
                    image.texture = info.image.texture;
                    currentIngredient = ingredient;
                    return;
                }
            }
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

        private void OnItemRemoved(ItemRemovedSignal arg)
        {
            HideChecked();
        }

    }
}