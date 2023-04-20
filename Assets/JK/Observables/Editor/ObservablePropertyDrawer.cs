using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Observables.Editor
{
    [CustomPropertyDrawer(typeof(ObservableProperty<>))]
    public class ObservablePropertyDrawer : PropertyDrawer
    {
        public bool foldout = false;

        public float height = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");

            height = EditorGUI.GetPropertyHeight(valueProperty);

            Rect firstHalfPosition = position;
            firstHalfPosition.height = height;
            firstHalfPosition.width /= 2;

            Rect secondHalfPosition = firstHalfPosition;
            secondHalfPosition.x += firstHalfPosition.width;

            EditorGUI.BeginProperty(position, label, property);

            foldout = EditorGUI.Foldout(firstHalfPosition, foldout, label);
            EditorGUI.PropertyField(secondHalfPosition, valueProperty, GUIContent.none);

            if (foldout)
            {
                Rect foldoutPosition = EditorGUI.IndentedRect(position);
                foldoutPosition.y += height;

                SerializedProperty onChangeProperty = property.FindPropertyRelative("onChange");
                EditorGUI.PropertyField(foldoutPosition, onChangeProperty);

                height += EditorGUI.GetPropertyHeight(onChangeProperty);
            }

            EditorGUI.EndProperty();
        }
    }
}