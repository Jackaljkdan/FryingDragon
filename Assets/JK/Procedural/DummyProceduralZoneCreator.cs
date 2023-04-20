using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class DummyProceduralZoneCreator : AbstractProceduralZoneCreator
    {
        #region Inspector

        public List<AssetReferencePoolableComponent<Zone>> zones;

        #endregion

        public override List<AssetReferencePoolableComponent<Zone>> GetZoneAssets(ZoneConnection connection)
        {
            return zones;
        }

        public override void FillUnfittableConnection(ZoneConnection connection)
        {
            Debug.Log($"unfittable {connection}!");
        }
    }
}