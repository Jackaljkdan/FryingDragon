using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JK.Utils
{
    public static class AssetDatabaseUtils
    {
        public static string GetGuid(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long localId))
                return guid;
            else
                return null;
#else
            return null;
#endif
        }

        public static bool TryGetGuid(UnityEngine.Object obj, out string guid)
        {
            guid = GetGuid(obj);
            return guid != null;
        }

        public static string GetAssetPath(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            return AssetDatabase.GetAssetPath(obj);
#else
            return null;
#endif
        }

        public static bool TryGetAssetPath(UnityEngine.Object obj, out string assetPath)
        {
            assetPath = GetAssetPath(obj);
            return !string.IsNullOrEmpty(assetPath);
        }

        /// <summary>
        /// N.B. non è deprecato ma non funziona e non so perché
        /// </summary>
        [Obsolete]
        public static bool TryFindAssetGuid(UnityEngine.Object obj, out string guid)
        {
#if UNITY_EDITOR
            // https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html
            string filter = obj.name + " t:" + obj.GetType().FullName;
            var guids = AssetDatabase.FindAssets(filter);

            if (guids.Length == 1)
            {
                guid = guids[0];
                return true;
            }
#endif

            guid = null;
            return false;
        }

        public static string GUIDFromAssetPath(string path)
        {
#if UNITY_EDITOR
            var guid = AssetDatabase.GUIDFromAssetPath(path);

            if (guid.CompareTo(new GUID("0")) != 0)
                return guid.ToString();
#endif

            return null;
        }

        public static string GUIDToAssetPath(string guid)
        {
#if UNITY_EDITOR
            return AssetDatabase.GUIDToAssetPath(guid);
#else
            return null;
#endif
        }

        public static T LoadAssetAtPath<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>(path);
#else
            return null;
#endif
        }
    }
}