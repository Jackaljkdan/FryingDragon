using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JK.Utils
{
#if UNITY_EDITOR

    [Serializable]
    public abstract class AbstractTitleHeader : DecoratorDrawer
    {
        public static readonly GUIStyle HeaderStyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperLeft,
            normal = new GUIStyleState()
            {
                textColor = Color.white,
            },
            contentOffset = new Vector2(0, 16),
        };

        public GUIContent headerContent;

        public AbstractTitleHeader()
        {
            headerContent = CreateContent();
        }

        public abstract GUIContent CreateContent();

        public override bool CanCacheInspectorGUI()
        {
            return true;
        }

        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + HeaderStyle.contentOffset.y;
        }

        public override void OnGUI(Rect position)
        {
            EditorGUI.LabelField(position, headerContent, HeaderStyle);
        }
    }

#endif
}