using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Items
{
    [Serializable]
    public struct ItemRemovedSignal
    {
        public GameObject item;
    }
}