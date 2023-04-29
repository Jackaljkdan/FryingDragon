using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items.Ingredients
{
    [DisallowMultipleComponent]
    public class IngeredientLostTrigger : MonoBehaviour
    {
        #region Inspector

        public Bowl bowl;

        private void Reset()
        {
            bowl = GetComponentInParent<Bowl>();
        }
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponentInParent(out Ingredient ingredient))
                bowl.AddIngredient(ingredient);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponentInParent(out Ingredient ingredient))
                bowl.RemoveIngredient(ingredient);
        }
    }
}