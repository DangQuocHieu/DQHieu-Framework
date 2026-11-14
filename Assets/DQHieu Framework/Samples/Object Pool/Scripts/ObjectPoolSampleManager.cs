using System;
using DQHieu.Framework.DesignPattern;
using UnityEngine.Pool;

namespace DQHieu.Framework.Sample
{
    using UnityEngine;
    using System.Collections.Generic;
    public class ObjectPoolSampleManager : MonoBehaviour
    {
        [SerializeField] private SamplePooledObject _samplePooledObjectPrefab;
        [SerializeField] private int _instantiateCount;
        [SerializeField] private float _spawnRadius = 10f;

        private void Start()
        {
            ObjectPoolManager.Instance.InitializePools();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < _instantiateCount; i++)
                {
                   var ins = ObjectPoolManager.Instance.Get(_samplePooledObjectPrefab, GetRandomSpawnPos(),
                        Quaternion.identity);
                   ins.Init();
                }
            }

        }

        public Vector3 GetRandomSpawnPos()
        {
            return Random.insideUnitSphere * _spawnRadius;
        }
    }

}