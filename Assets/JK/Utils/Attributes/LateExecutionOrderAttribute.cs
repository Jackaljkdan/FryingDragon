using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public class LateExecutionOrderAttribute : DefaultExecutionOrder
    {
        public LateExecutionOrderAttribute() : base(1) { }
    }
}