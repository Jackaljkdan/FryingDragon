using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class OcclusionCullingListener : MonoBehaviour
    {
        #region Inspector

        public float radius = 3;

        public string cameraInjectionKey = "player.camera";

        public bool cullGameObject = true;

        public Color gizmoColor = new Color(1, 0.647f, 0);

        public UnityEvent onVisible = new UnityEvent();
        public UnityEvent onInvisible = new UnityEvent();

        [Injected]
        public new Camera camera;

        #endregion

        protected CullingGroup cullingGroup;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            camera = context.Get<Camera>(this, cameraInjectionKey);
        }

        private void Awake()
        {
            Inject();
        }

        protected virtual void Start()
        {
            cullingGroup = new CullingGroup();
            cullingGroup.targetCamera = camera;

            cullingGroup.SetBoundingSpheres(new BoundingSphere[]
            {
                new BoundingSphere(transform.position, radius),
            });

            cullingGroup.SetBoundingSphereCount(1);

            cullingGroup.onStateChanged = OnCullingStateChanged;

            if (!cullingGroup.IsVisible(0))
            {
                if (cullGameObject)
                    gameObject.SetActive(false);

                onInvisible.Invoke();
            }
        }

        private void OnDestroy()
        {
            if (cullingGroup != null)
            {
                cullingGroup.onStateChanged -= OnCullingStateChanged;
                cullingGroup.Dispose();
            }
        }

        private void OnCullingStateChanged(CullingGroupEvent ev)
        {
            if (ev.hasBecomeVisible)
            {
                //Debug.Log($"{transform.parent.name} onculling visible");

                if (cullGameObject)
                    gameObject.SetActive(true);

                onVisible.Invoke();
            }
            else if (ev.hasBecomeInvisible)
            {
                //Debug.Log($"{transform.parent.name} onculling INvisible");

                if (cullGameObject)
                    gameObject.SetActive(false);

                onInvisible.Invoke();
            }
        }

        public bool IsVisible()
        {
            return cullingGroup.IsVisible(0);
        }

        private void OnDrawGizmosSelected()
        {
            HandlesUtils.WithRestoredColor(() =>
            {
                HandlesUtils.SetColor(gizmoColor);
                HandlesUtils.DrawWireDisc(transform.position, Vector3.up, radius);
                HandlesUtils.DrawWireDisc(transform.position, Vector3.right, radius);
            });
        }
    }
}