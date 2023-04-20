using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class CameraBobScript : MonoBehaviour
    {
        #region Inspector

        public float walkVerticalMovement = 0.03f;
        public float runVerticalMovement = 0.04f;

        public float offsetLerp = 0.1f;

        public float walkZRotation = 0.5f;
        public float runZRotation = 0.8f;

        public int movementLayer = 0;

        [FormerlySerializedAs("curve")]
        public AnimationCurve verticalCurve = new AnimationCurve();

        public AnimationCurve zRotationCurve = new AnimationCurve();

        [Range(0, 1)]
        public float curveTimeOffset = 0;

        [RuntimeField]
        public float initialY;

        [DebugField, SerializeField]
        private float t;

        [DebugField, SerializeField]
        private float movementMultiplier;

        [DebugField, SerializeField]
        public float offset;

        [Injected]
        public Animator playerAnimator;

        #endregion

        private int xId;
        private int zId;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            playerAnimator = context.Get<Animator>(this, "player");
        }

        private void Awake()
        {
            Inject();

            initialY = transform.localPosition.y;

            xId = Animator.StringToHash("X");
            zId = Animator.StringToHash("Z");
        }

        private void OnEnable()
        {
            offset = transform.localPosition.y - initialY;
        }

        private void OnDisable()
        {
            transform.localPosition = transform.localPosition.WithY(initialY);
        }

        private void Update()
        {
            AnimatorStateInfo animatorState = playerAnimator.GetCurrentAnimatorStateInfo(movementLayer);
            t = (animatorState.normalizedTime - curveTimeOffset) % 1;

            Vector2 movement = new Vector2(
                playerAnimator.GetFloat(xId),
                playerAnimator.GetFloat(zId)
            );

            movementMultiplier = Mathf.Clamp01(movement.magnitude / 2);

            // -1 because i don't want the camera to go higher
            float verticalCurveEval = verticalCurve.Evaluate(t) - 1;
            float verticalMovement = Mathf.Lerp(walkVerticalMovement, runVerticalMovement, movementMultiplier);

            Transform myTransform = transform;

            offset = Mathf.Lerp(offset, movementMultiplier * verticalCurveEval * verticalMovement, TimeUtils.AdjustToClampedFrameRate(offsetLerp));
            myTransform.localPosition = myTransform.localPosition.WithY(initialY + offset);

            float zRotationCurveEval = zRotationCurve.Evaluate(t);
            float zRotation = Mathf.Lerp(walkZRotation, runZRotation, movementMultiplier);
            myTransform.localEulerAngles = myTransform.localEulerAngles.WithZ(movementMultiplier * zRotationCurveEval * zRotation);
        }
    }
}