using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace U3D.Migration.Editor
{
    public class MigrationToolsWindow : EditorWindow
    {
        private Vector2 scroll;
        private bool highlight;
        private MigrationScanResult lastScan;
        private bool hasScanned;

        [MenuItem("Tools/Unreality3D/Migration Tools")]
        public static void Open()
        {
            var window = GetWindow<MigrationToolsWindow>("Migration Tools");
            window.minSize = new Vector2(360f, 480f);
        }

        private void OnGUI()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);

            EditorGUILayout.LabelField("Migration Tools", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Clean up missing scripts, broken object references, and third-party Visual Scripting graphs when migrating a Unity project from another platform. Every tool can be undone with Ctrl+Z.",
                MessageType.Info);
            EditorGUILayout.Space(8);

            DrawScanSection();
            EditorGUILayout.Space(10);
            DrawMissingScriptsSection();
            EditorGUILayout.Space(10);
            DrawMissingReferencesSection();
            EditorGUILayout.Space(10);
            DrawVisualScriptingSection();

            EditorGUILayout.EndScrollView();
        }

        private void DrawScanSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("🔎 See What Needs Fixing", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(
                "Highlight and select problem objects before you change anything. Nothing here modifies your scene.",
                EditorStyles.wordWrappedMiniLabel);
            EditorGUILayout.Space(4);

            highlight = MigrationHighlightOverlay.IsActive;
            bool newHighlight = EditorGUILayout.ToggleLeft(
                "Highlight issues in the Scene view",
                highlight);
            if (newHighlight != highlight)
            {
                MigrationHighlightOverlay.SetActive(newHighlight);
                highlight = newHighlight;
            }

            EditorGUILayout.LabelField(
                "Red = missing scripts   Yellow = missing references   Blue = placeholders",
                EditorStyles.miniLabel);
            EditorGUILayout.Space(4);

            if (GUILayout.Button("Scan and Select All Issues", GUILayout.Height(30)))
            {
                RunScan();
            }

            if (hasScanned)
            {
                EditorGUILayout.HelpBox(
                    $"Last scan: {lastScan.missingScripts.Count} with missing scripts, " +
                    $"{lastScan.missingReferences.Count} with missing references, " +
                    $"{lastScan.placeholders.Count} with placeholders.",
                    MessageType.None);
            }

            EditorGUILayout.EndVertical();
        }

        private void RunScan()
        {
            lastScan = MigrationScanner.ScanLoadedScenes();
            hasScanned = true;

            var union = new List<UnityEngine.Object>();
            var seen = new HashSet<GameObject>();

            void AddAll(List<GameObject> list)
            {
                foreach (GameObject go in list)
                {
                    if (go != null && seen.Add(go)) union.Add(go);
                }
            }

            AddAll(lastScan.missingScripts);
            AddAll(lastScan.missingReferences);
            AddAll(lastScan.placeholders);

            Selection.objects = union.ToArray();

            if (MigrationHighlightOverlay.IsActive) MigrationHighlightOverlay.Rescan();

            Debug.Log(
                $"🔎 Migration scan: {lastScan.missingScripts.Count} missing-script object(s), " +
                $"{lastScan.missingReferences.Count} missing-reference object(s), " +
                $"{lastScan.placeholders.Count} placeholder object(s). " +
                $"Selected {union.Count} object(s) in the Hierarchy.");
        }

        private void DrawMissingScriptsSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("🔄 Missing Scripts", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("1. Replace → creates placeholders so you can see where scripts were", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("2. Fix or rebuild functionality", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("3. Remove placeholders when done", EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);

            ToolButton(
                "Replace Missing Scripts",
                "Replace missing script references with placeholder components to prevent errors while keeping a visible reminder of where they were.",
                AssetCleanupTools.ReplaceMissingScriptsWithPlaceholders);

            ToolButton(
                "Remove Script Placeholders",
                "Remove placeholder components added by the Replace Missing Scripts tool.",
                AssetCleanupTools.RemovePlaceholderComponents);

            ToolButton(
                "Clean Missing Scripts from Scene",
                "Remove missing script components directly from all GameObjects in the loaded scenes.",
                AssetCleanupTools.RemoveMissingScriptsFromScene);

            ToolButton(
                "Clean Missing Scripts from Prefabs",
                "Remove missing script components from prefabs in a selected folder.",
                AssetCleanupTools.CleanPrefabsInFolder);
        }

        private void DrawMissingReferencesSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("🔗 Missing References", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("1. Replace → tracks missing object references", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("2. Find → locate and rewire them", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("3. Remove placeholders when done", EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);

            ToolButton(
                "Replace Missing References",
                "Detect missing object references inside components and add placeholder tracking components.",
                AssetCleanupTools.ReplaceMissingReferencesWithPlaceholders);

            ToolButton(
                "Find Reference Placeholders",
                "Locate and select all GameObjects with missing reference placeholders for easy rewiring.",
                AssetCleanupTools.FindMissingReferencePlaceholders);

            ToolButton(
                "Remove Reference Placeholders",
                "Remove all missing reference placeholder components from the scene.",
                AssetCleanupTools.RemoveMissingReferencePlaceholders);
        }

        private void DrawVisualScriptingSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("🎮 Visual Scripting", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("1. Clean project assets → clears broken graphs in files", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("2. Clean scene graphs → clears remaining broken graphs", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("3. Rebuild logic with fresh graphs or your own components", EditorStyles.miniLabel);
            EditorGUILayout.HelpBox(
                "Use these after importing scenes from platforms that embed proprietary Visual Scripting nodes. Graphs are cleared without deleting GameObjects.",
                MessageType.Warning);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);

            ToolButton(
                "Clean Third-Party Project Assets",
                "Scan a selected folder (including subfolders) for .asset and .prefab files containing third-party Visual Scripting nodes. Clears broken graphs and removes missing scripts from prefabs in one pass.",
                AssetCleanupTools.CleanThirdPartyVSProjectAssets);

            ToolButton(
                "Clean Visual Scripting Graphs",
                "Scan the loaded scenes for Script Machine and State Machine components with third-party nodes and clear their graphs to stop deserialization errors.",
                AssetCleanupTools.CleanVisualScriptingGraphs);
        }

        private void ToolButton(string title, string description, Action action)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.LabelField(description, EditorStyles.wordWrappedMiniLabel);

            if (GUILayout.Button("Apply", GUILayout.Height(28)))
            {
                if (EditorUtility.DisplayDialog(
                    "Confirm",
                    $"{title}\n\n{description}\n\nThis action can be undone with Ctrl+Z.",
                    "Continue", "Cancel"))
                {
                    action?.Invoke();
                    if (MigrationHighlightOverlay.IsActive) MigrationHighlightOverlay.Rescan();
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(4);
        }
    }
}
