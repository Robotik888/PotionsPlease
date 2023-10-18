using UnityEngine;
using UnityEditor;

/// <summary>
/// Adds the corresponding namespace to the newly created script.
/// </summary>
public class AddNameSpace : AssetModificationProcessor
{
    private const string DEFAULT_NAMESPACE = "PotionsPlease";

    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");

        if (index < 0)
            return;

        string fileExtesion = path.Substring(index);

        if (fileExtesion != ".cs")
            return;

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        
        if (!System.IO.File.Exists(path))
            return;

        string fileText = System.IO.File.ReadAllText(path);

        /// File already has a namespace set (it is probably only being renamed)
        if (fileText.Contains($"namespace {DEFAULT_NAMESPACE}."))
            return;

        string lastPart = path.Substring(path.IndexOf("Assets"));
        string correctNamespace = lastPart.Substring(0, lastPart.LastIndexOf('/'));
        correctNamespace = correctNamespace.Replace('/', '.');
        correctNamespace = correctNamespace.Replace("Assets.Scripts", DEFAULT_NAMESPACE);

        fileText = fileText.Replace(DEFAULT_NAMESPACE, correctNamespace);

        System.IO.File.WriteAllText(path, fileText);
        AssetDatabase.Refresh();
    }
}