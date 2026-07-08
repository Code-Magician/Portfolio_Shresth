using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class BuildInfoLoader : MonoBehaviour
{
    private static BuildInfoLoader Instance;
    public static BuildInfo Info { get; private set; }

    public static bool IsLoaded { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "buildinfo.json");

#if UNITY_WEBGL && !UNITY_EDITOR

        using UnityWebRequest request = UnityWebRequest.Get(path);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to load BuildInfo\n{request.error}");
            IsLoaded = true;
            yield break;
        }

        Info = JsonUtility.FromJson<BuildInfo>(request.downloadHandler.text);

#else

        if (!File.Exists(path))
        {
            Debug.LogError($"buildinfo.json not found\n{path}");
            IsLoaded = true;
            yield break;
        }

        Info = JsonUtility.FromJson<BuildInfo>(File.ReadAllText(path));

#endif

        IsLoaded = true;

        Debug.Log("BuildInfo Loaded");
        Debug.Log(JsonUtility.ToJson(Info, true));
    }
}