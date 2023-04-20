using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EarlyExecutionOrderAttribute : DefaultExecutionOrder
    {
        public EarlyExecutionOrderAttribute() : base(-1) { }
    }
}