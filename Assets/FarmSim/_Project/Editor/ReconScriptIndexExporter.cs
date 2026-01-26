using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FarmSim.Project.Editor.Recon
{
    public static class ReconScriptIndexExporter
    {
        private const string ReconVersion = "1.0";

        [MenuItem("Tools/Recon/Export Script Index")]
        public static void Export()
        {
            var snapshot = new ScriptIndexSnapshot
            {
                reconVersion = ReconVersion,
                generatedUtc = DateTime.UtcNow.ToString("u"),
                scripts = new List<ScriptEntry>()
            };

            var guids = AssetDatabase.FindAssets("t:MonoScript");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                if (script == null)
                    continue;

                var type = script.GetClass();

                snapshot.scripts.Add(new ScriptEntry
                {
                    name = script.name,
                    path = path,
                    @namespace = type?.Namespace ?? "<none>",
                    className = type?.Name ?? "<unknown>"
                });
            }

            var json = JsonUtility.ToJson(snapshot, true);

            var exportDir = Path.Combine(
                UnityEngine.Application.dataPath,
                "FarmSim/_Project/Recon/Exports"
            );

            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }

            var fileName =
                $"recon_script_index_snapshot_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";

            var fullPathOut = Path.Combine(exportDir, fileName);

            File.WriteAllText(fullPathOut, json);

            Debug.Log($"[Recon] Script index exported to: {fullPathOut}");
            AssetDatabase.Refresh();
        }

        [Serializable]
        private class ScriptIndexSnapshot
        {
            public string reconVersion;
            public string generatedUtc;
            public List<ScriptEntry> scripts;
        }

        [Serializable]
        private class ScriptEntry
        {
            public string name;
            public string path;
            public string @namespace;
            public string className;
        }
    }
}
