using UnityEngine;

namespace U3D.Migration
{
    /// <summary>
    /// Placeholder component marking where a missing script was removed during migration.
    /// Runtime component (not editor-only) so it can be attached to scene GameObjects; its
    /// Inspector is provided by MissingScriptPlaceholderEditor in the Editor assembly.
    /// </summary>
    public class MissingScriptPlaceholder : MonoBehaviour
    {
        [SerializeField]
        [TextArea(3, 5)]
        private string missingScriptNote = "⚠️ This component replaced a missing script.\n\nUse 'Remove Script Placeholders' in the Migration Tools window to clean these up when ready.";

        [SerializeField]
        private string replacementDateTime = "";

        [SerializeField]
        private string suggestedInteractivity = "";

        public string SuggestedInteractivity => suggestedInteractivity;
        public string ReplacementDateTime => replacementDateTime;

        /// <summary>
        /// Sets a friendly interactivity hint (for example "grabbing and carrying objects").
        /// Called by the editor cleanup tool at placeholder-creation time, since the lookup
        /// lives in the editor assembly and this runtime class can't reference it directly.
        /// </summary>
        public void SetSuggestedInteractivity(string hint)
        {
            suggestedInteractivity = hint;
        }

        /// <summary>
        /// Sets the replacement timestamp. Called by the editor cleanup tool at
        /// placeholder-creation time. Editor AddComponent calls don't fire Awake,
        /// so this can't be done lazily on first run.
        /// </summary>
        public void SetReplacementDateTime(string dateTime)
        {
            if (string.IsNullOrEmpty(replacementDateTime))
            {
                replacementDateTime = dateTime;
            }
        }
    }
}
