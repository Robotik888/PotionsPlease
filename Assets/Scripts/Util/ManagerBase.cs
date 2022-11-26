using UnityEngine;

namespace PotionsPlease.Util
{
    [System.Diagnostics.DebuggerStepThrough]
    public abstract class ManagerBase<T> : MonoBehaviour where T : ManagerBase<T>
    {
#if UNITY_EDITOR
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>(true);
                }

                return _instance;
            }
        }

        private static T _instance;

        protected virtual void Awake()
        {
            _instance = transform.GetComponent<T>();
        }
#else
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = transform.GetComponent<T>();
        }
#endif
    }
}