using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public abstract class AbstractProceduralZoneCreator : MonoBehaviour
    {
        #region Inspector

        public Transform fittersParent;

        private void Reset()
        {
            fittersParent = transform.Find("Fitters");

            if (fittersParent == null)
            {
                var fittersGo = new GameObject("Fitters");
                fittersGo.transform.SetParent(transform);
                fittersParent = fittersGo.transform;
            }
        }

        #endregion

        protected virtual void Start()
        {
            foreach (Zone zone in GetComponentsInChildren<Zone>())
                SetupFurnishedZone(zone);
        }

        public Coroutine TryCreateZoneForConnection(ZoneConnection connection)
        {
            if (!connection.NeedsPeer)
                return null;

            connection.isConnectingAsync = true;
            return StartCoroutine(TryInstantiatingFittingZoneCoroutine(connection));
        }

        private IEnumerator TryInstantiatingFittingZoneCoroutine(ZoneConnection connection)
        {
            // TODO: questo produce garbage, ma non penso di saper fare di meglio
            // devo creare una copia perché durante il foreach mi interrompo con uno yield
            // e questo metodo potrebbe essere richiamato di nuovo nel frattempo
            // a quel punto se non avessi fatto una copia farei un nuovo shuffle della lista stessa
            // e romperei il foreach di prima con un "collection was modified"
            var shuffledAssets = new List<AssetReferencePoolableComponent<Zone>>(GetZoneAssets(connection));
            shuffledAssets.ShuffleInPlace();

            foreach (AssetReferencePoolableComponent<Zone> asset in shuffledAssets)
            {
                var operation = asset.GetPoolableAsync(transform);
                yield return operation.WaitUntilDone();

                Zone baseInstance = operation.Result;
                baseInstance.gameObject.SetActive(true);

                baseInstance.Connections.ShuffleInPlace();

                foreach (ZoneConnection baseConnection in baseInstance.Connections)
                {
                    baseConnection.MoveTo(connection);
                    Physics.SyncTransforms();

                    if (baseInstance.IsFittingCurrentPosition())
                    {
                        Transform baseTransform = baseInstance.transform;
                        Vector3 basePosition = baseTransform.position;
                        Quaternion baseRotation = baseTransform.rotation;

                        operation = baseInstance.furnishedAsset.InstantiateAsync(basePosition, baseRotation, transform);
                        yield return operation.WaitUntilDone();

                        Zone furnishedInstance = operation.Result;

                        baseInstance.ReorderConnections();
                        ZoneConnection furnishedConnection = baseConnection.GetMatchingConnection(furnishedInstance);
                        furnishedConnection.Connect(connection);
                        SetupFurnishedZone(furnishedInstance);

                        connection.isConnectingAsync = false;

                        baseInstance.gameObject.SetActive(false);

                        yield break;
                    }
                }

                //Debug.Log("could not fit: " + baseInstance);

                baseInstance.gameObject.SetActive(false);
            }

            connection.enabled = false;
            connection.isConnectingAsync = false;
            FillUnfittableConnection(connection);
        }

        public void SetupFurnishedZone(Zone furnishedZone)
        {
            furnishedZone.SetConnectionsActive(true);
            furnishedZone.fitter.transform.SetParent(fittersParent, worldPositionStays: true);
        }

        public abstract List<AssetReferencePoolableComponent<Zone>> GetZoneAssets(ZoneConnection connection);

        public abstract void FillUnfittableConnection(ZoneConnection connection);
    }
}