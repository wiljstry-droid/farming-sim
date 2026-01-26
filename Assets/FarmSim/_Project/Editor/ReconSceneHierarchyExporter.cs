using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Project.Editor.Recon
{
    public static class ReconSceneHierarchyExporter
    {
        private const string ReconVersion = "1.0";

        [MenuItem("Tools/Recon/Export Scene Hierarchy")]
        public static void Export()
        {
            var scene = SceneManager.GetActiveScene();

            if (!scene.IsValid() || !scene.isLoaded)
            {
                Debug.LogError("[Recon] Active scene is not valid or not loaded.");
                return;
            }

            var snapshot = new SceneHierarchySnapshot
            {
                reconVersion = ReconVersion,
                generatedUtc = DateTime.UtcNow.ToString("u"),
                sceneName = scene.name,
                scenePath = scene.path,
                roots = new List<GameObjectNode>()
            };

            var rootObjects = scene.GetRootGameObjects();
            foreach (var go in rootObjects)
            {
                snapshot.roots.Add(BuildNode(go));
            }

            var exportDir = Path.Combine(
                UnityEngine.Application.dataPath,
                "FarmSim/_Project/Recon/Exports"
            );

            if (!Directory.Exists(exportDir))
                Directory.CreateDirectory(exportDir);

            var fileName = $"recon_scene_hierarchy_snapshot_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            var fullPathOut = Path.Combine(exportDir, fileName);

            // IMPORTANT: Avoid UnityEngine.JsonUtility depth limit by writing JSON manually.
            var json = WriteSnapshotJson(snapshot);
            File.WriteAllText(fullPathOut, json, Encoding.UTF8);

            Debug.Log($"[Recon] Scene hierarchy exported to: {fullPathOut}");
            AssetDatabase.Refresh();
        }

        private static GameObjectNode BuildNode(GameObject go)
        {
            var node = new GameObjectNode
            {
                name = go.name,
                isActiveSelf = go.activeSelf,
                components = new List<string>(),
                children = new List<GameObjectNode>()
            };

            try
            {
                var comps = go.GetComponents<Component>();
                for (int i = 0; i < comps.Length; i++)
                {
                    var c = comps[i];
                    node.components.Add(c == null ? "<MissingScript>" : c.GetType().FullName);
                }
            }
            catch
            {
                node.components.Add("<ComponentReadError>");
            }

            var t = go.transform;
            for (int i = 0; i < t.childCount; i++)
            {
                node.children.Add(BuildNode(t.GetChild(i).gameObject));
            }

            return node;
        }

        // -------------------------
        // Manual JSON writer (no depth limit)
        // -------------------------

        private static string WriteSnapshotJson(SceneHierarchySnapshot s)
        {
            var sb = new StringBuilder(1024 * 64);

            sb.Append("{\n");
            WriteProp(sb, "reconVersion", s.reconVersion, 1); sb.Append(",\n");
            WriteProp(sb, "generatedUtc", s.generatedUtc, 1); sb.Append(",\n");
            WriteProp(sb, "sceneName", s.sceneName, 1); sb.Append(",\n");
            WriteProp(sb, "scenePath", s.scenePath, 1); sb.Append(",\n");

            Indent(sb, 1); sb.Append("\"roots\": [\n");
            for (int i = 0; i < s.roots.Count; i++)
            {
                WriteNode(sb, s.roots[i], 2);
                if (i < s.roots.Count - 1) sb.Append(",\n");
                else sb.Append("\n");
            }
            Indent(sb, 1); sb.Append("]\n");
            sb.Append("}\n");

            return sb.ToString();
        }

        private static void WriteNode(StringBuilder sb, GameObjectNode n, int indent)
        {
            Indent(sb, indent); sb.Append("{\n");

            WriteProp(sb, "name", n.name, indent + 1); sb.Append(",\n");
            Indent(sb, indent + 1); sb.Append("\"isActiveSelf\": ").Append(n.isActiveSelf ? "true" : "false").Append(",\n");

            // components
            Indent(sb, indent + 1); sb.Append("\"components\": [");
            if (n.components != null && n.components.Count > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < n.components.Count; i++)
                {
                    Indent(sb, indent + 2);
                    sb.Append("\"").Append(Escape(n.components[i] ?? "")).Append("\"");
                    if (i < n.components.Count - 1) sb.Append(",\n");
                    else sb.Append("\n");
                }
                Indent(sb, indent + 1); sb.Append("]");
            }
            else
            {
                sb.Append("]");
            }
            sb.Append(",\n");

            // children
            Indent(sb, indent + 1); sb.Append("\"children\": [");
            if (n.children != null && n.children.Count > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < n.children.Count; i++)
                {
                    WriteNode(sb, n.children[i], indent + 2);
                    if (i < n.children.Count - 1) sb.Append(",\n");
                    else sb.Append("\n");
                }
                Indent(sb, indent + 1); sb.Append("]");
            }
            else
            {
                sb.Append("]");
            }
            sb.Append("\n");

            Indent(sb, indent); sb.Append("}");
        }

        private static void WriteProp(StringBuilder sb, string key, string value, int indent)
        {
            Indent(sb, indent);
            sb.Append("\"").Append(Escape(key)).Append("\": ");
            sb.Append("\"").Append(Escape(value ?? "")).Append("\"");
        }

        private static void Indent(StringBuilder sb, int n)
        {
            for (int i = 0; i < n; i++) sb.Append("  ");
        }

        private static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }

        [Serializable]
        private class SceneHierarchySnapshot
        {
            public string reconVersion;
            public string generatedUtc;
            public string sceneName;
            public string scenePath;
            public List<GameObjectNode> roots;
        }

        [Serializable]
        private class GameObjectNode
        {
            public string name;
            public bool isActiveSelf;
            public List<string> components;
            public List<GameObjectNode> children;
        }
    }
}
