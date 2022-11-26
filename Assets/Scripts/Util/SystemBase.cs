using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Util.Helpers
{
    [System.Diagnostics.DebuggerStepThrough]
    public abstract class SystemBase<T> : ManagerBase<T> where T : SystemBase<T>
    {
        protected override void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            base.Awake();

            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
}