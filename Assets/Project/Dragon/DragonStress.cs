using JK.Injection;
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

        public float stress;

        public float ingredientLostStress = 0.1f;

        public float fireStressRelief = 0.25f;

        public Animator dragonAnimator;

        public DragonInput dragonInput;

        public DragonMovement dragonMovement;

        public DragonFireAnimation dragonFireAnimation;

        public DragonInteractore dragonInteractore;

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
        public Slider slider;

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
            dragonInput = GetComponent<DragonInput>();
            dragonMovement = GetComponent<DragonMovement>();
            dragonFireAnimation = GetComponent<DragonFireAnimation>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            slider = context.GetOptional<Slider>("dragon.stress");
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
            signalBus.AddListener<IngredientLostSignal>(OnIngredientLost);
        }

        private void OnDisable()
        {
            signalBus.RemoveListener<IngredientLostSignal>(OnIngredientLost);
        }

        private void OnIngredientLost(IngredientLostSignal signal)
        {
            IncrementStress(ingredientLostStress);
        }

        private void IncrementStress(float delta)
        {
            stress = Mathf.Clamp01(stress + delta);

            if (stress >= 1 && !isInFrenzy)
                StartFrenzy();
        }

        public void StartFrenzy()
        {
            stress = 1;
            isInFrenzy = true;
            dragonInput.enabled = false;
            dragonInteractore.enabled = false;
            onFrenzy.Invoke();
        }

        public void StopFrenzy()
        {
            stress = 0;
            isInFrenzy = false;
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

        private void Update()
        {
            slider.value = stress;

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
                chosenFlammable = RandomUtils.Choose(flammableList.list);

            Transform dragonTransform = dragonMovement.transform;

            Vector3 vector = (chosenFlammable.transform.position - dragonTransform.position).WithY(0);
            distanceToChosen = vector.magnitude;
            bool isTooFar = vector.magnitude > 4f;

            Vector3 direction = vector.normalized;
            float dot = Vector3.Dot(dragonTransform.forward, direction);
            bool isFacing = dot >= 0.999f;

            Vector2 movementInput = new Vector2(0, 0);

            if (!isFacing)
            {
                float rightDot = Vector3.Dot(dragonTransform.right, direction);
                float rightDotSign = Mathf.Sign(rightDot);
                movementInput.x = Mathf.Lerp(1 * rightDotSign, 0.6f * rightDotSign, dot);
            }

            if (isTooFar || !isFacing)
            {
                if (isTooFar && dot >= 0.1f)
                    movementInput.y = 1;
            }
            else if (!isFiring)
            {
                dragonFireAnimation.PlayFireAnimation(onBreathFireEnd: () => Invoke(nameof(StopFiring), 2));
                chosenFlammable.StartFire();

                isFiring = true;
                chosenFlammable = null;

                stress = Mathf.Clamp01(stress - fireStressRelief);
                if (stress <= 0)
                    StopFrenzy();
            }

            dragonMovement.Move(movementInput);
        }

        private void StopFiring()
        {
            isFiring = false;
        }
    }
}