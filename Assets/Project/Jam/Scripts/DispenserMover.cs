using UnityEngine;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class DispenserMover : MonoBehaviour
    {
        #region Inspector

        public Transform anchor1;
        public Transform anchor2;
        public float moveSpeed = 1f;

        #endregion

        private Transform targetAnchor;

        private void Update()
        {
            if (!targetAnchor)
                targetAnchor = anchor1;

            transform.position = Vector3.MoveTowards(transform.position, targetAnchor.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetAnchor.position) < 0.001f)
                ChangeAnchor();
        }

        public void ChangeAnchor()
        {
            targetAnchor = targetAnchor == anchor1 ? anchor2 : anchor1;
        }
    }
}
