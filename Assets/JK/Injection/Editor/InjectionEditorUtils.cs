using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Editor
{
    [InitializeOnLoad]
    public static class InjectionEditorUtils
    {
        static InjectionEditorUtils()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            InjectionBinding.onValidateCalled += OnBindingChanged;

            areContextsDirty = true;
        }

        private static void OnHierarchyChanged()
        {
            areContextsDirty = true;
        }

        private static void OnBindingChanged()
        {
            areContextsDirty = true;
        }

        private static bool areContextsDirty = true;

        private static List<MonoContext> contextsBuffer;

        public static void InitContexts(Component component)
        {
            if (!areContextsDirty)
                return;

            if (Application.isPlaying)
                return;

            if (contextsBuffer == null)
                contextsBuffer = new List<MonoContext>(8);

            component.transform.root.GetComponentsInChildren(includeInactive: false, contextsBuffer);

            ProjectContext projectContext = ProjectContext.Get();

            if (projectContext != null)
            {
                projectContext.Clear();
                projectContext.InitIfNeeded();
                InitBindings(projectContext);
            }

            if (contextsBuffer.Count > 0)
            {
                foreach (MonoContext monoContext in contextsBuffer)
                    monoContext.Clear();

                foreach (MonoContext monoContext in contextsBuffer)
                    monoContext.InitIfNeeded();

                InitBindings(contextsBuffer[0]);
            }

            areContextsDirty = false;
        }

        private static List<InjectionBinding> bindingsBuffer;

        private static void InitBindings(MonoBehaviour root)
        {
            if (bindingsBuffer == null)
                bindingsBuffer = new List<InjectionBinding>(32);

            root.GetComponentsInChildren(bindingsBuffer);

            foreach (InjectionBinding binding in bindingsBuffer)
                binding.Bind();
        }
    }
}