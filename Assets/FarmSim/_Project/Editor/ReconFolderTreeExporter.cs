using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FarmSim.Project.Editor.Recon
{
    public static class ReconFolderTreeExporter
    {
        private const string ReconVersion = "1.0";

        [MenuItem("Tools/Recon/Export Folder Tree")]
        public static void Export()
        {
            var snapshot = new FolderTreeSnapshot
            {
                reconVersion = ReconVersion,
                generatedUtc = DateTime.UtcNow.ToString("u"),
                root = BuildFolderNode("Assets")
            };

            var exportDir = Path.Combine(
                UnityEngine.Application.dataPath,
                "FarmSim/_Project/Recon/Exports"
            );

            if (!Directory.Exists(exportDir))
                Directory.CreateDirectory(exportDir);

            var fileName = $"recon_folder_tree_snapshot_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            var fullPathOut = Path.Combine(exportDir, fileName);

            // IMPORTANT: Avoid UnityEngine.JsonUtility depth limit by writing JSON manually.
            var json = WriteSnapshotJson(snapshot);
            File.WriteAllText(fullPathOut, json, Encoding.UTF8);

            Debug.Log($"[Recon] Folder tree exported to: {fullPathOut}");
            AssetDatabase.Refresh();
        }

        private static FolderNode BuildFolderNode(string assetFolderPath)
        {
            var node = new FolderNode
            {
                name = Path.GetFileName(assetFolderPath),
                children = new List<FolderNode>()
            };

            var subfolders = AssetDatabase.GetSubFolders(assetFolderPath);
            foreach (var sub in subfolders)
            {
                node.children.Add(BuildFolderNode(sub));
            }

            return node;
        }

        // -------------------------
        // Manual JSON writer (no depth limit)
        // -------------------------

        private static string WriteSnapshotJson(FolderTreeSnapshot s)
        {
            var sb = new StringBuilder(1024 * 32);

            sb.Append("{\n");
            WriteProp(sb, "reconVersion", s.reconVersion, 1); sb.Append(",\n");
            WriteProp(sb, "generatedUtc", s.generatedUtc, 1); sb.Append(",\n");
            Indent(sb, 1); sb.Append("\"root\": ");
            WriteFolderNode(sb, s.root, 1);
            sb.Append("\n}\n");

            return sb.ToString();
        }

        private static void WriteFolderNode(StringBuilder sb, FolderNode n, int indent)
        {
            sb.Append("{\n");
            WriteProp(sb, "name", n.name, indent + 1); sb.Append(",\n");

            Indent(sb, indent + 1); sb.Append("\"children\": [");
            if (n.children != null && n.children.Count > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < n.children.Count; i++)
                {
                    Indent(sb, indent + 2);
                    WriteFolderNode(sb, n.children[i], indent + 2);
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
        private class FolderTreeSnapshot
        {
            public string reconVersion;
            public string generatedUtc;
            public FolderNode root;
        }

        [Serializable]
        private class FolderNode
        {
            public string name;
            public List<FolderNode> children;
        }
    }
}
