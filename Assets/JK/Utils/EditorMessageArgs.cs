using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public struct EditorMessageArgs
    {
        public EditorMessageType type;
        public string message;
        public bool show;

        public EditorMessageArgs(EditorMessageType type, string message)
        {
            this.type = type;
            this.message = message;
            show = true;
        }

        public static readonly EditorMessageArgs DontShow;
    }
}