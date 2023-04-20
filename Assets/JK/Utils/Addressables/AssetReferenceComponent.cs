using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JK.Utils.Addressables
{
    /// <summary>
    /// https://github.com/Unity-Technologies/Addressables-Sample/blob/master/Basic/ComponentReference/Assets/Samples/Addressables/1.19.19/ComponentReference/ComponentReference.cs
    /// </summary>
    [Serializable]
    public class AssetReferenceComponent<TComponent> : AssetReference where TComponent : Component
    {
        public static readonly AssetReferenceComponent<TComponent> Null = new AssetReferenceComponent<TComponent>(null);

        public AssetReferenceComponent(string guid) : base(guid) { }

        public new AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateChainOperation(base.InstantiateAsync(position, rotation, parent), GameObjectReady);
        }

        public new AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
        {
            return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateChainOperation(base.InstantiateAsync(parent, instantiateInWorldSpace), GameObjectReady);
        }

        public AsyncOperationHandle<TComponent> LoadAssetAsync()
        {
            return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateChainOperation(base.LoadAssetAsync<GameObject>(), GameObjectReady);
        }

        public AsyncOperationHandle LoadAssetAsyncIfNecessary()
        {
            if (OperationHandle.IsValid())
                return OperationHandle;
            else
                return LoadAssetAsync();
        }

        public TComponent Cast()
        {
            return ((GameObject)Asset).GetComponent<TComponent>();
        }

        private AsyncOperationHandle<TComponent> GameObjectReady(AsyncOperationHandle<GameObject> arg)
        {
            var comp = arg.Result.GetComponent<TComponent>();
            return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateCompletedOperation(comp, errorMsg: string.Empty);
        }

        public override bool ValidateAsset(UnityEngine.Object obj)
        {
            var go = obj as GameObject;
            return go != null && go.GetComponent<TComponent>() != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            // this load can be expensive...
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return go != null && go.GetComponent<TComponent>() != null;
#else
            return false;
#endif
        }

        public void ReleaseInstance(AsyncOperationHandle<TComponent> op)
        {
            // Release the instance
            var component = op.Result as Component;
            if (component != null)
            {
                UnityEngine.AddressableAssets.Addressables.ReleaseInstance(component.gameObject);
            }

            // Release the handle
            UnityEngine.AddressableAssets.Addressables.Release(op);
        }
    }

    public static class AssetReferenceComponentUtils
    {
        public static bool IsNull<TComponent>(this AssetReferenceComponent<TComponent> self) where TComponent : Component
        {
            if (self == null)
                return true;

            if (self == AssetReferenceComponent<TComponent>.Null)
                return true;

            if (PlatformUtils.IsEditor && self.GetEditorAsset() == null)
                return true;

            return false;
        }

        public static bool HasGuid<TComponent>(this AssetReferenceComponent<TComponent> self, string guid) where TComponent : Component
        {
            if (self.IsNull())
                return false;
            else
                return self.AssetGUID == guid;
        }

        public static TComponent InstantiateAsPrefab<TComponent>(AssetReferenceComponent<TComponent> asset, Transform parent) where TComponent : Component
        {
            return InstantiateAsPrefab(asset, parent, Vector3.zero, Quaternion.identity);
        }

        public static TComponent InstantiateAsPrefab<TComponent>(AssetReferenceComponent<TComponent> asset, Transform parent, Vector3 localPosition, Quaternion localRotation) where TComponent : Component
        {
            string path = AssetDatabaseUtils.GUIDToAssetPath(asset.AssetGUID);
            var prefab = AssetDatabaseUtils.LoadAssetAtPath<TComponent>(path);
            var instance = PrefabUtils.InstantiatePrefab(prefab, parent);
            ((Component)instance).transform.SetLocalPositionAndRotation(localPosition, localRotation);
            return (TComponent)instance;
        }
    }
}