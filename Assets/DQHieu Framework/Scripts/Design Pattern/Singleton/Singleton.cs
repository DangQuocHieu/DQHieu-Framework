namespace DQHieu.Framework.DesignPattern
{
    using UnityEngine;
    
    public class Singleton<T>: MonoBehaviour where T : Component
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        SetupInstance();
                    }
                }

                return instance;
            }
        }
        
        protected virtual void Awake()
        {
            RemoveDuplicates();
        }

        private static void SetupInstance()
        {
            instance = FindAnyObjectByType<T>();
            if(instance == null)
            {
                GameObject go = new();
                go.name = typeof(T).Name;   
                instance =  go.AddComponent<T>();
            }
        }

        private void RemoveDuplicates()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}