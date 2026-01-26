#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FarmSim.Editor.Recon
{
    public static class ReconSourceGrepSnapshotGenerator
    {
        // Canonical scope: keep it focused and meaningful.
        private const string ScopeRoot = "Assets/FarmSim/_Application/";

        // Canonical queries (minimum set).
        private static readonly string[] Queries =
        {
            "TickExecSnap",
            "TickExec",
            "ExecutionSnapshot",
            "SimulationTickExecutionSnapshot",
            "SimulationTickAuthority",
            "SimulationStepOutcome",
            "Denied(",
            "Debug.Log",
            "Debug.LogError",
            "Debug.LogWarning"
        };

        [MenuItem("FarmSim/Recon/Generate Source Grep Snapshot (JSON)")]
        public static void Generate()
        {
            try
            {
                // IMPORTANT: fully qualify UnityEngine.Application
                var projectRoot = Directory.GetParent(UnityEngine.Application.dataPath)?.FullName;
                if (string.IsNullOrWhiteSpace(projectRoot))
                {
                    Debug.LogError("[Recon] Could not resolve project root.");
                    return;
                }

                var scopeAbs = Path.Combine(
                    projectRoot,
                    ScopeRoot.Replace('/', Path.DirectorySeparatorChar)
                );

                if (!Directory.Exists(scopeAbs))
                {
                    Debug.LogError($"[Recon] Scope folder not found: {ScopeRoot}");
                    return;
                }

                // Output folder at project root
                var outDirAbs = Path.Combine(projectRoot, "Recon");
                Directory.CreateDirectory(outDirAbs);

                var utc = DateTime.UtcNow;
                var ts = utc.ToString("yyyyMMdd_HHmmss");
                var outFile = Path.Combine(
                    outDirAbs,
                    $"recon_source_grep_snapshot_{ts}.json"
                );

                var csFiles = Directory.GetFiles(
                    scopeAbs,
                    "*.cs",
                    SearchOption.AllDirectories
                );

                var sb = new StringBuilder(1024 * 256);

                sb.Append("{\n");
                AppendJsonKV(sb, "generatedAtUtc", utc.ToString("o"), 1, true);
                AppendJsonKV(sb, "projectRoot", projectRoot.Replace('\\', '/'), 1, true);
                AppendJsonKV(sb, "tool", "UnityEditor (FarmSim Recon)", 1, true);
                AppendJsonKV(sb, "scope", ScopeRoot, 1, true);

                sb.Append("  \"queries\": [\n");

                for (int qi = 0; qi < Queries.Length; qi++)
                {
                    var query = Queries[qi];
                    sb.Append("    {\n");
                    AppendJsonKV(sb, "query", query, 3, true);
                    sb.Append("      \"matches\": [\n");

                    var firstMatch = true;

                    foreach (var absPath in csFiles)
                    {
                        var relPath = MakeProjectRelative(absPath, projectRoot);
                        var lines = File.ReadAllLines(absPath);

                        for (int i = 0; i < lines.Length; i++)
                        {
                            var line = lines[i];
                            if (line == null) continue;

                            if (line.Contains(query, StringComparison.Ordinal))
                            {
                                if (!firstMatch) sb.Append(",\n");
                                firstMatch = false;

                                sb.Append("        { ");
                                sb.Append("\"file\": ").Append(JsonString(relPath)).Append(", ");
                                sb.Append("\"line\": ").Append(i + 1).Append(", ");
                                sb.Append("\"text\": ").Append(JsonString(line));
                                sb.Append(" }");
                            }
                        }
                    }

                    sb.Append("\n      ]\n");
                    sb.Append("    }");
                    if (qi < Queries.Length - 1) sb.Append(",");
                    sb.Append("\n");
                }

                sb.Append("  ]\n");
                sb.Append("}\n");

                File.WriteAllText(outFile, sb.ToString(), Encoding.UTF8);

                AssetDatabase.Refresh();

                Debug.Log($"[Recon] Wrote {outFile.Replace('\\', '/')}");
            }
            catch (Exception ex)
            {
                Debug.LogError("[Recon] Failed: " + ex.GetType().Name + " " + ex.Message);
            }
        }

        private static string MakeProjectRelative(string absPath, string projectRoot)
        {
            var p = absPath.Replace('\\', '/');
            var root = projectRoot.Replace('\\', '/').TrimEnd('/') + "/";
            return p.StartsWith(root, StringComparison.Ordinal)
                ? p.Substring(root.Length)
                : p;
        }

        private static void AppendJsonKV(
            StringBuilder sb,
            string key,
            string value,
            int indent,
            bool trailingComma
        )
        {
            sb.Append(new string(' ', indent * 2));
            sb.Append(JsonString(key));
            sb.Append(": ");
            sb.Append(JsonString(value));
            sb.Append(trailingComma ? ",\n" : "\n");
        }

        private static string JsonString(string s)
        {
            if (s == null) return "null";
            return "\"" + s
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\t", "\\t")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                + "\"";
        }
    }
}
#endif
