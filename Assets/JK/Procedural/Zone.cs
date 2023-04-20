using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class Zone : MonoBehaviour
    {
        #region Inspector

        public Transform connectionsParent;

        public ZoneFitter fitter;

        public Zone basePrefab;

        public AssetReferenceComponent<Zone> furnishedAsset;

        [DebugHeader]

        [ReadOnly(callbackMethodName: nameof(Validate))]
        public bool _isFittingInEditMode;

        [ContextMenu("Log is fitting")]
        private void LogIsFittingInEditMode()
        {
            fitter.Init();
            Debug.Log("is zone fitting: " + IsFittingCurrentPosition());
        }

        [ContextMenu("Apply relevant overrides to base prefab")]
        [AutomatedTask]
        private void ApplyRelevantOverridesToBasePrefab()
        {
            if (!PrefabUtils.IsEditedPrefab(this))
                return;

            if (!furnishedAsset.IsNull())
                return;

            if (!PrefabUtils.TryGetBasePrefab(this, out basePrefab))
                return;

            string guid = PrefabUtils.GetCurrentPrefabGuid();

            if (!basePrefab.furnishedAsset.HasGuid(guid))
            {
                if (AddressablesUtils.TryMarkAddressable(basePrefab))
                {
                    AddressablesUtils.MarkAddressable(guid);
                    basePrefab.furnishedAsset = new AssetReferenceComponent<Zone>(guid);
                    furnishedAsset = AssetReferenceComponent<Zone>.Null;

                    UndoUtils.SetDirty(basePrefab);
                    UndoUtils.SetDirty(this);

                    PrefabUtils.RecordPrefabInstancePropertyModifications(basePrefab);
                    PrefabUtils.RecordPrefabInstancePropertyModifications(this);
                }
            }

            string baseAssetPath = AssetDatabaseUtils.GetAssetPath(basePrefab);

            PrefabUtils.ApplyAutomatedPropertyOverride(this, nameof(fitter), baseAssetPath);

            if (fitter != null)
            {
                PrefabUtils.ApplyAutomatedObjectOverride(fitter, baseAssetPath);
                PrefabUtils.ApplyAutomatedObjectOverride(fitter.transform, baseAssetPath);
                PrefabUtils.ApplyAutomatedObjectOverride(fitter.gameObject, baseAssetPath);

                fitter.Init();

                foreach (Component collider in fitter.EnumerateColliders())
                    PrefabUtils.ApplyAutomatedAddedGameObjectOrOverrides(collider.gameObject, baseAssetPath);
            }

            PrefabUtils.ApplyAutomatedPropertyOverride(this, nameof(connectionsParent), baseAssetPath);

            if (connectionsParent != null)
            {
                PrefabUtils.ApplyAutomatedObjectOverride(connectionsParent, baseAssetPath);
                PrefabUtils.ApplyAutomatedObjectOverride(connectionsParent.gameObject, baseAssetPath);

                foreach (var connection in connectionsParent.GetComponentsInChildren<ZoneConnection>(includeInactive: true))
                {
                    connection.gameObject.SetActive(false);
                    PrefabUtils.RecordPrefabInstancePropertyModifications(connection.gameObject);
                    PrefabUtils.ApplyAutomatedAddedGameObjectOrOverrides(connection.gameObject, baseAssetPath);
                }
            }

            PrefabUtils.SavePrefabAsset(basePrefab.gameObject);
            PrefabUtils.SaveCurrentPrefab();
        }

        private void Reset()
        {
            connectionsParent = transform;
            fitter = GetComponentInChildren<ZoneFitter>();
            Validate();
        }

        [ContextMenu("Validate")]
        private void Validate()
        {
            fitter.Init();
            _isFittingInEditMode = IsFittingCurrentPosition();
        }

        #endregion

        public List<ZoneConnection> Connections { get; private set; }

        private void Awake()
        {
            // troppo scomodo ed error prone da fare in editor e dev'essere pronto appena instanziato
            Init();

            // controllo che le connection non siano attive, altrimenti triggerano la generazione procedurale troppo presto
            if (PlatformUtils.IsEditor)
            {
                foreach (ZoneConnection connection in Connections)
                    Assert.IsFalse(connection.gameObject.activeInHierarchy);
            }
        }

        public void Init()
        {
            // devo essere già pronto per la generazione procedurale
            Connections = new List<ZoneConnection>(8);
            ReorderConnections();

            foreach (ZoneConnection connection in Connections)
                connection.Init();
        }

        public void ReorderConnections()
        {
            connectionsParent.GetComponentsInChildren(includeInactive: true, Connections);
        }

        public void SetConnectionsActive(bool value)
        {
            if (value && !connectionsParent.gameObject.activeSelf)
                connectionsParent.gameObject.SetActive(true);

            foreach (ZoneConnection connection in Connections)
                connection.gameObject.SetActive(value);
        }

        public bool IsFittingCurrentPosition()
        {
            return fitter.IsFitting();
        }
    }
}