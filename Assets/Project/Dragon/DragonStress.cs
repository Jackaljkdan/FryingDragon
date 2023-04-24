using JK.Injection;
using JK.Utils;
using Project.Character;
using Project.Flames;
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

        public float value;

        public DragonInput dragonInput;

        public DragonMovement dragonMovement;

        public DragonFireAnimation dragonFireAnimation;

        public DragonInteractore dragonInteractore;

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

        public bool IsInFrenzy => value >= 1;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            slider = context.GetOptional<Slider>("dragon.stress");
            flammableList = context.Get<FlammableList>(this);
        }

        private void Awake()
        {
            Inject();
        }

        public void StartFrenzy()
        {
            value = 1;
            dragonInput.enabled = false;
            dragonInteractore.enabled = false;
        }

        public void StopFrenzy()
        {
            value = 0;
            dragonInput.enabled = true;
            dragonInteractore.enabled = true;
        }

        private void Update()
        {
            if (!IsInFrenzy)
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
                movementInput.x = Mathf.Lerp(1 * rightDotSign, 0.33f * rightDotSign, dot);
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
            }

            dragonMovement.Move(movementInput);
        }

        private void StopFiring()
        {
            isFiring = false;
        }
    }
}