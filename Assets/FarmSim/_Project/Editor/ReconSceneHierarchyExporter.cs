using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Project.Editor
{
    public static class ReconSceneHierarchyExporter
    {
        private const int SchemaVersion = 1;

        public static void Export()
        {
            Scene scene = EditorSceneManager.GetActiveScene();

            // IMPORTANT: Fully qualify UnityEngine.Application to avoid collision with FarmSim.Application namespace.
            string projectRoot = Directory.GetParent(UnityEngine.Application.dataPath)?.FullName;
            if (string.IsNullOrWhiteSpace(projectRoot))
            {
                UnityEngine.Debug.LogError("ReconSceneHierarchyExporter: Could not resolve project root from UnityEngine.Application.dataPath.");
                return;
            }

            string reconDir = Path.Combine(projectRoot, "Recon");
            Directory.CreateDirectory(reconDir);

            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            string fileName = $"recon_scene_hierarchy_snapshot_{timestamp}.json";
            string fullPath = Path.Combine(reconDir, fileName);

            var sb = new StringBuilder(1024 * 1024);

            sb.Append("{\n");
            sb.Append("  \"schemaVersion\": ").Append(SchemaVersion).Append(",\n");
            sb.Append("  \"generatedUtc\": \"").Append(JsonEscape(DateTime.UtcNow.ToString("o"))).Append("\",\n");
            sb.Append("  \"activeSceneName\": \"").Append(JsonEscape(scene.name)).Append("\",\n");
            sb.Append("  \"activeScenePath\": \"").Append(JsonEscape(scene.path)).Append("\",\n");
            sb.Append("  \"roots\": [\n");

            GameObject[] roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                GameObject root = roots[i];
                WriteNode(sb, root, "/" + root.name, 2);

                if (i < roots.Length - 1) sb.Append(",\n");
                else sb.Append("\n");
            }

            sb.Append("  ]\n");
            sb.Append("}\n");

            File.WriteAllText(fullPath, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            AssetDatabase.Refresh();

            UnityEngine.Debug.Log($"ReconSceneHierarchyExporter: Wrote {fileName}");
        }

        private static void WriteNode(StringBuilder sb, GameObject go, string path, int indentLevel)
        {
            string indent = Indent(indentLevel);
            string indentInner = Indent(indentLevel + 1);

            sb.Append(indent).Append("{\n");
            sb.Append(indentInner).Append("\"name\": \"").Append(JsonEscape(go.name)).Append("\",\n");
            sb.Append(indentInner).Append("\"path\": \"").Append(JsonEscape(path)).Append("\",\n");
            sb.Append(indentInner).Append("\"isActive\": ").Append(go.activeSelf ? "true" : "false").Append(",\n");

            // Components
            sb.Append(indentInner).Append("\"components\": [");
            var comps = go.GetComponents<Component>();
            if (comps != null && comps.Length > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < comps.Length; i++)
                {
                    Component c = comps[i];
                    string typeName = c == null ? "<Missing Script>" : c.GetType().FullName;

                    sb.Append(indentInner).Append("  \"").Append(JsonEscape(typeName)).Append("\"");
                    if (i < comps.Length - 1) sb.Append(",\n");
                    else sb.Append("\n");
                }
                sb.Append(indentInner).Append("],\n");
            }
            else
            {
                sb.Append("],\n");
            }

            // Children
            sb.Append(indentInner).Append("\"children\": [");
            int childCount = go.transform.childCount;
            if (childCount > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = go.transform.GetChild(i);
                    GameObject childGo = child.gameObject;

                    string childPath = path + "/" + childGo.name;
                    WriteNode(sb, childGo, childPath, indentLevel + 2);

                    if (i < childCount - 1) sb.Append(",\n");
                    else sb.Append("\n");
                }
                sb.Append(indentInner).Append("]\n");
            }
            else
            {
                sb.Append("]\n");
            }

            sb.Append(indent).Append("}");
        }

        private static string Indent(int level)
        {
            return new string(' ', level * 2);
        }

        private static string JsonEscape(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            var sb = new StringBuilder(s.Length + 16);
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                switch (ch)
                {
                    case '\"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (ch < 0x20)
                            sb.Append("\\u").Append(((int)ch).ToString("x4"));
                        else
                            sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
