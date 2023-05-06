using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class MoneySpawner : MonoBehaviour
    {
        #region Inspector

        public GameObject moneyPrefab;

        public float minSpawnDelaySeconds = 0.1f;
        public float maxSpawnDelaySeconds = 0.5f;

        public float minSpawnDistance = 0.1f;
        public float maxSpawnDistance = 0.5f;

        #endregion

        private float nextSpawnTime;


        private void Update()
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnMoney();
                nextSpawnTime = Time.time + Random.Range(minSpawnDelaySeconds, maxSpawnDelaySeconds);
            }
        }

        private void SpawnMoney()
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(minSpawnDistance, maxSpawnDistance), transform.position.y, transform.position.z + Random.Range(minSpawnDistance, maxSpawnDistance));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            GameObject money = Instantiate(moneyPrefab, spawnPosition, spawnRotation);
            money.transform.SetParent(transform.root);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxSpawnDistance);
        }
    }
}