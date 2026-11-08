using System;
using DQHieu.Framework.DesignPattern;
using UnityEngine.Pool;

namespace DQHieu.Framework.Sample
{
    using UnityEngine;
    using System.Collections.Generic;
    public class ObjectPoolSampleManager : MonoBehaviour
    {
        [SerializeField] private SamplePooledObject samplePooledObjectPrefab;
        [SerializeField] private int instantiateCount;
        [SerializeField] private float spawnRadius = 10f;

        private void Start()
        {
            ObjectPoolManager.Instance.InitializePools();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < instantiateCount; i++)
                {
                   var ins = ObjectPoolManager.Instance.Get(samplePooledObjectPrefab, GetRandomSpawnPos(),
                        Quaternion.identity);
                   ins.Init();
                }
            }

        }

        public Vector3 GetRandomSpawnPos()
        {
            return Random.insideUnitSphere * spawnRadius;
        }
    }

}