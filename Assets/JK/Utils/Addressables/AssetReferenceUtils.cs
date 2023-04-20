using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JK.Utils.Addressables
{
    public static class AssetReferenceUtils
    {
        public static AsyncOperationHandle LoadAssetAsyncIfNecessary<T>(this AssetReferenceT<T> self) where T : UnityEngine.Object
        {
            if (self.OperationHandle.IsValid())
                return self.OperationHandle;
            else
                return self.LoadAssetAsync();
        }

        public static AsyncOperationHandle LoadAssetAsyncIfNecessaryT<T>(this AssetReferenceT<T> self) where T : UnityEngine.Object
        {
            return self.LoadAssetAsyncIfNecessary();
        }

        public static Coroutine LoadAssetAsyncIfNecessaryAndWaitSeconds<T>(this AssetReferenceT<T> self, MonoBehaviour monoBehaviour, float seconds) where T : UnityEngine.Object
        {
            return monoBehaviour.StartCoroutine(self.LoadAssetAsyncIfNecessaryAndWaitSecondsCoroutine(monoBehaviour, seconds));
        }

        public static IEnumerator LoadAssetAsyncIfNecessaryAndWaitSecondsCoroutine<T>(this AssetReferenceT<T> self, MonoBehaviour monoBehaviour, float seconds) where T : UnityEngine.Object
        {
            float time = Time.time;

            yield return self.LoadAssetAsyncIfNecessaryT().WaitUntilDone();

            float elapsed = Time.time - time;
            float remaining = seconds - elapsed;

            if (remaining > 0)
                yield return new WaitForSeconds(remaining);
        }

        public static Coroutine RunAfterLoadAssetAsyncPlusSeconds<T>(this AssetReferenceT<T> self, MonoBehaviour monoBehaviour, float seconds, UnityAction action) where T : UnityEngine.Object
        {
            return monoBehaviour.StartCoroutine(coroutine());

            IEnumerator coroutine()
            {
                yield return self.LoadAssetAsyncIfNecessaryAndWaitSecondsCoroutine(monoBehaviour, seconds);
                action();
            }
        }

        public static Action<AsyncOperationHandle> GetAsync<T>(this AssetReferenceT<T> self, UnityAction action) where T : UnityEngine.Object
        {
            Action<AsyncOperationHandle> listener = _ => action();

            if (self.OperationHandle.IsValid())
                self.OperationHandle.Completed += listener;
            else
                self.LoadAssetAsync().CompletedTypeless += listener;

            return listener;
        }

        public static Action<AsyncOperationHandle> GetSafelyAsync<T>(this AssetReferenceT<T> self, Component component, UnityAction action) where T : UnityEngine.Object
        {
            return self.GetAsync(() =>
            {
                if (component != null)
                    action();
            });
        }

        public static T Cast<T>(this AssetReferenceT<T> self) where T : UnityEngine.Object
        {
            return (T)self.Asset;
        }

        public static UnityEngine.Object GetEditorAsset(this AssetReference self)
        {
#if UNITY_EDITOR
            return self.editorAsset;
#else
            return null;
#endif
        }

        public static T InstantiateAsPrefab<T>(AssetReferenceT<T> asset, Transform parent) where T : UnityEngine.Object
        {
            string path = AssetDatabaseUtils.GUIDToAssetPath(asset.AssetGUID);
            var prefab = AssetDatabaseUtils.LoadAssetAtPath<T>(path);
            var instance = PrefabUtils.InstantiatePrefab(prefab, parent);
            return (T)instance;
        }
    }
}