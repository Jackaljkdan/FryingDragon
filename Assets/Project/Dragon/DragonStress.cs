using JK.Injection;
using JK.Observables;
using JK.Utils;
using Project.Flames;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

        public NavMeshAgent agent;

        public DragonInput dragonInput;

        public DragonFireAnimation dragonFireAnimation;

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
        public float remainingDistance;

        [DebugField]
        public float stoppingDistance;

        [DebugField]
        public float dot;

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
            dragonFireAnimation = GetComponent<DragonFireAnimation>();
            dragonItemHolder = GetComponent<DragonItemHolder>();
        }

        #endregion

        private int xHash;
        private int zHash;

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
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

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
            dragonInput.movement.enabled = false;
            agent.enabled = true;
            dragonItemHolder.DropItem();
            onFrenzy.Invoke();
        }

        public void StopFrenzy()
        {
            stress.Value = 0;
            isInFrenzy = false;
            isFiring = false;
            isEmbarassed = true;
            agent.enabled = false;
            dragonAnimator.CrossFade("Confused", 0.2f);
        }

        public void StopEmarassment()
        {
            dragonAnimator.CrossFade("Move", 0.2f);
            isEmbarassed = false;
            dragonInput.movement.enabled = true;
            dragonInput.enabled = true;
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

            float fireDistance = 4;

            if (chosenFlammable == null)
            {
                chosenFlammable = ChooseRandomFlammable();
                agent.stoppingDistance = fireDistance;
                stoppingDistance = agent.stoppingDistance;
            }

            agent.SetDestination(chosenFlammable.transform.position);

            remainingDistance = agent.remainingDistance;
            bool isTooFar = agent.remainingDistance > agent.stoppingDistance;

            Transform dragonTransform = agent.transform;
            Vector3 vector = (chosenFlammable.transform.position - dragonTransform.position).WithY(0);
            Vector3 direction = vector.normalized;
            dot = Vector3.Dot(dragonTransform.forward, direction);
            bool isFacing = dot >= 0.99f;

            Vector3 dragonRelativeVector = Vector3.ClampMagnitude(dragonTransform.InverseTransformDirection(vector), 2);

            dragonAnimator.SetFloat(xHash, dragonRelativeVector.x);
            dragonAnimator.SetFloat(zHash, dragonRelativeVector.z);

            if (isTooFar)
                return;

            if (!isFacing)
            {
                dragonTransform.rotation = Quaternion.Lerp(dragonTransform.rotation, Quaternion.LookRotation(direction), 0.1f);
                return;
            }

            dragonAnimator.SetFloat(xHash, 0);
            dragonAnimator.SetFloat(zHash, 0);

            dragonFireAnimation.PlayFireAnimation(onBreathFireEnd: () => Invoke(nameof(StopFiring), 2));
            chosenFlammable.StartFire();

            isFiring = true;
            chosenFlammable = null;

            stress.Value = Mathf.Clamp01(stress.Value - fireStressRelief);
            if (stress.Value <= 0)
                StopFrenzy();
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