#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarmSim.Project.Editor
{
    public static class ReconSceneHierarchyExporter
    {
        private const string MenuPath = "FarmSim/Recon/Export Scene Hierarchy";

        [MenuItem(MenuPath)]
        public static void Export()
        {
            try
            {
                string projectRoot = GetProjectRootAbsolute();
                string outputDir = EnsureReconDocsFolder(projectRoot);

                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                string fileName = $"recon_scene_hierarchy_snapshot_{timestamp}.json";
                string outputPath = Path.Combine(outputDir, fileName);

                SceneHierarchySnapshot snapshot = BuildSnapshot();
                string json = JsonUtility.ToJson(snapshot, true);

                File.WriteAllText(outputPath, json);
                Debug.Log($"[Recon] Scene hierarchy snapshot written: {outputPath}");

                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Recon] Scene hierarchy export failed: {ex}");
            }
        }

        private static SceneHierarchySnapshot BuildSnapshot()
        {
            Scene active = SceneManager.GetActiveScene();
            if (!active.IsValid() || !active.isLoaded)
                throw new InvalidOperationException("Active scene is not valid or not loaded.");

            var snap = new SceneHierarchySnapshot
            {
                schemaVersion = 1,
                generatedUtc = DateTime.UtcNow.ToString("O"),
                activeSceneName = active.name,
                activeScenePath = active.path,
                roots = new List<GameObjectNode>()
            };

            GameObject[] roots = active.GetRootGameObjects();
            Array.Sort(roots, (a, b) => string.CompareOrdinal(a.name, b.name));

            foreach (var go in roots)
                snap.roots.Add(BuildNode(go));

            return snap;
        }

        private static GameObjectNode BuildNode(GameObject go)
        {
            var node = new GameObjectNode
            {
                name = go.name,
                path = GetHierarchyPath(go.transform),
                isActive = go.activeSelf,
                components = new List<string>(),
                children = new List<GameObjectNode>()
            };

            Component[] comps = go.GetComponents<Component>();
            foreach (var c in comps)
            {
                if (c == null)
                {
                    node.components.Add("<MissingComponent>");
                    continue;
                }
                node.components.Add(c.GetType().FullName ?? c.GetType().Name);
            }

            node.components.Sort(StringComparer.Ordinal);

            int childCount = go.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = go.transform.GetChild(i);
                node.children.Add(BuildNode(child.gameObject));
            }

            node.children.Sort((a, b) => string.CompareOrdinal(a.name, b.name));
            return node;
        }

        private static string GetHierarchyPath(Transform t)
        {
            var stack = new Stack<string>();
            while (t != null)
            {
                stack.Push(t.name);
                t = t.parent;
            }
            return "/" + string.Join("/", stack);
        }

        private static string GetProjectRootAbsolute()
        {
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

        [Serializable]
        private class SceneHierarchySnapshot
        {
            public int schemaVersion;
            public string generatedUtc = "";
            public string activeSceneName = "";
            public string activeScenePath = "";
            public List<GameObjectNode> roots = new List<GameObjectNode>();
        }

        [Serializable]
        private class GameObjectNode
        {
            public string name = "";
            public string path = "";
            public bool isActive;
            public List<string> components = new List<string>();
            public List<GameObjectNode> children = new List<GameObjectNode>();
        }
    }
}
