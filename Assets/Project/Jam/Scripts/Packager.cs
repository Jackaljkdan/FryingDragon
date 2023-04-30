using DG.Tweening;
using JK.Injection;
using JK.Interaction;
using JK.Utils;
using Project.Cooking.Recipes;
using Project.Dragon;
using Project.Flames;
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
        public Flammable flammable;

        public UnityEvent onBowlRemoved;

        public FarmerAnimator farmerAnimator;

        public Slider slider;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        [Injected]
        public OrderFulfiller orderFulfiller;

        [Injected]
        public DragonStress dragonStress;

        [Injected]
        private SignalBus signalBus;

        #endregion
        private Tween tween;

        private Bowl bowl;

        private bool isPacking = false;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonItemHolder = context.Get<DragonItemHolder>(this);
            signalBus = context.Get<SignalBus>(this);
            orderFulfiller = context.Get<OrderFulfiller>(this);
            dragonStress = context.Get<DragonStress>(this);
        }

        private void Awake()
        {
            Inject();
            slider.transform.localScale = Vector3.zero;
        }

        protected override void Start()
        {
            base.Start();

            signalBus.AddListener<FirefighterExitSignal>(OnFirefighterExit);
            signalBus.AddListener<FireStartSignal>(OnFireStart);
            dragonStress.onFrenzy.AddListener(OnDragonStartingFrenzy);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<FirefighterExitSignal>(OnFirefighterExit);
            signalBus.RemoveListener<FireStartSignal>(OnFireStart);
            dragonStress.onFrenzy.RemoveListener(OnDragonStartingFrenzy);
        }

        private void OnDragonStartingFrenzy()
        {
            farmerAnimator.PlayHorrorLoop();
            coverParticles.transform.DOScale(Vector3Utils.Create(0.7f), 0.5f);
            tween.Pause();
        }

        private void OnFirefighterExit(FirefighterExitSignal _)
        {
            coverParticles.transform.DOScale(Vector3.one, 0.5f);

            if (tween.IsActive())
                tween.Play();
            else
                farmerAnimator.PlayIdle();
        }

        private void OnFireStart(FireStartSignal signal)
        {
            if (signal.flammable != flammable)
                return;

            if (!item)
                return;

            tween?.Kill();
            Destroy(item);
            item = null;
            isPacking = false;
            coverParticles.GetComponent<ParticleSystem>().Stop();
            slider.transform.DOScale(Vector3.zero, 0.5f);
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

            if (dragonItemHolder.heldItem.Value != null && item != null)
                return;

            if (item != null && dragonItemHolder.heldItem.Value == null)
            {
                RetrieveBowl();
                return;
            }

            item = dragonItemHolder.heldItem.Value;

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
                dragonItemHolder.heldItem.Value = null;
                StartPacking();
            });
        }

        private void StartPacking()
        {
            signalBus.Invoke(new ItemAddedSignal());
            isPacking = true;

            slider.value = 0;
            slider.transform.DOScale(Vector3.one, 0.5f);

            coverParticles.SetActive(true);

            tween?.Kill();
            tween = slider.DOValue(1f, packingSeconds).SetEase(Ease.Linear);
            tween.OnPlay(farmerAnimator.PlayPack);
            tween.OnComplete(() =>
            {
                InstantiateBox();
                slider.transform.DOScale(Vector3.zero, 0.5f);
                coverParticles.SetActive(false);
                farmerAnimator.PlayIdle();
                isPacking = false;
            });
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
                dragonItemHolder.heldItem.Value = item.gameObject;
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