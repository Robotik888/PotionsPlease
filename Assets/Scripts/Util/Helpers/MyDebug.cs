using UnityEngine;

[System.Diagnostics.DebuggerStepThrough]
public static class MyDebug
{
    public static void DrawCross(Vector3 position, float size) => DrawCross(position, size, Gizmos.color);

    public static void DrawCross(Vector3 position, float size, Color color)
    {
        Debug.DrawLine(position + Vector3.forward * size, position + Vector3.forward * -size, color);
        Debug.DrawLine(position + Vector3.right * size, position + Vector3.right * -size, color);
        Debug.DrawLine(position + Vector3.up * size, position + Vector3.up * -size, color);
    }
}
