using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IconBoxAttribute : PropertyAttribute
    {
        public string title;
        public string methodName;
        public float debounceSeconds = 0.2f;
        public bool hideField = false;

        public IconBoxAttribute(string methodName)
        {
            this.methodName = methodName;
        }

        public IconBoxAttribute(string methodName, bool hideField)
        {
            this.methodName = methodName;
            this.hideField = hideField;
        }

        public IconBoxAttribute(string title, string methodName)
        {
            this.title = title;
            this.methodName = methodName;
        }

        public IconBoxAttribute(string methodName, float debounceSeconds)
        {
            this.methodName = methodName;
            this.debounceSeconds = debounceSeconds;
        }

        public IconBoxAttribute(string title, string methodName, float debounceSeconds)
        {
            this.title = title;
            this.methodName = methodName;
            this.debounceSeconds = debounceSeconds;
        }
    }
}