using UnityEngine;

namespace peroth
{
    [DisallowMultipleComponent]
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static volatile T _instance;
        private static readonly object _syncRoot = new object();
        public bool dontDestroyOnLoad;

        public static T instance
        {
            get
            {
                Initialize();
                return _instance;
            }
        }

        public static bool isInitialized => _instance != null;

        protected virtual void Awake()
        {
            if (_instance != null) Debug.LogError(GetType().Name + " Singleton class is already created.");

            if (dontDestroyOnLoad) DontDestroyOnLoad(this);

            OnAwake();
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        public static void Initialize()
        {
            if (_instance != null) return;
            lock (_syncRoot)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).FullName);
                    _instance = go.AddComponent<T>();
                }
            }
        }

        protected virtual void OnAwake()
        {
        }
    }
}