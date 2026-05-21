using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;
using UnityEngine;

public static class BuildValidator
{
    // Called by pre-push hook: -executeMethod BuildValidator.Validate
    // Checks compilation only — fast (~30s).
    public static void Validate()
    {
        if (EditorUtility.scriptCompilationFailed)
        {
            Debug.LogError("[BuildValidator] Compilation failed. Fix errors before pushing.");
            EditorApplication.Exit(1);
            return;
        }

        Debug.Log("[BuildValidator] Compilation OK.");
        EditorApplication.Exit(0);
    }

    // Optional: full Windows build — slow (~5min). Swap in pre-push hook if needed.
    public static void Build()
    {
        if (EditorUtility.scriptCompilationFailed)
        {
            Debug.LogError("[BuildValidator] Compilation failed.");
            EditorApplication.Exit(1);
            return;
        }

        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/ValidationBuild/game.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"[BuildValidator] Build succeeded in {report.summary.totalTime.TotalSeconds:F1}s.");
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError($"[BuildValidator] Build failed with {report.summary.totalErrors} error(s).");
            EditorApplication.Exit(1);
        }
    }
}
