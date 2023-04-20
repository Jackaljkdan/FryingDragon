using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace JK.Utils.Addressables
{
    public static class AddressablesUtils
    {
        public static bool TryMarkAddressable<T>(T component, out string guid) where T : Component
        {
#if UNITY_EDITOR
            if (component.gameObject.scene.IsValid())
            {
                guid = null;
                return false;
            }

            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(component, out guid, out long localId))
                return false;

            MarkAddressable(guid);

            return true;
#else
            guid = null;
            return false;
#endif
        }

        public static bool TryMarkAddressable<T>(T component) where T : Component
        {
            return TryMarkAddressable(component, out _);
        }

        public static void MarkAddressable(string guid)
        {
#if UNITY_EDITOR
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            var entry = settings.FindAssetEntry(guid);

            if (entry == null)
                entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
#endif
        }

        public static AssetReferenceT<T> FindAssetT<T>(string guid) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            var entry = settings.FindAssetEntry(guid);

            if (entry != null)
                return new AssetReferenceT<T>(entry.guid);
#endif

            return null;
        }

        public static bool TryFindAssetT<T>(string guid, out AssetReferenceT<T> asset) where T : UnityEngine.Object
        {
            asset = FindAssetT<T>(guid);
            return asset != null;
        }

        public static AssetReferenceT<T> FindAssetT<T>(T obj) where T : UnityEngine.Object
        {
            if (AssetDatabaseUtils.TryGetGuid(obj, out string guid))
                return FindAssetT<T>(guid);

            return null;
        }

        public static bool TryFindAssetT<T>(T obj, out AssetReferenceT<T> asset) where T : UnityEngine.Object
        {
            asset = FindAssetT(obj);
            return asset != null;
        }

        public static AssetReferenceGameObject FindGameObjectAsset(GameObject gameObject)
        {
            if (TryFindAssetT(gameObject, out AssetReferenceT<GameObject> asset))
                return new AssetReferenceGameObject(asset.AssetGUID);

            if (PrefabUtils.TryGetBasePrefab(gameObject, out GameObject prefab))
                if (AssetDatabaseUtils.TryGetGuid(prefab, out string guid))
                    if (TryFindAssetT(prefab, out asset))
                        return new AssetReferenceGameObject(asset.AssetGUID);

            return null;
        }

        public static bool TryFindGameObjectAsset(GameObject go, out AssetReferenceGameObject asset)
        {
            if (TryFindAssetT(go, out AssetReferenceT<GameObject> assetT))
            {
                asset = new AssetReferenceGameObject(assetT.AssetGUID);
                return true;
            }
            else
            {
                asset = null;
                return false;
            }
        }
    }
}