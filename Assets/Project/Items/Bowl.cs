using JK.Injection;
using JK.Utils;
using Project.Items.Ingredients;
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

        public GameObject ingredientLostTrigger;

        public Transform spawnAnchor;
        public Rigidbody rb;

        public Transform dropForceAnchor;

        [RuntimeField]
        public List<Ingredient> ingredients = new();

        [Injected]
        public Transform anchorTransform;

        [Injected]
        private SignalBus signalBus;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
            ingredientLostTrigger = GetComponentInChildren<IngeredientLostTrigger>().gameObject;
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);
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
            ingredients.Add(spawned.GetComponent<Ingredient>());
            signalBus.Invoke(new IngredientTakenSignal() { ingredient = spawned.GetComponent<Ingredient>() });
        }

        public void RemoveIngredient(Ingredient ingredient)
        {

            ingredients.Remove(ingredient);
            signalBus.Invoke(new IngredientLostSignal() { ingredient = ingredient.GetComponent<Ingredient>(), availableIngredients = ingredients });
        }

        public void GlueIngredients()
        {
            ingredientLostTrigger.SetActive(false);

            foreach (Ingredient ingredient in ingredients)
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
            foreach (Ingredient ingredient in ingredients)
            {
                ingredient.transform.SetParent(transform.root);
                Rigidbody rb = ingredient.GetComponent<Rigidbody>();

                if (!rb)
                    continue;

                rb.useGravity = true;
                rb.isKinematic = false;
            }

            ingredientLostTrigger.SetActive(true);
        }

        public void Drop()
        {
            enabled = false;

            float force = 3 * (ingredients.Count + 1);

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForceAtPosition(dropForceAnchor.forward * force, dropForceAnchor.position, ForceMode.Impulse);

            ingredientLostTrigger.SetActive(false);

            for (int i = ingredients.Count - 1; i >= 0; i--)
                RemoveIngredient(ingredients[i]);
        }
    }
}