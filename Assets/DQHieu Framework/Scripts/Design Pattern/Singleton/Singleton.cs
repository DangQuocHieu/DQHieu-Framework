namespace DQHieu.Framework.DesignPattern
{
    using UnityEngine;
    
    public class Singleton<T>: MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        SetupInstance();
                    }
                }

                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            RemoveDuplicates();
        }

        private static void SetupInstance()
        {
            _instance = FindAnyObjectByType<T>();
            if(_instance == null)
            {
                GameObject go = new();
                go.name = typeof(T).Name;   
                _instance =  go.AddComponent<T>();
            }
        }

        private void RemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}