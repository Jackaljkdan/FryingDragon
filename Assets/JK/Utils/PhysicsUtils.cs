using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class PhysicsUtils
    {
        private static object editorSimulation;

        public static void StartSimulationInEditor()
        {
            if (Application.isPlaying)
                return;

            if (editorSimulation != null)
                return;

            Physics.autoSimulation = false;

            editorSimulation = EditorApplicationUtils.AddUpdateListener(() =>
            {
                Physics.Simulate(Time.fixedDeltaTime);
            });

            //int loops = Mathf.CeilToInt(seconds / Time.fixedDeltaTime);

            //for (int i = 0; i < loops; i++)
            //    Physics.Simulate(Time.fixedDeltaTime);

        }

        public static void StartSlowMotionSimulationInEditor(float secondsBetweenUpdates = 0.1f)
        {
            if (Application.isPlaying)
                return;

            if (editorSimulation != null)
                return;

            Physics.autoSimulation = false;

            DateTime lastUpdateDateTime = DateTime.Now;
            TimeSpan timeSpan = TimeSpan.FromSeconds(secondsBetweenUpdates);

            editorSimulation = EditorApplicationUtils.AddUpdateListener(() =>
            {
                if (DateTime.Now - lastUpdateDateTime >= timeSpan)
                {
                    Physics.Simulate(Time.fixedDeltaTime);
                    lastUpdateDateTime = DateTime.Now;
                }
            });
        }

        public static void StopSimulationInEditor()
        {
            if (editorSimulation == null)
                return;

            EditorApplicationUtils.RemoveUpdateListener(editorSimulation);
            editorSimulation = null;

            Physics.autoSimulation = true;
        }
    }
}