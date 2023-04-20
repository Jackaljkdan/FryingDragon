using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.Reflection;

namespace JK.Injection.Editor
{
    [CustomPropertyDrawer(typeof(InjectedAttribute))]
    public class InjectedPropertyDrawer : PropertyDrawer
    {
        public static GUIStyle buttonStyle;
        public static GUIStyle errorButtonStyle;
        public static GUIStyle errorBoxStyle;

        private void InitStylesIfNeeded()
        {
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.alignment = TextAnchor.MiddleLeft;

                errorButtonStyle = new GUIStyle(buttonStyle);
                errorButtonStyle.normal.textColor = Color.red;

                errorBoxStyle = new GUIStyle(GUI.skin.box);
                errorBoxStyle.normal.textColor = Color.red;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InitStylesIfNeeded();

            float iconSize = position.height;
            float iconMargin = 4;
            float prevWidth = position.width;

            position.width = iconSize;

            var tex = EditorGUIUtility.FindTexture("Assets/JK/Injection/Sprites/SyringeInspector.png");

            if (tex != null)
                EditorGUI.DrawPreviewTexture(position, tex);

            position.width = prevWidth;
            float buttonWidth = position.width / 2;

            position.x += iconSize + iconMargin;
            position.width -= buttonWidth + iconSize + iconMargin;
            EditorGUI.LabelField(position, label, GUI.skin.label);

            position.x += position.width;
            position.width = buttonWidth;

            object injectedValue;
            bool hasInjectionError;

            try
            {
                injectedValue = GetInjectedValue(property);
                hasInjectionError = false;
            }
            catch (BindingNotFoundException ex)
            {
                JkDebug.LogError(ex.Message);
                injectedValue = "Binding not found";
                hasInjectionError = true;
            }

            if (!property.propertyType.IsUnityObjectType())
            {
                EditorGUI.LabelField(
                    position,
                    $"{injectedValue} ({fieldInfo.FieldType.Name})",
                    !hasInjectionError ? GUI.skin.box : errorBoxStyle
                );
                return;
            }

            bool clicked = GUI.Button(
                position,
                $"{injectedValue} ({fieldInfo.FieldType.Name})",
                !hasInjectionError ? buttonStyle : errorButtonStyle
            );

            if (clicked)
            {
                injectedValue = GetInjectedValue(property);

                Assert.IsTrue(
                    injectedValue is UnityEngine.Object,
                    "Field is not a unity object despite property type " + property.propertyType
                );

                EditorGUIUtility.PingObject((UnityEngine.Object)injectedValue);
            }
        }

        private object GetInjectedValue(SerializedProperty property)
        {
            if (PrefabUtils.IsPrefabMode())
                return "Unavailable in prefab mode";

            if (!(property.serializedObject.targetObject is Component component))
                return null;

            if (!component.gameObject.scene.IsValid())
                return "Only available in scene";

            var injectMethods = JkTypeCache.GetMethodsWithAttribute<InjectMethodAttribute>(component.GetType());

            if (injectMethods.Count == 0)
            {
                JkDebug.LogError($"{component.GetType()} has no method marked [{nameof(InjectMethodAttribute)}]");
                return $"no method marked [{nameof(InjectMethodAttribute)}]";
            }

            InjectionEditorUtils.InitContexts(component);

            try
            {
                foreach (var method in injectMethods)
                    method.Invoke(component, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

            object injectedValue = fieldInfo.GetValue(component);
            return injectedValue;
        }
    }
}