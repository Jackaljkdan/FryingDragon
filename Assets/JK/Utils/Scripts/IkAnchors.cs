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

        #endregion

        public void SetIKWithWeight(Animator animator, float weight)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
        }
    }
}