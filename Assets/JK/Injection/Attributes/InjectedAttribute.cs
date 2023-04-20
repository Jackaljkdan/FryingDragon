using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectedAttribute : PropertyAttribute
    {
    }
}