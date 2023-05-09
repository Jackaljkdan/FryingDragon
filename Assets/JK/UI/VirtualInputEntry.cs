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

        public static VirtualInputEntry From(VirtualInputEntry other)
        {
            return new VirtualInputEntry()
            {
                realEntry = other.realEntry,
                buttonType = other.buttonType,
                text = other.text,
            };
        }

        public static VirtualInputEntry From(VirtualInputEntry other, InputEntry realEntry)
        {
            return new VirtualInputEntry()
            {
                realEntry = realEntry,
                buttonType = other.buttonType,
                text = other.text,
            };
        }
    }
}