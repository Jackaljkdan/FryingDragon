using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class CharacterControllerPhysics : MonoBehaviour
    {
        public float collisionForce = 2f;

        private void Start()
        {
            // allow disabling in the inspector
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!enabled)
                return;

            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;

            // push rigidbodies away
            Vector3 pushDir = hit.gameObject.transform.position - transform.position;
            pushDir.y = 0;
            pushDir.Normalize();
            body.AddForce(pushDir * collisionForce);

            // workaround because unity does not call OnCollision* when the character controller collides
            if (body.TryGetComponent(out CharacterControllerHitListener listner))
                listner.OnControllerColliderHit(hit);
        }
    }
}