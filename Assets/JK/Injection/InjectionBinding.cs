using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    public class InjectionBinding : MonoBehaviour
    {
        #region Inspector

        public List<Component> components;

        public GameObject targetGameObject;

        public string id = string.Empty;

        public InjectionType injectionType = InjectionType.Self;

        private void OnValidate()
        {
            onValidateCalled?.Invoke();
        }

        #endregion

        public static event Action onValidateCalled;

        [Serializable]
        public enum InjectionType
        {
            Self,
            Base,
            Interfaces,
        }

        private void Awake()
        {
            Bind();
        }

        public void Bind()
        {
            Context context = Context.Find(this);

            switch (injectionType)
            {
                default:
                case InjectionType.Self:
                    BindToSelf(context);
                    break;
                case InjectionType.Base:
                    BindToBase(context);
                    break;
                case InjectionType.Interfaces:
                    BindToInterfaces(context);
                    break;
            }

            if (targetGameObject != null)
                context.BindUnsafe(targetGameObject, typeof(GameObject), id);
        }

        private void BindToSelf(Context context)
        {
            foreach (var comp in components)
                context.BindUnsafe(comp, comp.GetType(), id);
        }

        private void BindToBase(Context context)
        {
            foreach (var comp in components)
                context.BindUnsafe(comp, comp.GetType().BaseType, id);
        }

        private void BindToInterfaces(Context context)
        {
            foreach (var comp in components)
                foreach (Type interfaceType in comp.GetType().GetInterfaces())
                    context.BindUnsafe(comp, interfaceType, id);
        }
    }
}