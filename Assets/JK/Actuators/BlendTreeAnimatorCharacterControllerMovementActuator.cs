using DG.Tweening;
using JK.Utils;
using JK.Utils.DGTweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class BlendTreeAnimatorCharacterControllerMovementActuator : AbstractMovementActuatorBehaviour
    {
        #region Inspector

        public Animator animator;

        public CharacterController characterController;

        [RuntimeField]
        public float initialSpeed;

        private void Reset()
        {
            DirectionReference = transform;
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        #endregion

        private int xId;
        private int zId;

        private void Awake()
        {
            initialSpeed = Speed;

            xId = Animator.StringToHash("X");
            zId = Animator.StringToHash("Z");
        }

        protected override void MoveProtected(Vector3 direction)
        {
            Vector3 velocity = direction * Speed;

            animator.SetFloat(xId, velocity.x / initialSpeed);
            animator.SetFloat(zId, velocity.z / initialSpeed);

            //characterController.SimpleMove(animator.deltaPosition.WithY(0) * 60);
            characterController.SimpleMove(DirectionReference.TransformDirection(velocity));
        }

        private void OnAnimatorMove()
        {
            // N.B. questo metodo serve perché altrimenti animator.deltaPosition è sempre zero,
            // ma il movimento è gestito nei metodi Moving*
            // questo metodo è chiamato prima di Update quindi siamo perfi
        }

        public Tween DOPosition(Vector3 position, Vector3 lookTarget)
        {
            Transform myTransform = transform;

            // figure out how long to move and rotate
            float referenceDistance = 0.5f;
            float referenceAngle = 40f;
            float referenceSeconds = 0.5f;

            Vector3 delta = position - myTransform.position;
            float distance = delta.magnitude;
            float distanceRatio = distance / referenceDistance;

            Quaternion targetRotation = Quaternion.LookRotation(lookTarget - position);
            float angle = Quaternion.Angle(myTransform.rotation, targetRotation);
            float angleRatio = angle / referenceAngle;

            float seconds = referenceSeconds * Mathf.Max(distanceRatio, angleRatio);

            // actually move and rotate
            Sequence tween = DOTween.Sequence();

            tween.Insert(0, myTransform.DOMove(position, seconds));
            tween.Insert(0, myTransform.DORotate(targetRotation.eulerAngles, seconds));

            // animate feet properly
            float t = 0;
            float animatorX = animator.GetFloat(xId);  // these get changed when move is called so i need a local copy
            float animatorZ = animator.GetFloat(zId);

            tween.Insert(0, DOTween.To(
                () => t,
                val =>
                {
                    t = val;

                    if (t < 1)
                    {
                        float deltaPerSecond = 3;

                        Vector3 direction = myTransform.InverseTransformDirection(position - myTransform.position).normalized;
                        float inverseT = 1 - t;

                        animatorX = AxisClass.MoveTowardsInput(animatorX, direction.x * inverseT, deltaPerSecond, snap: true);
                        animatorZ = AxisClass.MoveTowardsInput(animatorZ, direction.z * inverseT, deltaPerSecond, snap: true);
                        animator.SetFloat(xId, animatorX);
                        animator.SetFloat(zId, animatorZ);
                    }
                    else
                    {
                        animator.SetFloat(xId, 0);
                        animator.SetFloat(zId, 0);
                    }
                },
                1,
                seconds
            ));

            tween.SetTarget(this);

            return tween;
        }
    }
}