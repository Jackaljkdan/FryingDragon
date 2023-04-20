using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JK.Utils.Addressables
{
    [Serializable]
    public class AssetReferencePoolableComponent<TComponent> : AssetReferenceComponent<TComponent> where TComponent : Component
    {
        public AssetReferencePoolableComponent(string guid) : base(guid) { }

        public TComponent pooled;

        public AsyncOperationHandle<TComponent> GetPoolableAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (pooled == null)
                return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateChainOperation(InstantiateAsync(position, rotation, parent), PoolInstance);

            Transform pooledTransform = pooled.transform;
            pooledTransform.position = position;
            pooledTransform.rotation = rotation;
            pooledTransform.SetParent(parent);

            return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateCompletedOperation(pooled, errorMsg: string.Empty);
        }

        public AsyncOperationHandle<TComponent> GetPoolableAsync(Transform parent = null, bool instantiateInWorldSpace = false)
        {
            if (pooled == null)
                return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateChainOperation(InstantiateAsync(parent, instantiateInWorldSpace), PoolInstance);

            pooled.transform.SetParent(parent, worldPositionStays: instantiateInWorldSpace);

            return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateCompletedOperation(pooled, errorMsg: string.Empty);
        }

        private AsyncOperationHandle<TComponent> PoolInstance(AsyncOperationHandle<TComponent> arg)
        {
            pooled = arg.Result;
            return arg;
        }
    }
}