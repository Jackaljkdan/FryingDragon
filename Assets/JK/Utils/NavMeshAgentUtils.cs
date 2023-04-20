using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class NavMeshAgentUtils
    {
        private static NavMeshPath path;
        private static Vector3[] cornersBuffer;

        /// <summary>
        /// TODO: genera rusco, piuttosto fai come per l'OvertStalker
        /// </summary>
        public static bool TryGetNextCornerTo(this NavMeshAgent agent, Vector3 position, out Vector3 corner)
        {
            if (path == null)
            {
                path = new NavMeshPath();
                cornersBuffer = new Vector3[4];
            }

            // CalculatePath è quello che genera rusco, perché istanzia ogni volta l'array dei corners
            // si può provare questo: https://github.com/Unity-Technologies/NavMeshComponents
            if (!agent.CalculatePath(position, path))
            {
                Debug.LogError("calculate path failed");
                corner = default;
                return false;
            }

            int count = path.GetCornersNonAlloc(cornersBuffer);

            if (count < 2)
            {
                Debug.LogError("no path corners");
                corner = default;
                return false;
            }

            corner = cornersBuffer[1];
            return true;
        }
    }
}