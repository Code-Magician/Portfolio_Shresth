using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildMetadataGenerator
{
    private static string GetEnv(string key, string fallback)
    {
        string value = Environment.GetEnvironmentVariable(key);

        Debug.Log($"{key} = {(string.IsNullOrEmpty(value) ? "<NULL>" : value)}");

        return string.IsNullOrEmpty(value) ? fallback : value;
    }

    public static void Generate()
    {
        Debug.Log("========== BUILD METADATA ==========");

        BuildInfo buildInfo = new BuildInfo
        {
            version = GetEnv("VERSION", "dev"),
            buildNumber = GetEnv("BUILD_NUMBER", "0"),
            commit = ShortCommit(GetEnv("COMMIT_SHA", "local")),
            branch = GetEnv("BRANCH_NAME", "local"),
            buildDate = GetEnv("BUILD_DATE", DateTime.UtcNow.ToString("O")),
            unityVersion = Application.unityVersion,
            workflow = GetEnv("WORKFLOW", "Local"),

            buildConfiguration = "Release",
            targetPlatform = "WebGL",
            buildMachine = Environment.MachineName
        };

        const string folder = "Assets/StreamingAssets";

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, "buildinfo.json");

        File.WriteAllText(path, JsonUtility.ToJson(buildInfo, true));

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("========== GENERATED JSON ==========");
        Debug.Log(File.ReadAllText(path));
    }

    private static string ShortCommit(string commit)
    {
        if (string.IsNullOrEmpty(commit))
            return "local";

        return commit.Length > 7 ? commit[..7] : commit;
    }
}