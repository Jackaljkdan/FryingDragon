using DG.Tweening;
using JK.Injection;
using JK.Interaction;
using JK.Utils;
using Project.Cooking.Recipes;
using Project.Dragon;
using Project.Items;
using Project.Items.Ingredients;
using Project.Jam.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class Packager : AbstractInteractable
    {
        #region Inspector

        public float packingSeconds = 10f;

        public Transform bowlAnchor;

        public GameObject item;
        public GameObject box;
        public GameObject coverParticles;
        public ParticleSystem readyParticles;

        public UnityEvent onBowlRemoved;

        public FarmerAnimator farmerAnimator;

        public Slider slider;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        [Injected]
        public OrderFulfiller orderFulfiller;

        [Injected]
        private SignalBus signalBus;

        #endregion
        private Bowl bowl;

        private bool isPacking = false;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonItemHolder = context.Get<DragonItemHolder>(this);
            signalBus = context.Get<SignalBus>(this);
            orderFulfiller = context.Get<OrderFulfiller>(this);
        }

        private void Awake()
        {
            Inject();
            slider.transform.localScale = Vector3.zero;
        }

        private bool CanDepositBowl(Bowl bowl)
        {

            List<IngredientTypeValue> bowlIngredients = new();
            foreach (Ingredient ingredient in bowl.ingredients)
            {
                bowlIngredients.Add(ingredient.ingredientTypeValue);
            }

            if (bowlIngredients.Count == 0)
                return false;

            foreach (Recipe recipe in orderFulfiller.recipes)
            {
                bool areEqual = recipe.ingredients.OrderBy(x => x).SequenceEqual(bowlIngredients.OrderBy(x => x));

                if (areEqual)
                {
                    signalBus.Invoke(new OrderFulfilledSignal() { recipe = recipe, ingredients = recipe.ingredients });
                    return true;
                }
            }
            return false;
        }

        protected override void InteractProtected(RaycastHit hit)
        {
            if (isPacking)
                return;

            if (dragonItemHolder.holdedItem != null && item != null)
                return;

            if (item != null && dragonItemHolder.holdedItem == null)
            {
                RetrieveBowl();
                return;
            }

            item = dragonItemHolder.holdedItem;

            if (!item)
                return;

            Transform heldTransform = item.transform;

            if (item.TryGetComponent<Bowl>(out bowl))
            {
                if (!CanDepositBowl(bowl))
                {
                    item = null;
                    return;
                }

                bowl.GlueIngredients();
            }

            dragonItemHolder.AnimatePutItem(onPutItemRelease: () =>
            {
                if (bowl)
                    bowl.enabled = false;

                signalBus.Invoke(new ItemRemovedSignal());
                heldTransform.SetParent(dragonItemHolder.transform.parent, worldPositionStays: true);
                heldTransform.DOMove(bowlAnchor.position, 0.2f);
                heldTransform.DORotate(bowlAnchor.eulerAngles, 0.2f);
                dragonItemHolder.holdedItem = null;
                StartPacking();
            });
        }

        private void StartPacking()
        {
            signalBus.Invoke(new ItemAddedSignal());
            isPacking = true;
            slider.value = 0;
            slider.transform.DOScale(Vector3.one, 0.5f);
            farmerAnimator.PlayPack(packingSeconds);
            coverParticles.SetActive(true);
            slider.DOValue(1f, packingSeconds).OnComplete(() =>
            {
                InstantiateBox();
                slider.transform.DOScale(Vector3.zero, 0.5f);
                coverParticles.SetActive(false);
                farmerAnimator.PlayIdle();
                isPacking = false;
            }).SetEase(Ease.Linear);
        }

        private void InstantiateBox()
        {
            if (bowl)
            {
                bowl.RemoveAllIngedients();
                bowl.TryAddBox(box);
                readyParticles.gameObject.SetActive(true);
                readyParticles.Play();
            }
        }

        private void RetrieveBowl()
        {
            dragonItemHolder.AnimateRetriveItem(
            onRetrieveItemRelease: () =>
            {
                if (bowl)
                    bowl.enabled = true;

                onBowlRemoved.Invoke();
                item.transform.SetParent(transform.root, worldPositionStays: true);
                item.transform.position = bowlAnchor.position;
                item.transform.rotation = bowlAnchor.rotation;
                dragonItemHolder.holdedItem = item.gameObject;
                signalBus.Invoke(new ItemAddedSignal());
            },
            onRetrieveEnd: () =>
            {
                if (bowl)
                    bowl.UnGlueIngredients();

                item = null;
            });
        }
    }
}