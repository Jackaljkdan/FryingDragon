using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    public class BindingNotFoundException : ArgumentException
    {
        private static string FormatInjector(object injector)
        {
            if (injector is Component component)
                return $"{component.GetHierarchyDotClassName()}";
            else
                return $"{injector}";
        }

        public BindingNotFoundException(Context context, object injector, Type type, string id)
            : base($"[{FormatInjector(injector)}] Binding of type {type} with id '{id}' was not found in context {context.name}") { }
    }
}