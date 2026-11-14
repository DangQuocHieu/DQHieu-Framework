namespace DQHieu.Framework.DesignPattern
{
    using UnityEngine;
    using System.Collections.Generic;

    [System.Serializable]
    public class PoolItemConfig
    {
        public GameObject Prefab;
        public int InitPoolSize;
    }
    
    [CreateAssetMenu(fileName = "ObjectPoolConfig", menuName = "Scriptable Objects/ObjectPoolConfig")]
    public class ObjectPoolConfig : ScriptableObject
    {
        public List<PoolItemConfig> PoolItemsConfigs = new();
    }

}