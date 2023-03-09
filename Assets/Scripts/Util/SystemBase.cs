using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Util
{
    //[System.Diagnostics.DebuggerStepThrough]
    public abstract class SystemBase<T> : MonoBehaviour where T : SystemBase<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
        
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = transform.GetComponent<T>();
        }
    }
}