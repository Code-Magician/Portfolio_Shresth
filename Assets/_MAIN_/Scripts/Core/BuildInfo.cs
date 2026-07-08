using System;
using System.IO;
using UnityEngine;

[Serializable]
public class BuildInfo
{
    public string version;
    public string buildNumber;
    public string commit;
    public string branch;
    public string buildDate;
    public string unityVersion;
    public string workflow;

    public static BuildInfo Load()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "buildinfo.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("buildinfo.json not found.");
            return null;
        }

        return JsonUtility.FromJson<BuildInfo>(File.ReadAllText(path));
    }
}