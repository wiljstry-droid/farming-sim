#nullable enable

using UnityEditor;
using UnityEngine;

namespace FarmSim.Project.Editor
{
    public static class ReconExportAll
    {
        private const string MenuPath = "FarmSim/Recon/Export ALL";

        [MenuItem(MenuPath)]
        public static void ExportAll()
        {
            Debug.Log("[Recon] Export ALL started.");

            ReconFolderTreeExporter.Export();
            ReconScriptIndexExporter.Export();
            ReconSceneHierarchyExporter.Export();

            Debug.Log("[Recon] Export ALL completed.");
        }
    }
}
