using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class PlatformUtils
    {
#if UNITY_EDITOR
        public const bool IsEditor = true;
#else
        public const bool IsEditor = false;
#endif

#if UNITY_ANDROID || UNITY_IOS
        public const bool IsMobileBuild = true;
#else
        public const bool IsMobileBuild = false;
#endif

        public const bool IsDesktopBuild = !IsMobileBuild;

#if UNITY_WEBGL
        public const bool IsWebBuild = true;
#else
        public const bool IsWebBuild = false;
#endif
    }
}