using System.Collections.Generic;

namespace DQHieu.Framework.DesignPattern
{
    using UnityEngine;

    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        private Dictionary<GameObject, Stack<Component>> _pools = new();
        private Dictionary<GameObject, GameObject> _instanceToPrefab = new();
        private Dictionary<GameObject, Transform> _poolContainers = new();
        [SerializeField] private ObjectPoolConfig _objectPoolConfig;

        public void InitializePools()
        {
            if (_objectPoolConfig == null)
            {
                Debug.Log("Chưa gắn asset Object Pool Config!");
                return;
            }

            foreach (var poolItemConfig in _objectPoolConfig.PoolItemsConfigs)
            {
                if (poolItemConfig.Prefab == null) continue;
                Prewarm(poolItemConfig.Prefab, poolItemConfig.InitPoolSize);
            }
        }

        private Transform GetOrCreateContainer(GameObject prefab)
        {
            if (!_poolContainers.TryGetValue(prefab, out Transform container))
            {
                container = new GameObject($"{prefab.name} Container").transform;
                container.SetParent(transform, false);
                _poolContainers.Add(prefab, container);
            }
            return container;
        }

        private void Prewarm(GameObject prefab, int initPoolSize)
        {
            if (!_pools.TryGetValue(prefab, out Stack<Component> pool))
            {
                pool = new Stack<Component>(initPoolSize);
                _pools.Add(prefab, pool);
            }

            Transform container = GetOrCreateContainer(prefab);

            for (int i = 0; i < initPoolSize; i++)
            {
                GameObject obj = Object.Instantiate(prefab, container);
                obj.SetActive(false);
                _instanceToPrefab.Add(obj, prefab);
                pool.Push(obj.transform);
            }
        }

        public T Get<T>(T prefab) where T : Component
        {
            GameObject prefabObj = prefab.gameObject;
            T instance = null;

            if (!_pools.TryGetValue(prefabObj, out Stack<Component> pool))
            {
                pool = new Stack<Component>();
                _pools.Add(prefabObj, pool);
            }

            Transform container = GetOrCreateContainer(prefabObj);

            if (pool.Count > 0)
            {
                Component comp = pool.Pop();
                if (comp is not T targetInstance)
                {
                    targetInstance = comp.GetComponent<T>();
                }
                instance = targetInstance;
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Object.Instantiate(prefab, container);
                _instanceToPrefab.Add(instance.gameObject, prefabObj);
            }
            return instance;
        }

        public T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            T instance = Get(prefab);
            instance.transform.SetPositionAndRotation(position, rotation);
            if (parent != null && instance.transform.parent != parent)
            {
                instance.transform.SetParent(parent);
            }
            return instance;
        }

        public void Release<T>(T instance) where T : Component
        {
            if (instance == null) return;

            GameObject obj = instance.gameObject;
            if (!obj.activeSelf)
            {
                Debug.Log($"{obj.name} đã được trả về pool!");
                return;
            }

            if (_instanceToPrefab.TryGetValue(obj, out GameObject prefabObj))
            {
                obj.SetActive(false);
                if (_poolContainers.TryGetValue(prefabObj, out Transform container) && instance.transform.parent != container)
                {
                    instance.transform.SetParent(container, false);
                }
                _pools[prefabObj].Push(instance);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        public GameObject Get(GameObject prefab)
        {
            return Get<Transform>(prefab.transform).gameObject;
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Get<Transform>(prefab.transform, position, rotation, parent).gameObject;
        }

        public void Release(GameObject instance)
        {
            if (instance == null) return;
            Release<Transform>(instance.transform);
        }
    }
}