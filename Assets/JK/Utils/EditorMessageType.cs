using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public enum EditorMessageType
    {
        None,
        Info,
        Warning,
        Error,
    }
}