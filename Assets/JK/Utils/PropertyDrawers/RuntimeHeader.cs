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

    [CustomPropertyDrawer(typeof(RuntimeHeaderAttribute))]
    public class RuntimeHeaderPropertyDrawer : AbstractTitleHeader
    {
        public override GUIContent CreateContent()
        {
            return new GUIContent("Runtime", "The values of the following fields are ignored and determined through code");
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Field)]
    public class RuntimeHeaderAttribute : PropertyAttribute
    {
    }

    // N.B. this does not work
    //[AttributeUsage(AttributeTargets.Field)]
    //public class RuntimeHeaderAttribute : HeaderAttribute
    //{
    //    public RuntimeHeaderAttribute() : base("Runtime") { }
    //}
}