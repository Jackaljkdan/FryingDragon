using JK.Injection;
using JK.Utils;
using Project.Cooking;
using Project.Cooking.Recipes;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public GameObject burnedPrefab;

        public Transform spawnAnchor;
        public Transform boxSpawnAnchor;
        public Transform recipeSpawnAnchor;
        public Rigidbody rb;

        public Transform dropForceAnchor;

        [RuntimeField]
        public List<Ingredient> ingredients = new();

        [Injected]
        public OrderFulfiller fulfiller;

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
            fulfiller = context.Get<OrderFulfiller>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            signalBus.AddListener<CookingFinishedSignal>(OnCookingFinished);
            signalBus.AddListener<CookingBurnedSignal>(OnCookingBurned);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<CookingFinishedSignal>(OnCookingFinished);
            signalBus.RemoveListener<CookingBurnedSignal>(OnCookingBurned);
        }

        private void FixedUpdate()
        {
            rb.MovePosition(anchorTransform.position);
            rb.MoveRotation(anchorTransform.rotation);
        }


        public void TryAddIngredient(GameObject prefab, Transform spawnPosition)
        {
            GameObject spawned = Instantiate(prefab, spawnPosition.position, UnityEngine.Random.rotation, transform.root);
            AddIngredient(spawned.GetComponent<Ingredient>());
        }

        public void TryAddIngredient(GameObject prefab)
        {
            TryAddIngredient(prefab, spawnAnchor);
        }

        public void TryAddBox(GameObject prefab)
        {
            GameObject spawned = Instantiate(prefab, boxSpawnAnchor.position, Quaternion.identity, transform);
            AddIngredient(spawned.GetComponent<Ingredient>());
        }

        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredients.Contains(ingredient))
                return;

            ingredients.Add(ingredient);
            signalBus.Invoke(new IngredientTakenSignal() { ingredient = ingredient });
        }

        public void RemoveIngredient(Ingredient ingredient)
        {

            ingredients.Remove(ingredient);
            signalBus.Invoke(new IngredientLostSignal() { ingredient = ingredient.GetComponent<Ingredient>(), availableIngredients = ingredients });
        }

        public void RemoveAllIngedients()
        {
            for (int i = ingredients.Count - 1; i >= 0; i--)
            {
                Ingredient ingredient = ingredients[i];
                ingredients.RemoveAt(i);
                Destroy(ingredient.gameObject);
            }
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

        private void OnCookingFinished(CookingFinishedSignal signal)
        {
            if (signal.bowl != this)
                return;

            if (IsCookingARecipe(out Recipe recipe))
            {
                RemoveAllIngedients();
                TryAddIngredient(recipe.recipePrefab, recipeSpawnAnchor);
                GlueIngredients();
            }

        }

        private void OnCookingBurned(CookingBurnedSignal signal)
        {
            if (signal.bowl != this)
                return;

            RemoveAllIngedients();
            TryAddIngredient(burnedPrefab, recipeSpawnAnchor);
            GlueIngredients();

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

        private bool IsCookingARecipe(out Recipe recipe)
        {
            recipe = null;
            if (ingredients == null)
                return false;

            List<IngredientTypeValue> ingredientsTypes = ingredients.Select(el => el.ingredientTypeValue).ToList();

            foreach (Recipe rec in fulfiller.recipes)
            {
                if (rec.MatchIngredients(ingredientsTypes))
                {
                    recipe = rec;
                    return true;
                }
            }

            return false;
        }
    }
}