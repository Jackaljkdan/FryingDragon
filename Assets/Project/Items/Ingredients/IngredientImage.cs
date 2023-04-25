using System;
using System.Collections;
using System.Collections.Generic;
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

        public List<Texture2D> ingredientsImages = new();

        #endregion

        private IngredientTypeValue currentIngredient;

        public void SetImage(IngredientTypeValue ingredient)
        {
            image.texture = ingredientsImages[(int)ingredient];
            currentIngredient = ingredient;
        }

        public bool ShowChecked(IngredientTypeValue ingredient)
        {
            if (ingredient != currentIngredient || checkedImage.activeSelf)
                return false;

            checkedImage.SetActive(true);
            return true;

        }
        public bool HideChecked(IngredientTypeValue ingredient)
        {

            if (ingredient != currentIngredient || !checkedImage.activeSelf)
                return false;

            checkedImage.SetActive(false);
            return true;

        }

    }
}