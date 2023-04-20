using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class StringUtils
    {
        public static string Colored(this string message, Color color)
        {
            return Colored(message, "#" + ColorUtility.ToHtmlStringRGB(color));
        }

        public static string Colored(this string message, string color)
        {
            return $"<color={color}>{message}</color>";
        }

        public static string Contextualized(this string message, Component component, bool includeHierarchy = false, bool includeClass = false)
        {
            return component.Contextualize(message, includeHierarchy, includeClass);
        }
    }
}