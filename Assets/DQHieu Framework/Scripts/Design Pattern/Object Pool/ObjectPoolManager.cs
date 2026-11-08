using System.Collections.Generic;

namespace DQHieu.Framework.DesignPattern
{
    using UnityEngine;

    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        private Dictionary<GameObject, Stack<Component>> pools = new();
        private Dictionary<GameObject, GameObject> instanceToPrefab = new();
        private Dictionary<GameObject, Transform> poolContainers = new();
        [SerializeField] private ObjectPoolConfig objectPoolConfig;

        public void InitializePools()
        {
            if (objectPoolConfig == null)
            {
                Debug.Log("Chưa gắn asset Object Pool Config!");
                return;
            }

            foreach (var poolItemConfig in objectPoolConfig.poolItemsConfigs)
            {
                if (poolItemConfig.prefab == null) continue;
                Prewarm(poolItemConfig.prefab, poolItemConfig.initPoolSize);
            }
        }

        private Transform GetOrCreateContainer(GameObject prefab)
        {
            if (!poolContainers.TryGetValue(prefab, out Transform container))
            {
                container = new GameObject($"{prefab.name} Container").transform;
                container.SetParent(transform, false);
                poolContainers.Add(prefab, container);
            }
            return container;
        }

        private void Prewarm(GameObject prefab, int initPoolSize)
        {
            if (!pools.TryGetValue(prefab, out Stack<Component> pool))
            {
                pool = new Stack<Component>(initPoolSize);
                pools.Add(prefab, pool);
            }

            Transform container = GetOrCreateContainer(prefab);

            for (int i = 0; i < initPoolSize; i++)
            {
                GameObject obj = Object.Instantiate(prefab, container);
                obj.SetActive(false);
                instanceToPrefab.Add(obj, prefab);
                pool.Push(obj.transform);
            }
        }

        public T Get<T>(T prefab) where T : Component
        {
            GameObject prefabObj = prefab.gameObject;
            T instance = null;

            if (!pools.TryGetValue(prefabObj, out Stack<Component> pool))
            {
                pool = new Stack<Component>();
                pools.Add(prefabObj, pool);
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
                instanceToPrefab.Add(instance.gameObject, prefabObj);
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

            if (instanceToPrefab.TryGetValue(obj, out GameObject prefabObj))
            {
                obj.SetActive(false);
                if (poolContainers.TryGetValue(prefabObj, out Transform container) && instance.transform.parent != container)
                {
                    instance.transform.SetParent(container, false);
                }
                pools[prefabObj].Push(instance);
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