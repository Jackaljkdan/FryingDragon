using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class ProceduralTrigger : MonoBehaviour
    {
        #region Inspector

        [Injected]
        public AbstractProceduralZoneCreator zoneCreator;

        private void Reset()
        {
            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
            UndoUtils.SetDirty(collider);
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            zoneCreator = context.Get<AbstractProceduralZoneCreator>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponentInParent(out ZoneConnection connection) && connection.NeedsPeer)
                zoneCreator.TryCreateZoneForConnection(connection);
        }
    }
}