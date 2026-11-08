namespace DQHieu.Framework.DesignPattern
{
    using UnityEngine;
    using System.Collections.Generic;

    [System.Serializable]
    public class PoolItemConfig
    {
        public GameObject prefab;
        public int initPoolSize;
    }
    
    [CreateAssetMenu(fileName = "ObjectPoolConfig", menuName = "Scriptable Objects/ObjectPoolConfig")]
    public class ObjectPoolConfig : ScriptableObject
    {
        public List<PoolItemConfig> poolItemsConfigs = new();
    }

}