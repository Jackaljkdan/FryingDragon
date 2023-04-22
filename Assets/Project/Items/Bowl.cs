using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items
{
    [DisallowMultipleComponent]
    [LateExecutionOrder]
    public class Bowl : MonoBehaviour
    {
        #region Inspector

        public Transform spawnAnchor;
        public Rigidbody rb;

        [RuntimeField]
        public List<GameObject> ingredients = new();

        [Injected]
        public Transform anchorTransform;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            anchorTransform = context.Get<Transform>(this, "dragon.anchor");
        }

        private void Awake()
        {
            Inject();
        }

        private void FixedUpdate()
        {
            rb.MovePosition(anchorTransform.position);
            rb.MoveRotation(anchorTransform.rotation);
        }

        public void TryAddIngredient(GameObject ingredient)
        {
            GameObject spawned = Instantiate(ingredient, spawnAnchor.position, UnityEngine.Random.rotation, transform.root);
            ingredients.Add(spawned);
        }

        public void RemoveIngredient(GameObject ingredient)
        {

            ingredients.Remove(ingredient);
        }

        public void GlueIngredients()
        {
            foreach (GameObject ingredient in ingredients)
            {
                ingredient.transform.SetParent(transform);
                Rigidbody rb = ingredient.GetComponent<Rigidbody>();

                if (!rb)
                    continue;

                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }

        public void UnGlueIngredients()
        {
            foreach (GameObject ingredient in ingredients)
            {
                ingredient.transform.SetParent(transform.root);
                Rigidbody rb = ingredient.GetComponent<Rigidbody>();

                if (!rb)
                    continue;

                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
    }
}