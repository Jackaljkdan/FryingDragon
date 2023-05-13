using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class CanvasCamera : MonoBehaviour
    {
        #region Inspector

        public Canvas canvas;

        public float planeDistance = 1;

        public string injectionId = "ui";

        [Injected]
        public new Camera camera;

        private void Reset()
        {
            canvas = GetComponent<Canvas>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            camera = context.Get<Camera>(this, injectionId);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = planeDistance;
        }
    }
}