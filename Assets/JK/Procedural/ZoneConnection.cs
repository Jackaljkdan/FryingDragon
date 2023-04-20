using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class ZoneConnection : MonoBehaviour
    {
        #region Inspector

        [field: SerializeField]
        public ZoneConnection Peer { get; private set; }

        [RuntimeHeader]

        [ReadOnly]
        public bool isConnectingAsync;

        [ContextMenu("Move To Connected")]
        private void MoveToConnectedInEditMode()
        {
            Init();
            // TODO: undo
            MoveTo(Peer);
        }

        [ContextMenu("Erase Connection")]
        private void EraseConnectionInEditMode()
        {
            // TODO: undo
            if (Peer != null)
            {
                Peer.Peer = null;
                Peer = null;
            }
        }

        #endregion

        [field: FormerlySerializedAs("ParentRoom")]
        public Zone ParentZone { get; private set; }

        public Zone ConnectedZone => Peer != null ? Peer.ParentZone : null;

        public bool NeedsPeer => enabled && Peer == null && !isConnectingAsync;

        public override string ToString()
        {
            return $"Connection {ParentZone.Connections.IndexOf(this)} {ParentZone.GetName()} -> {ConnectedZone.GetName()}";
        }

        private void Awake()
        {
            if (ParentZone == null)
                Init();
        }

        public void Init()
        {
            // avevo provato a fare una cosa che lo setta automaticamente in OnValidate ma non funziona e non so perché
            // non funziona in modo strano nel senso che quando è istanziato è null ma in editor lo vedo settato
            ParentZone = GetComponentInParent<Zone>();
        }

        private void Start()
        {

        }

        public void Connect(ZoneConnection target)
        {
            Peer = target;
            target.Peer = this;
        }

        public void ConnectAndMoveSelf(ZoneConnection target)
        {
            Connect(target);
            MoveTo(target);
        }

        public void MoveTo(ZoneConnection target)
        {
            Transform targetTransform = target.transform;
            Transform myTransform = transform;
            Transform parentZoneTransform = ParentZone.transform;

            //Quaternion connectionRotation = Quaternion.LookRotation(-targetTransform.forward, targetTransform.up);
            //Quaternion rotationOffset = Quaternion.FromToRotation(zone.transform.InverseTransformDirection(transform.forward), Vector3.forward);
            //zone.transform.rotation = rotationOffset * connectionRotation;

            // https://answers.unity.com/questions/1408415/rotating-a-parent-object-to-achieve-a-specific-chi.html
            //Quaternion targetConnectionRotation = Quaternion.LookRotation(-targetTransform.forward, targetTransform.up);
            //Quaternion lookRotationVar = targetConnectionRotation * Quaternion.Inverse(transform.rotation);
            //lookRotationVar = lookRotationVar * zone.transform.rotation;
            //zone.transform.rotation = lookRotationVar;

            Quaternion targetConnectionRotation = Quaternion.LookRotation(-targetTransform.forward, targetTransform.up);
            parentZoneTransform.rotation = targetConnectionRotation * Quaternion.Inverse(myTransform.rotation) * parentZoneTransform.rotation;

            Vector3 positionOffset = parentZoneTransform.position - myTransform.position;
            parentZoneTransform.position = targetTransform.position + positionOffset;
        }

        public void ConnectAndMoveTarget(ZoneConnection target)
        {
            target.ConnectAndMoveSelf(this);
        }

        public ZoneConnection GetMatchingConnection(Zone zone)
        {
            int index = ParentZone.Connections.IndexOf(this);
            return zone.Connections[index];
        }
    }
}