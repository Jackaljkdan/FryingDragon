using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class LightLOD : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private List<Level> levels;

        [DebugField]
        public int enabledLightIndex;

        private void Reset()
        {
            levels = GetComponentsInChildren<Light>().Select(light => new Level()
            {
                light = light,
                performance = light.gameObject.GetOrAddComponent<EventPerformanceActivationTarget>(),
            }).ToList();

            OnValidate();
        }

        [ContextMenu("Validate")]
        private void OnValidate()
        {
            if (levels == null)
                return;

            for (int i = 1; i < levels.Count; i++)
                SetupLodInEditMode(levels[i]);
        }

        private void SetupLodInEditMode(Level lod)
        {
            Level mainLod = levels[0];

            Light mainLight = mainLod.light;
            Light lodLight = lod.light;

            if (mainLight == null || lodLight == null)
                return;

            bool shouldSetDirty = false;

            shouldSetDirty = shouldSetDirty || lodLight.enabled;
            lodLight.enabled = false;

            shouldSetDirty = shouldSetDirty || lodLight.type != mainLight.type;
            lodLight.type = mainLight.type;

            shouldSetDirty = shouldSetDirty || lodLight.color != mainLight.color;
            lodLight.color = mainLight.color;

            shouldSetDirty = shouldSetDirty || lodLight.colorTemperature != mainLight.colorTemperature;
            lodLight.colorTemperature = mainLight.colorTemperature;

            shouldSetDirty = shouldSetDirty || lodLight.useColorTemperature != mainLight.useColorTemperature;
            lodLight.useColorTemperature = mainLight.useColorTemperature;

            shouldSetDirty = shouldSetDirty || lodLight.intensity != mainLight.intensity;
            lodLight.intensity = mainLight.intensity;

            shouldSetDirty = shouldSetDirty || lodLight.range != mainLight.range;
            lodLight.range = mainLight.range;

            shouldSetDirty = shouldSetDirty || lodLight.renderingLayerMask != mainLight.renderingLayerMask;
            lodLight.renderingLayerMask = mainLight.renderingLayerMask;

            if (shouldSetDirty)
                UndoUtils.SetDirty(lodLight);
        }

        #endregion

        [Serializable]
        public class Level
        {
            public Light light;
            public EventPerformanceActivationTarget performance;
            public UnityAction onActivate;
            public UnityAction onDeactivate;
        }

        public float intensity
        {
            get => levels[0].light.intensity;
            set
            {
                foreach (var lvl in levels)
                    lvl.light.intensity = value;
            }
        }

        private void Start()
        {
            enabledLightIndex = levels.Count - 1;
            levels[enabledLightIndex].light.enabled = true;

            for (int i = 0; i < levels.Count - 1; i++)
            {
                int index = i;
                var lvl = levels[i];

                lvl.light.enabled = false;

                lvl.onActivate = () =>
                {
                    if (index > enabledLightIndex)
                        return;

                    levels[enabledLightIndex].light.enabled = false;
                    levels[index].light.enabled = true;

                    enabledLightIndex = index;

                };

                lvl.onDeactivate = () =>
                {
                    if (index == enabledLightIndex)
                    {
                        levels[enabledLightIndex].light.enabled = false;
                        enabledLightIndex++;
                        levels[enabledLightIndex].light.enabled = true;
                    }
                };

                lvl.performance.onShouldActivate.AddListener(lvl.onActivate);
                lvl.performance.onShouldDeactivate.AddListener(lvl.onDeactivate);
            }
        }

        private void EnableCorrectLight()
        {
            for (int i = 0; i < levels.Count; i++)
                levels[i].light.enabled = i == enabledLightIndex;
        }

        private void OnEnable()
        {
            EnableCorrectLight();
        }

        //private void OnDisable()
        //{
        //    levels[0].light.enabled = true;

        //    for (int i = 1; i < levels.Count; i++)
        //        levels[i].light.enabled = false;
        //}

        private void OnDestroy()
        {
            foreach (var lvl in levels)
            {
                if (lvl.performance != null)
                {
                    if (lvl.onActivate != null)
                        lvl.performance.onShouldActivate.RemoveListener(lvl.onActivate);

                    if (lvl.onDeactivate != null)
                        lvl.performance.onShouldDeactivate.RemoveListener(lvl.onDeactivate);
                }
            }
        }

        //private void UpdateActiveLight()
        //{
        //    if (!PlatformUtils.IsEditor)
        //        return;

        //    foreach (var lvl in levels)
        //    {
        //        if (lvl.light.enabled)
        //        {
        //            activeLight = lvl.light;
        //            return;
        //        }
        //    }
        //}

        public Tween DOIntensity(float endValue, float seconds)
        {
            Sequence tween = DOTween.Sequence();

            foreach (var lvl in levels)
                tween.Insert(0, lvl.light.DOIntensity(endValue, seconds));

            tween.SetTarget(this);

            return tween;
        }
    }
}