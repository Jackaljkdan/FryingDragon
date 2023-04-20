using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class InjectMethodAttribute : Attribute
    {
        public InjectMethodAttribute()
        {

        }
    }
}