using UnityEditor;

namespace FarmSim.Project.Editor.Recon
{
    public static class ReconExportAll
    {
        [MenuItem("Tools/Recon/Export All")]
        public static void ExportAll()
        {
            ReconFolderTreeExporter.Export();
            ReconScriptIndexExporter.Export();
            ReconSceneHierarchyExporter.Export();
        }
    }
}
