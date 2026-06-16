using UnityEditor;
using UnityEngine;

namespace U3D.Migration.Editor
{
    [CustomEditor(typeof(MissingScriptPlaceholder))]
    public class MissingScriptPlaceholderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var placeholder = (MissingScriptPlaceholder)target;
            string hint = placeholder.SuggestedInteractivity;

            EditorGUILayout.Space(8);

            if (!string.IsNullOrEmpty(hint))
            {
                EditorGUILayout.HelpBox(
                    $"This object's name suggests it used {hint}. " +
                    "Unreality3D's free Creator Dashboard includes ready-made tools for adding interactivity like this without code.",
                    MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "Unreality3D's free Creator Dashboard includes ready-made tools for adding interactivity " +
                    "without code, which can help rebuild whatever this missing script did.",
                    MessageType.Info);
            }

            if (GUILayout.Button("Learn more at unreality3d.com", GUILayout.Height(28)))
            {
                Application.OpenURL(MigrationSuggestions.LearnMoreUrl);
            }
        }
    }
}
