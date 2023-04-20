using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class ShadowsLOD : AbstractRelaxed
    {
        #region Inspector

        public List<Level> visibleLODs = new List<Level>();
        public List<Level> invisibleLODs = new List<Level>();

        public Light targetLight;

        public Renderer targetRenderer;

        [DebugField, SerializeField]
        private Level prevLevel;

        [Injected]
        public Transform player;

        private void Reset()
        {
            updateSeconds = 0.1f;
            targetLight = GetComponentInChildren<Light>();
            targetRenderer = this.GetComponentInChildrenOrParent<Renderer>();
        }

        [ContextMenu("Reset LODs")]
        private void ResetLODsInEditMode()
        {
            visibleLODs = new List<Level>()
            {
                new Level()
                {
                    type = targetLight.shadows,
                    resolution = targetLight.shadowResolution,
                    nearPlane = targetLight.shadowNearPlane,
                    distance = 5,
                    visible = true,
                },
                new Level()
                {
                    type = targetLight.shadows,
                    resolution = LightShadowResolution.Low,
                    nearPlane = targetLight.shadowNearPlane,
                    distance = float.PositiveInfinity,
                    visible = true,
                },
            };

            invisibleLODs = new List<Level>()
            {
                new Level()
                {
                    type = targetLight.shadows,
                    resolution = LightShadowResolution.Low,
                    nearPlane = targetLight.shadowNearPlane,
                    distance = 5,
                    visible = false,
                },
                new Level()
                {
                    type = LightShadows.None,
                    distance = float.PositiveInfinity,
                    visible = false,
                },
            };
        }

        [ContextMenu("Validate")]
        private void OnValidate()
        {
            visibleLODs = visibleLODs
                .OrderBy(l => l.distance)
                .Select(l =>
                {
                    l.visible = true;
                    return l;
                })
                .ToList()
            ;

            invisibleLODs = invisibleLODs
                .OrderBy(l => l.distance)
                .Select(l =>
                {
                    l.visible = false;
                    return l;
                })
                .ToList()
            ;
        }

        #endregion

        [Serializable]
        public struct Level
        {
            public LightShadows type;
            public LightShadowResolution resolution;
            public float nearPlane;
            public float distance;
            public UnityEvent onSwitch;

            [HideInInspector]
            public bool visible;
        }

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            player = context.Get<Transform>(this, "player");
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            prevLevel = default;
        }

        public override void RelaxedUpdate()
        {
            List<Level> levels = targetRenderer.isVisible ? visibleLODs : invisibleLODs;

            float distance = Vector3.Distance(player.position, transform.position);
            Level currentLevel = prevLevel;

            foreach (var level in levels)
            {
                if (distance <= level.distance)
                {
                    currentLevel = level;
                    break;
                }
            }

            if (currentLevel.distance == prevLevel.distance && currentLevel.visible == prevLevel.visible)
                return;

            prevLevel = currentLevel;

            targetLight.shadows = currentLevel.type;
            targetLight.shadowResolution = LightShadowResolution.Low;
            targetLight.shadowNearPlane = currentLevel.nearPlane;

            currentLevel.onSwitch?.Invoke();
        }
    }
}