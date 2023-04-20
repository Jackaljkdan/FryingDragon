using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class JkDebug
    {
        public static void Log(string message)
        {
            if (IsSpam(message))
                return;

            lastLoggedMessage = message;
            Debug.Log(message);
        }

        public static void LogWarning(string message)
        {
            if (IsSpam(message))
                return;

            lastLoggedMessage = message;
            Debug.LogWarning(message);
        }

        public static void LogError(string message)
        {
            if (IsSpam(message))
                return;

            lastLoggedMessage = message;
            Debug.LogError(message);
        }

        public static bool IsSpam(string message)
        {
            return lastLoggedMessage == message;
        }

        private static string lastLoggedMessage;
    }
}