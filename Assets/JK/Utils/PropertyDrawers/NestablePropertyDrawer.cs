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
    public class NestablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            // non so se sia fabbile, hai il campo fieldInfo per sapere che attributi ci sono
            // da quelli dovresti risalire alla classe del drawer
            // a quel punto però se la istanzi non so se puoi assegnare i campi attribute e fieldInfo
            // però poi unity mi chiama per ogni attributo
        }
    }

#endif
}