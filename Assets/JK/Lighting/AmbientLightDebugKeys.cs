using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class AmbientLightDebugKeys : MonoBehaviour
    {
        #region Inspector

        public float delta;

        [Injected]
        public EnvironmentLighting environmentLighting;

        #endregion

        private Color initialAmbientLight;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            environmentLighting = context.Get<EnvironmentLighting>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            initialAmbientLight = environmentLighting.initialAmbientLight;
        }

        private Color Sum(Color color, float v)
        {
            color.r += v;
            color.g += v;
            color.b += v;
            return color;
        }

        private void SetLight(Color color)
        {
            environmentLighting.initialAmbientLight = color;
            RenderSettings.ambientLight = color;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadPlus))
                SetLight(Sum(environmentLighting.initialAmbientLight, delta));
            else if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadMinus))
                SetLight(Sum(environmentLighting.initialAmbientLight, -delta));
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Keypad0))
                SetLight(initialAmbientLight);
        }
    }
}