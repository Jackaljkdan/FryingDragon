using JK.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.UI
{
    [Serializable]
    public struct VirtualInputEntry
    {
        public InputEntry realEntry;
        public ButtonType buttonType;
        public string text;
    }
}