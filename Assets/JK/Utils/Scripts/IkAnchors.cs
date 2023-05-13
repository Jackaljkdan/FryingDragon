using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class IkAnchors : MonoBehaviour
    {
        #region Inspector

        public Transform rightHand;
        public Transform leftHand;

        public Transform leftElbow;

        #endregion

        public void SetIKWithWeight(Animator animator, float weight)
        {
            if (rightHand != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
            }

            if (leftHand != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
            }

            if (leftElbow != null)
            {
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, weight);
                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbow.position);
            }
        }
    }
}