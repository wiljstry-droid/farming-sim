#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace FarmSim.Project.Editor
{
    public static class ReconScriptIndexExporter
    {
        private const string MenuPath = "FarmSim/Recon/Export Script Index";

        [MenuItem(MenuPath)]
        public static void Export()
        {
            try
            {
                string projectRoot = GetProjectRootAbsolute();
                string outputDir = EnsureReconDocsFolder(projectRoot);

                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                string fileName = $"recon_script_index_snapshot_{timestamp}.json";
                string outputPath = Path.Combine(outputDir, fileName);

                ScriptIndexSnapshot snapshot = BuildSnapshot(projectRoot);
                string json = JsonUtility.ToJson(snapshot, true);

                File.WriteAllText(outputPath, json);
                Debug.Log($"[Recon] Script index snapshot written: {outputPath}");

                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Recon] Script index export failed: {ex}");
            }
        }

        private static ScriptIndexSnapshot BuildSnapshot(string projectRootAbs)
        {
            var snap = new ScriptIndexSnapshot
            {
                schemaVersion = 1,
                generatedUtc = DateTime.UtcNow.ToString("O"),
                projectRoot = projectRootAbs,
                scripts = new List<ScriptRecord>()
            };

            // Scan only Assets for scripts.
            string assetsAbs = UnityEngine.Application.dataPath;

            foreach (string file in Directory.EnumerateFiles(assetsAbs, "*.cs", SearchOption.AllDirectories))
            {
                string rel = MakeRelativePath(projectRootAbs, file).Replace('\\', '/');

                string text;
                try { text = File.ReadAllText(file); }
                catch
                {
                    snap.scripts.Add(new ScriptRecord
                    {
                        path = rel,
                        namespaces = Array.Empty<string>(),
                        types = Array.Empty<string>()
                    });
                    continue;
                }

                string[] namespaces = ExtractNamespaces(text);
                string[] types = ExtractTypes(text);

                snap.scripts.Add(new ScriptRecord
                {
                    path = rel,
                    namespaces = namespaces,
                    types = types
                });
            }

            return snap;
        }

        private static string[] ExtractNamespaces(string source)
        {
            var matches = Regex.Matches(
                source,
                @"^\s*namespace\s+([A-Za-z_][A-Za-z0-9_.]*)\s*",
                RegexOptions.Multiline);

            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (Match m in matches)
            {
                if (m.Success && m.Groups.Count > 1)
                    set.Add(m.Groups[1].Value.Trim());
            }

            var list = new List<string>(set);
            list.Sort(StringComparer.Ordinal);
            return list.ToArray();
        }

        private static string[] ExtractTypes(string source)
        {
            // Lightweight indexing: list any class/struct/interface/enum declarations.
            var matches = Regex.Matches(
                source,
                @"^\s*(?:public|internal|protected|private|static|sealed|abstract|partial|\s)*\s*(class|struct|interface|enum)\s+([A-Za-z_][A-Za-z0-9_]*)\b",
                RegexOptions.Multiline);

            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (Match m in matches)
            {
                if (!m.Success || m.Groups.Count < 3) continue;
                string kind = m.Groups[1].Value.Trim();
                string name = m.Groups[2].Value.Trim();
                set.Add($"{kind} {name}");
            }

            var list = new List<string>(set);
            list.Sort(StringComparer.Ordinal);
            return list.ToArray();
        }

        private static string GetProjectRootAbsolute()
        {
            // IMPORTANT: fully qualify UnityEngine.Application to avoid collisions.
            string assetsPath = UnityEngine.Application.dataPath; // <root>/Assets
            DirectoryInfo? parent = Directory.GetParent(assetsPath);
            if (parent == null)
                throw new InvalidOperationException("Unable to resolve project root from UnityEngine.Application.dataPath.");

            return parent.FullName;
        }

        private static string EnsureReconDocsFolder(string projectRootAbs)
        {
            string outputDir = Path.Combine(projectRootAbs, "Recon Docs");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            return outputDir;
        }

        private static string MakeRelativePath(string rootAbs, string fullAbs)
        {
            Uri root = new Uri(EnsureTrailingSlash(rootAbs));
            Uri full = new Uri(fullAbs);
            return Uri.UnescapeDataString(root.MakeRelativeUri(full).ToString());
        }

        private static string EnsureTrailingSlash(string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)) return path;
            if (path.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.Ordinal)) return path;
            return path + Path.DirectorySeparatorChar;
        }

        [Serializable]
        private class ScriptIndexSnapshot
        {
            public int schemaVersion;
            public string generatedUtc = "";
            public string projectRoot = "";
            public List<ScriptRecord> scripts = new List<ScriptRecord>();
        }

        [Serializable]
        private class ScriptRecord
        {
            public string path = "";
            public string[] namespaces = Array.Empty<string>();
            public string[] types = Array.Empty<string>();
        }
    }
}
