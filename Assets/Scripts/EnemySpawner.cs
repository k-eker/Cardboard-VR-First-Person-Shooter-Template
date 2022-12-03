using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UNIT9
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The closest distance to spawn enemies.")]
        [Range(10, 25)]
        private float m_InnerRadius = 19;
        [Tooltip("The furthest distance to spawn enemies.")]
        [Range(10, 25)]
        private float m_OuterRadius = 23;

        [Tooltip("Minimum time it takes to spawn a new enemy.")]
        [Range(0, 5f)]
        private float m_MinSpawnDelay = 0.2f;
        [Tooltip("Maximum time it takes to spawn a new enemy.")]
        [Range(0, 5f)]
        private float m_MaxSpawnDelay = 5f;

        [Header("References")]
        [SerializeField] private GameObject m_EnemyPrefab;


        private void Start()
        {
            // start spawning enemies
            StartCoroutine(Spawning());
        }

        IEnumerator Spawning()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(m_MinSpawnDelay, m_MaxSpawnDelay));

                InstantiateEnemy(GetRandomPositionInDonut(Vector3.zero));
            }
        }

        private void InstantiateEnemy(Vector3 pos)
        {
            Instantiate(m_EnemyPrefab, pos, Quaternion.identity);
        }


        // HOWTO: get a random position in a donut shape?
        // https://answers.unity.com/questions/1580130/i-need-to-instantiate-an-object-inside-a-donut-ins.html
        private Vector3 GetRandomPositionInDonut(Vector3 center)
        {
            float angle = UnityEngine.Random.Range(0, 2f * (float)Math.PI);
            var dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 point = dir * UnityEngine.Random.Range(m_InnerRadius, m_OuterRadius);

            return point;
        }
    }

}
