using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildScript
{
    private static string Env(string key, string fallback)
    {
        string value = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrEmpty(value) ? fallback : value;
    }

    private static void GenerateBuildInfo()
    {
        var info = new BuildInfo
        {
            version = Env("VERSION", "dev"),
            buildNumber = Env("BUILD_NUMBER", "0"),
            commit = Env("COMMIT_SHA", "local"),
            branch = Env("BRANCH_NAME", "local"),
            buildDate = Env("BUILD_DATE", DateTime.UtcNow.ToString("O")),
            unityVersion = Application.unityVersion,
            workflow = Env("WORKFLOW", "Local")
        };

        string commit = Env("COMMIT_SHA", "local");
        info.commit = commit.Length > 7 ? commit.Substring(0, 7) : commit;

        string folder = "Assets/StreamingAssets";

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string json = JsonUtility.ToJson(info, true);

        File.WriteAllText(Path.Combine(folder, "buildinfo.json"), json);

        AssetDatabase.Refresh();

        Debug.Log("Generated buildinfo.json");
    }

    public static void BuildWebGL()
    {
        GenerateBuildInfo();

        const string outputPath = "Build/WebGL";

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
    }
}