using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.Editor
{
    public static class EditorMessageTypeUtils
    {
        public static MessageType Convert(this EditorMessageType type)
        {
            switch (type)
            {
                case EditorMessageType.None:
                default:
                    return MessageType.None;
                case EditorMessageType.Info:
                    return MessageType.Info;
                case EditorMessageType.Warning:
                    return MessageType.Warning;
                case EditorMessageType.Error:
                    return MessageType.Error;
            }
        }
    }
}