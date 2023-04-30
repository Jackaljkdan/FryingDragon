using DG.Tweening;
using JK.Injection;
using JK.Interaction;
using JK.Observables;
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

        public float secondsUntilSleep = 5f;

        public Transform bowlAnchor;

        public GameObject item;
        public GameObject box;
        public GameObject coverParticles;
        public ParticleSystem readyParticles;
        public Flammable flammable;

        public UnityEvent onBowlRemoved;

        public FarmerAnimator farmerAnimator;

        public Slider slider;

        [RuntimeField]
        public ObservableProperty<bool> isSleeping = new ObservableProperty<bool>();

        [Injected]
        public DragonInput dragonInput;

        [Injected]
        public DragonItemHolder dragonItemHolder;

        [Injected]
        public OrderFulfiller orderFulfiller;

        [Injected]
        public DragonStress dragonStress;

        [Injected]
        public DragonFireAnimation dragonFireAnimation;

        [Injected]
        private SignalBus signalBus;


        [Injected]
        public LevelSettings levelSettings;

        [ContextMenu("Fall Asleep")]
        private void FallAsleepInEditMode()
        {
            if (Application.isPlaying)
                FallAsleep();
        }

        #endregion

        private Tween tween;

        private bool isPacking = false;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonInput = context.Get<DragonInput>(this);
            dragonItemHolder = context.Get<DragonItemHolder>(this);
            signalBus = context.Get<SignalBus>(this);
            orderFulfiller = context.Get<OrderFulfiller>(this);
            dragonStress = context.Get<DragonStress>(this);
            dragonFireAnimation = context.Get<DragonFireAnimation>(this);
            levelSettings = context.Get<LevelSettings>(this);
        }

        private void Awake()
        {
            Inject();
            slider.transform.localScale = Vector3.zero;

            packingSeconds = levelSettings.packingSeconds;
            secondsUntilSleep = levelSettings.secondsUntilSleep;
        }

        protected override void Start()
        {
            base.Start();

            isSleeping.SetSilently(false);
            ScheduleFallAsleep();

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

        public void FallAsleep()
        {
            isSleeping.Value = true;
            CancelInvoke(nameof(FallAsleep));

            farmerAnimator.PlaySleepLoop();

            tween?.Pause();
        }

        public void ScheduleFallAsleep()
        {
            Invoke(nameof(FallAsleep), secondsUntilSleep);
        }

        private void OnDragonStartingFrenzy()
        {
            isSleeping.Value = false;
            CancelInvoke(nameof(FallAsleep));

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

            ScheduleFallAsleep();
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
            if (dragonItemHolder.heldItem.Value != null || !isSleeping.Value)
                InteractWorkbench();
            else
                InteractAlseep();
        }

        public void InteractWorkbench()
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

            if (item.TryGetComponent<Bowl>(out Bowl bowl))
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

                if (!isSleeping.Value)
                    StartPacking();
            });
        }

        public void InteractAlseep()
        {
            dragonInput.enabled = false;

            Vector3 rotationEuler = Quaternion.LookRotation((farmerAnimator.transform.position - dragonFireAnimation.transform.position).normalized).eulerAngles;
            dragonFireAnimation.transform.DORotate(rotationEuler, 0.2f).OnComplete(() =>
            {
                dragonFireAnimation.PlayFireAnimation(onBreathFireEnd: () =>
                {
                    dragonInput.enabled = true;
                    isSleeping.Value = false;
                    ScheduleFallAsleep();

                    if (tween.IsActive())
                    {
                        tween.Play();
                        return;
                    }

                    if (item != null && item.TryGetComponent(out Bowl bowl))
                        StartPacking();
                    else
                        farmerAnimator.PlayIdle();
                });
            });
        }

        private void StartPacking()
        {
            signalBus.Invoke(new ItemAddedSignal());
            isPacking = true;

            slider.value = 0;
            slider.transform.DOScale(Vector3.one, 0.5f);

            coverParticles.SetActive(true);
            float packingSecondsWithIngredients = packingSeconds + (item.TryGetComponent(out Bowl bowl) ? bowl.ingredients.Count * 2 : 0);

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
            if (item != null && item.TryGetComponent(out Bowl bowl))
            {
                bowl.RemoveAllIngedients();
                bowl.TryAddBox(box);
                readyParticles.gameObject.SetActive(true);
                readyParticles.Play();
            }
        }

        private void RetrieveBowl()
        {
            Bowl bowl = item.GetComponentSafely<Bowl>();

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
                }
            );
        }
    }
}