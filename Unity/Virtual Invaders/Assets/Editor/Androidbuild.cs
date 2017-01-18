using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

class PerformBuild
{
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;

            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }

    static string GetBuildPathAndroid()
    {
        return "android.apk";
    }

    [UnityEditor.MenuItem("CUSTOM/Test Command Line Build Step Android")]
    static void androidBuild()
    {
        Debug.Log("Command line build android version\n------------------\n------------------");

        string[] scenes = GetBuildScenes();
        string path = GetBuildPathAndroid();
        if (scenes == null || scenes.Length == 0 || path == null)
            return;

        Debug.Log(string.Format("Path: \"{0}\"", path));
        for (int i = 0; i < scenes.Length; ++i)
        {
            Debug.Log(string.Format("Scene[{0}]: \"{1}\"", i, scenes[i]));
        }

        Debug.Log("Starting Android Build!");
        BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
    }
}