using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class CharacterControllerAgent : MonoBehaviour
    {
        #region Inspector

        public float rotationLerp = 0.04f;

        public CharacterController characterController;
        public NavMeshAgent agent;
        public Animator animator;

        private void Reset()
        {
            characterController = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        #endregion

        public bool MoveTo(Vector3 position)
        {
            Transform myTransform = transform;

            bool destinationSet = agent.SetDestination(position);
            SimpleMoveTowards(myTransform, agent.nextPosition);
            agent.nextPosition = myTransform.position;

            return destinationSet;
        }

        public void SimpleMoveTowards(Vector3 targetPosition)
        {
            SimpleMoveTowards(transform, targetPosition);
        }

        private void SimpleMoveTowards(Transform myTransform, Vector3 targetPosition)
        {
            Vector3 deltaWithZeroY = (targetPosition - myTransform.position).WithY(0);

            if (deltaWithZeroY != Vector3.zero)
            {
                Vector3 targetDirection = deltaWithZeroY.normalized;
                myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.LookRotation(targetDirection, Vector3.up), TimeUtils.AdjustToFrameRate(rotationLerp * animator.speed));
            }

            characterController.SimpleMove(animator.deltaPosition.WithY(0) * 60);
        }

    }
}