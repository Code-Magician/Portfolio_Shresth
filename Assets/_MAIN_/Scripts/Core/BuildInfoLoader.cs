using System.IO;
using UnityEngine;

public class BuildInfoLoader : MonoBehaviour
{
    public static BuildInfo Info { get; private set; }

    private void Awake()
    {
        Load();
    }

    public static void Load()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "buildinfo.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"buildinfo.json not found:\n{path}");
            return;
        }

        string json = File.ReadAllText(path);

        Info = JsonUtility.FromJson<BuildInfo>(json);

        Debug.Log("BuildInfo Loaded Successfully");
    }
}