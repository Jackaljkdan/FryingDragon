using JK.Injection;
using JK.Observables;
using JK.Utils;
using Project.Character;
using Project.Flames;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class DragonStress : MonoBehaviour
    {
        #region Inspector

        public ObservableProperty<float> stress = new ObservableProperty<float>();

        public float ingredientLostStress = 0.1f;

        public float fireStressRelief = 0.25f;

        public Animator dragonAnimator;

        public DragonInput dragonInput;

        public DragonMovement dragonMovement;

        public DragonFireAnimation dragonFireAnimation;

        public DragonInteractore dragonInteractore;

        public DragonItemHolder dragonItemHolder;

        public UnityEvent onFrenzy = new UnityEvent();

        [RuntimeField]
        public bool isInFrenzy;

        [RuntimeField]
        public bool isEmbarassed;

        [RuntimeField]
        public bool isFiring;

        [RuntimeField]
        public Flammable chosenFlammable;

        [DebugField]
        public float distanceToChosen;

        [Injected]
        public FlammableList flammableList;

        [Injected]
        private SignalBus signalBus;

        [ContextMenu("Start frenzy")]
        private void StartFrenzyInEditMode()
        {
            if (Application.isPlaying)
                StartFrenzy();
        }

        [ContextMenu("Stop frenzy")]
        private void StopFrenzyInEditMode()
        {
            if (Application.isPlaying)
                StopFrenzy();
        }

        private void Reset()
        {
            dragonAnimator = GetComponent<Animator>();
            dragonInput = GetComponent<DragonInput>();
            dragonMovement = GetComponent<DragonMovement>();
            dragonFireAnimation = GetComponent<DragonFireAnimation>();
            dragonInteractore = GetComponentInChildren<DragonInteractore>();
            dragonItemHolder = GetComponent<DragonItemHolder>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            flammableList = context.Get<FlammableList>(this);
            signalBus = context.Get<SignalBus>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            isInFrenzy = false;
            isEmbarassed = false;
        }

        private void OnEnable()
        {
            signalBus.AddListener<IngredientFallenSignal>(OnIngredientFallen);
        }

        private void OnDisable()
        {
            signalBus.RemoveListener<IngredientFallenSignal>(OnIngredientFallen);
        }

        private void OnIngredientFallen(IngredientFallenSignal signal)
        {
            IncrementStress(ingredientLostStress);
        }

        private void IncrementStress(float delta)
        {
            stress.Value = Mathf.Clamp01(stress.Value + delta);

            if (stress.Value >= 1 && !isInFrenzy)
                StartFrenzy();
        }

        public void StartFrenzy()
        {
            stress.Value = 1;
            isInFrenzy = true;
            dragonInput.enabled = false;
            dragonInteractore.enabled = false;
            dragonItemHolder.DropItem();
            onFrenzy.Invoke();
        }

        public void StopFrenzy()
        {
            stress.Value = 0;
            isInFrenzy = false;
            isFiring = false;
            isEmbarassed = true;
            dragonMovement.Move(Vector2.zero);
            dragonAnimator.CrossFade("Confused", 0.2f);
        }

        public void StopEmarassment()
        {
            dragonAnimator.CrossFade("Move", 0.2f);
            isEmbarassed = false;
            dragonInput.enabled = true;
            dragonInteractore.enabled = true;
        }

        private Flammable ChooseRandomFlammable()
        {
            ListUtils.ShuffleInPlace(flammableList.list);

            foreach (var flammable in flammableList.list)
                if (flammable != null && flammable.isActiveAndEnabled)
                    return flammable;

            return null;
        }

        private void Update()
        {
            if (isEmbarassed && flammableList.fires.Value == 0)
            {
                StopEmarassment();
                return;
            }

            if (!isInFrenzy)
                return;

            if (isFiring)
                return;

            if (chosenFlammable == null)
                chosenFlammable = ChooseRandomFlammable();

            Transform dragonTransform = dragonMovement.transform;

            Vector3 vector = (chosenFlammable.transform.position - dragonTransform.position).WithY(0);
            distanceToChosen = vector.magnitude;
            bool isTooFar = vector.magnitude > 4f;

            Vector3 direction = vector.normalized;
            float dot = Vector3.Dot(dragonTransform.forward, direction);
            bool isFacing = dot >= 0.999f;

            Vector3 fwd = dragonMovement.transform.forward;
            Vector2 fwd2 = new Vector2(fwd.x, fwd.z);

            Vector2 movementInput = new Vector2(0, 0);
            Vector2 rotationInput = Vector2.Lerp(fwd2, new Vector2(direction.x, direction.z), 0.1f);

            if (isTooFar || !isFacing)
            {
                if (isTooFar && dot >= 0.1f)
                    movementInput.y = 1;

                dragonMovement.Move(movementInput, rotationInput);
            }
            else if (!isFiring)
            {
                dragonMovement.Move(Vector2.zero);

                dragonFireAnimation.PlayFireAnimation(onBreathFireEnd: () => Invoke(nameof(StopFiring), 2));
                chosenFlammable.StartFire();

                isFiring = true;
                chosenFlammable = null;

                stress.Value = Mathf.Clamp01(stress.Value - fireStressRelief);
                if (stress.Value <= 0)
                    StopFrenzy();
            }
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Backspace) && !isInFrenzy)
                StartFrenzy();
        }

        private void StopFiring()
        {
            isFiring = false;
        }
    }
}