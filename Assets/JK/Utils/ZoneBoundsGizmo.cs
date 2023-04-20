using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class ZoneBoundsGizmo : MonoBehaviour
    {
        #region Inspector

        public float cellSize = 3f;
        public int cols = 20;
        public int rows = 20;
        public Color gridColor = Color.green;
        public Vector3 offset = new Vector3(-1.5f, 0, -1.5f);

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gridColor;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float x = col * cellSize + offset.x;
                    float z = row * cellSize + offset.z;

                    Vector3 startPoint = transform.position + new Vector3(x, 0f, z);
                    Vector3 endPoint = startPoint + new Vector3(cellSize, 0f, 0f);
                    Gizmos.DrawLine(startPoint, endPoint);

                    endPoint = startPoint + new Vector3(0f, 0f, cellSize);
                    Gizmos.DrawLine(startPoint, endPoint);
                }
            }
            // Draw last row
            Vector3 lastRowStart = transform.position + new Vector3(offset.x, 0f, rows * cellSize + offset.z);
            Vector3 lastRowEnd = lastRowStart + new Vector3(cols * cellSize, 0f, 0f);
            Gizmos.DrawLine(lastRowStart, lastRowEnd);

            // Draw last column
            Vector3 lastColStart = transform.position + new Vector3(cols * cellSize + offset.x, 0f, offset.z);
            Vector3 lastColEnd = lastColStart + new Vector3(0f, 0f, rows * cellSize);
            Gizmos.DrawLine(lastColStart, lastColEnd);

        }
    }
}