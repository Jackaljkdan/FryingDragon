using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class AutomatedTaskAttribute : Attribute
    {
        public AutomatedTaskAttribute()
        {

        }
    }
}