using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class LightOcclusionCulling : MonoBehaviour
    {
        #region Inspector

        public new Light light;

        public float rangeMultiplier = 0.7f;

        public string cameraInjectionKey = "player.camera";

        public Color gizmoColor = new Color(1, 0.647f, 0);

        public UnityEvent onVisible = new UnityEvent();
        public UnityEvent onInvisible = new UnityEvent();

        [Injected]
        public new Camera camera;

        private void Reset()
        {
            light = GetComponent<Light>();
        }

        #endregion

        private CullingGroup cullingGroup;

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

        private void Start()
        {
            cullingGroup = new CullingGroup();
            cullingGroup.targetCamera = camera;

            cullingGroup.SetBoundingSpheres(new BoundingSphere[]
            {
                new BoundingSphere(transform.position, light.range * rangeMultiplier),
            });

            cullingGroup.SetBoundingSphereCount(1);

            cullingGroup.onStateChanged = OnCullingStateChanged;

            if (!cullingGroup.IsVisible(0))
                gameObject.SetActive(false);
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

                onVisible.Invoke();
                gameObject.SetActive(true);
            }
            else if (ev.hasBecomeInvisible)
            {
                //Debug.Log($"{transform.parent.name} onculling INvisible");

                gameObject.SetActive(false);
                onInvisible.Invoke();
            }
        }

        private void OnDrawGizmosSelected()
        {
            HandlesUtils.WithRestoredColor(() =>
            {
                HandlesUtils.SetColor(gizmoColor);
                HandlesUtils.DrawWireDisc(transform.position, Vector3.up, light.range * rangeMultiplier);
                HandlesUtils.DrawWireDisc(transform.position, Vector3.right, light.range * rangeMultiplier);
            });
        }
    }
}