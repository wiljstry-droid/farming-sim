#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FarmSim.Project.Editor
{
    public static class ReconFolderTreeExporter
    {
        private const string MenuPath = "FarmSim/Recon/Export Folder Tree";

        [MenuItem(MenuPath)]
        public static void Export()
        {
            try
            {
                string projectRoot = GetProjectRootAbsolute();
                string outputDir = EnsureReconDocsFolder(projectRoot);

                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                string fileName = $"recon_folder_tree_snapshot_{timestamp}.json";
                string outputPath = Path.Combine(outputDir, fileName);

                FolderTreeSnapshot snapshot = BuildSnapshot(projectRoot);
                string json = JsonUtility.ToJson(snapshot, true);

                File.WriteAllText(outputPath, json);
                Debug.Log($"[Recon] Folder tree snapshot written: {outputPath}");

                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Recon] Folder tree export failed: {ex}");
            }
        }

        private static FolderTreeSnapshot BuildSnapshot(string projectRootAbs)
        {
            var snap = new FolderTreeSnapshot
            {
                schemaVersion = 1,
                generatedUtc = DateTime.UtcNow.ToString("O"),
                projectRoot = projectRootAbs,
                entries = new List<FolderTreeEntry>()
            };

            // Exclude noisy / non-authoritative directories.
            var excludedRootSegments = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".git",
                "Library",
                "Temp",
                "Obj",
                "Logs",
                "UserSettings",
                "Build",
                "Builds",
                "Recon Docs" // prevent snapshot self-inflation
            };

            foreach (string path in Directory.EnumerateFileSystemEntries(projectRootAbs, "*", SearchOption.AllDirectories))
            {
                string rel = MakeRelativePath(projectRootAbs, path).Replace('\\', '/');

                string rootSeg = GetFirstSegment(rel);
                if (!string.IsNullOrEmpty(rootSeg) && excludedRootSegments.Contains(rootSeg))
                    continue;

                bool isDir = Directory.Exists(path);
                long bytes = 0;

                if (!isDir)
                {
                    try { bytes = new FileInfo(path).Length; }
                    catch { bytes = 0; }
                }

                snap.entries.Add(new FolderTreeEntry
                {
                    relativePath = rel,
                    entryType = isDir ? "dir" : "file",
                    bytes = bytes
                });
            }

            return snap;
        }

        private static string GetProjectRootAbsolute()
        {
            // IMPORTANT: fully qualify UnityEngine.Application to avoid namespace collisions.
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

        private static string GetFirstSegment(string relPath)
        {
            relPath = relPath.Replace('\\', '/').TrimStart('/');
            int idx = relPath.IndexOf('/');
            return idx < 0 ? relPath : relPath.Substring(0, idx);
        }

        [Serializable]
        private class FolderTreeSnapshot
        {
            public int schemaVersion;
            public string generatedUtc = "";
            public string projectRoot = "";
            public List<FolderTreeEntry> entries = new List<FolderTreeEntry>();
        }

        [Serializable]
        private class FolderTreeEntry
        {
            public string relativePath = "";
            public string entryType = ""; // "dir" | "file"
            public long bytes;
        }
    }
}
