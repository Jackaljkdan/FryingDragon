using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [Serializable]
    public class Context
    {
        public const string EmptyId = "";

        public string name;

        private InjectionDictionary dict = new InjectionDictionary();

        private Context container;

        public Context(Context container, string name)
        {
            this.container = container;
            this.name = name;
        }

        public T Get<T>(string id = EmptyId)
        {
            return Get<T>(injector: null, id);
        }

        public T Get<T>(object injector, string id = EmptyId)
        {
            return (T)Get(injector, typeof(T), id);
        }

        public object Get(Type type, string id = EmptyId)
        {
            return Get(injector: null, type, id);
        }

        public object Get(object injector, Type type, string id = EmptyId)
        {
            var key = dict.GetKey(type, id);

            if (dict.TryGetValue(key, out object value))
                return value;

            if (container != null)
                return container.Get(type, id);

            throw new BindingNotFoundException(this, injector, type, id);
        }

        public T GetOptional<T>(string id = EmptyId, T defaultValue = default)
        {
            return (T)GetOptional(typeof(T), id, defaultValue);
        }

        public object GetOptional(Type type, string id = EmptyId, object defaultValue = null)
        {
            var key = dict.GetKey(type, id);

            if (dict.TryGetValue(key, out object value))
                return value;

            if (container != null)
                return container.GetOptional(type, id, defaultValue);

            return defaultValue;
        }

        public bool TryGetOptional<T>(out T value, string id = EmptyId, T defaultValue = default)
        {
            bool found = TryGetOptional(typeof(T), out object obj, id, defaultValue);
            value = (T)obj;
            return found;
        }

        public bool TryGetOptional(Type type, out object value, string id = EmptyId, object defaultValue = null)
        {
            value = GetOptional(type, id, defaultValue: null);

            if (value != null)
                return true;

            value = defaultValue;
            return false;
        }

        public void Bind<T>(T value, string id = EmptyId)
        {
            BindUnsafe(value, typeof(T), allowOverwrite: false, id);
        }

        public void Rebind<T>(T value, string id = EmptyId)
        {
            BindUnsafe(value, typeof(T), allowOverwrite: true, id);
        }

        public void BindAs<TValue, TBinding>(TValue value, string id = EmptyId) where TValue : TBinding
        {
            BindUnsafe(value, typeof(TBinding), allowOverwrite: false, id);
        }

        public void RebindAs<TValue, TBinding>(TValue value, string id = EmptyId) where TValue : TBinding
        {
            BindUnsafe(value, typeof(TBinding), allowOverwrite: true, id);
        }

        /// <summary>
        /// This is unsafe because if you pass a <paramref name="value"/> that isn't
        /// of type <paramref name="type"/> then you will get an <see cref="InvalidCastException"/>
        /// when you try to <see cref="Get{T}(string)"/> it
        /// </summary>
        public void BindUnsafe(object value, Type type, string id = EmptyId)
        {
            BindUnsafe(value, type, allowOverwrite: false, id);
        }

        public void RebindUnsafe(object value, Type type, string id = EmptyId)
        {
            BindUnsafe(value, type, allowOverwrite: true, id);
        }

        private void BindUnsafe(object value, Type type, bool allowOverwrite, string id = EmptyId)
        {
            InjectionUtils.ThrowInEditorForInvalidId(id);

            var key = dict.GetKey(type, id);

            if (PlatformUtils.IsEditor && !allowOverwrite)
            {
                if (dict.ContainsKey(key))
                    Debug.LogError($"Context {name} already had a binding for type {type} with id {id} and it will be overwritten");
            }

            dict[key] = value;
        }

        public T GetOrBind<T>(T value, string id = EmptyId)
        {
            return (T)GetOrBindUnsafe(value, typeof(T), id);
        }

        public TBinding GetOrBindAs<TValue, TBinding>(TValue value, string id = EmptyId) where TValue : TBinding
        {
            return (TBinding)GetOrBindUnsafe(value, typeof(TBinding), id);
        }

        public object GetOrBindUnsafe(object value, Type type, string id = EmptyId)
        {
            try
            {
                return Get(injector: null, type, id);
            }
            catch (BindingNotFoundException)
            {
                BindUnsafe(value, type, id);
                return value;
            }
        }

        public T GetOrBind<T>(Func<T> valueFn, string id = EmptyId)
        {
            try
            {
                return (T)Get(injector: null, typeof(T), id);
            }
            catch (BindingNotFoundException)
            {
                T value = valueFn();
                BindUnsafe(value, typeof(T), id);
                return value;
            }
        }

        public TBinding GetOrBindAs<TValue, TBinding>(Func<TValue> valueFn, string id = EmptyId) where TValue : TBinding
        {
            try
            {
                return (TBinding)Get(injector: null, typeof(TBinding), id);
            }
            catch (BindingNotFoundException)
            {
                TBinding value = valueFn();
                BindUnsafe(value, typeof(TBinding), id);
                return value;
            }
        }

        public object GetOrBindUnsafeFn(Func<object> valueFn, Type type, string id = EmptyId)
        {
            try
            {
                return Get(injector: null, type, id);
            }
            catch (BindingNotFoundException)
            {
                object value = valueFn();
                BindUnsafe(value, type, id);
                return value;
            }
        }

        public override string ToString()
        {
            return $"{name} (Context)";
        }

        public static Context Find(Component component)
        {
            MonoContext monoContext = component.GetComponentInParent<MonoContext>();

            if (monoContext != null)
                return monoContext.context;

            monoContext = ProjectContext.Get();

            if (monoContext != null)
                return monoContext.context;

            return null;
        }

        public static bool TryFind(Component component, out Context context)
        {
            context = Find(component);
            return context != null;
        }
    }
}