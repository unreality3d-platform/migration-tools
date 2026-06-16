using System.IO;
using UnityEditor;
using UnityEngine;

namespace U3D.Migration.Editor
{
    public static class MigrationPackageExporter
    {
        private const string EXPORT_FOLDER = "Assets/Migration Tools";
        private const string OUTPUT_DIRECTORY = "Build";
        private const string PACKAGE_NAME = "MigrationTools.unitypackage";

        [MenuItem("Tools/Unreality3D/Export .unitypackage")]
        public static void ExportPackage()
        {
            string outputPath = Export();
            EditorUtility.RevealInFinder(outputPath);
        }

        public static void ExportPackageCommandLine()
        {
            try
            {
                string outputPath = Export();
                Debug.Log("[MigrationPackageExporter] Export succeeded: " + outputPath);
                EditorApplication.Exit(0);
            }
            catch (System.Exception e)
            {
                Debug.LogError("[MigrationPackageExporter] Export failed: " + e);
                EditorApplication.Exit(1);
            }
        }

        private static string Export()
        {
            if (!AssetDatabase.IsValidFolder(EXPORT_FOLDER))
                throw new DirectoryNotFoundException("Export folder not found: " + EXPORT_FOLDER);

            Directory.CreateDirectory(OUTPUT_DIRECTORY);
            string outputPath = Path.Combine(OUTPUT_DIRECTORY, PACKAGE_NAME);

            AssetDatabase.ExportPackage(EXPORT_FOLDER, outputPath, ExportPackageOptions.Recurse);

            return outputPath;
        }
    }
}
