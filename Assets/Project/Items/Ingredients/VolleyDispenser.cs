using JK.Interaction;
using Project.Jam;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items.Ingredients
{
    [DisallowMultipleComponent]
    public class VolleyDispenser : AbstractInteractable, IDispenser
    {
        #region Inspector

        public float force = 2;

        public Rigidbody prefab;

        public Transform spawnAnchor;
        public Transform volleyDirection;

        public IngredientTypeValue ingredientType => prefab.GetComponent<Ingredient>().ingredientTypeValue;

        private void Reset()
        {
            spawnAnchor = transform;
            volleyDirection = transform;
        }

        #endregion

        protected override void InteractProtected(RaycastHit hit)
        {
            var instance = Instantiate(prefab, spawnAnchor.position, Quaternion.identity, transform.parent);
            instance.AddForce(volleyDirection.forward * force, ForceMode.Impulse);
        }
    }
}