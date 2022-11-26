using UnityEngine;

[System.Diagnostics.DebuggerStepThrough]
public static class Static
{
    public static Vector3 SetX(this Vector3 sender, float value) => new Vector3(value, sender.y, sender.z);
    public static Vector3 SetY(this Vector3 sender, float value) => new Vector3(sender.x, value, sender.z);
    public static Vector3 SetZ(this Vector3 sender, float value) => new Vector3(sender.x, sender.y, value);
    public static Vector3 SetXYZ(this Vector3 sender, float? x, float? y, float? z) => new Vector3(x ?? sender.x, y ?? sender.y, z ?? sender.z);
    public static Color SetA(this Color sender, float a) => new Color(sender.r, sender.g, sender.b, a);

    public static void CopyValuesFrom(this Transform sender, Component copyComponent)
    {
        sender.position = copyComponent.transform.position;
        sender.rotation = copyComponent.transform.rotation;
        sender.localScale = copyComponent.transform.localScale;
    }

    public static float GetAngle2D(Vector3 from, Vector3 to)
    {
        var targetDir = to - from;
        var angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        return angle;
    }

    public static T GetInstanceProperty<T>(ref T instanceVariable) where T : MonoBehaviour
    {
#if UNITY_EDITOR
        if (instanceVariable == null)
            instanceVariable = Object.FindObjectOfType<T>();
#endif
        return instanceVariable;
    }

    public static T GetComponentByNameProperty<T>(ref T componentVariable, string gameObjectName) where T : Component
    {
#if UNITY_EDITOR
        if (componentVariable == null)
            componentVariable = GameObject.Find(gameObjectName).GetComponent<T>();
#endif
        return componentVariable;
    }

}
