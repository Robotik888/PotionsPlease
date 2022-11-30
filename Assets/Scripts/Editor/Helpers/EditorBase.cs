using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditorBase<T> : Editor where T : Object
{
    protected new T target;
    protected new T[] targets;

    private void Awake()
    {
        target = (T)base.target;
        targets = base.targets.Cast<T>().ToArray();
    }
}
