using System.Collections.Generic;

namespace U3D.Migration.Editor
{
    /// <summary>
    /// Maps GameObject naming hints to a friendly, plain-language description of the kind
    /// of interactivity the object probably had. Project-agnostic: it makes no assumption
    /// that any particular toolset is installed. The placeholder Inspector uses the hint to
    /// point creators toward Unreality3D's free interactivity tools as one way to rebuild it.
    /// </summary>
    public static class MigrationSuggestions
    {
        /// <summary>
        /// Where the placeholder Inspector's "Learn more" button sends creators.
        /// Change this single string to retarget the link.
        /// </summary>
        public const string LearnMoreUrl = "https://unreality3d.com/quickstart.html";

        // Substring found in a GameObject name -> short, friendly description of the
        // interactivity that name implies. First match wins.
        private static readonly Dictionary<string, string> nameToHint = new Dictionary<string, string>
        {
            // Object interactions
            {"grabbable", "grabbing and carrying objects"},
            {"grab", "grabbing and carrying objects"},
            {"pickup", "grabbing and carrying objects"},
            {"throw", "throwing objects"},
            {"kick", "kicking objects"},
            {"push", "pushing objects"},
            {"climb", "climbing"},
            {"swim", "swimming"},
            {"spawn", "spawning objects"},

            // Triggers and activations
            {"interactable", "triggers and interactions"},
            {"interact", "triggers and interactions"},
            {"trigger", "triggers and interactions"},
            {"switch", "triggers and interactions"},
            {"door", "triggers and interactions"},
            {"button", "triggers and interactions"},
            {"click", "triggers and interactions"},

            // Movement and transport
            {"rideable", "rideable platforms"},
            {"ride", "rideable platforms"},
            {"platform", "rideable platforms"},
            {"vehicle", "steerable vehicles"},
            {"portal", "portals and teleportation"},
            {"teleport", "portals and teleportation"},
            {"warp", "portals and teleportation"},

            // Game systems
            {"quest", "quests"},
            {"mission", "quests"},
            {"objective", "quests"},
            {"inventory", "inventory"},
            {"item", "inventory"},
            {"dialogue", "dialogue"},
            {"conversation", "dialogue"},
            {"npc", "dialogue"},
            {"timer", "timers"},
            {"countdown", "timers"},
            {"achievement", "achievements"},
            {"unlock", "achievements"},
            {"reward", "achievements"},
            {"score", "score tracking"},
            {"checkpoint", "checkpoints"},
            {"respawn", "checkpoints"},
            {"quiz", "quizzes"},
            {"question", "quizzes"},

            // Media and content
            {"audio", "audio playback"},
            {"sound", "audio playback"},
            {"music", "audio playback"},
            {"video", "video playback"},
            {"slide", "slide presentations"},
            {"presentation", "slide presentations"},
            {"guestbook", "guestbooks"},
            {"message", "guestbooks"},
            {"sign", "world-space signs and labels"},
            {"label", "world-space signs and labels"},
            {"url", "clickable links"},
            {"link", "clickable links"},
            {"website", "clickable links"},

            // Monetization
            {"shop", "shops and purchases"},
            {"purchase", "shops and purchases"},
            {"buy", "shops and purchases"},
            {"tip", "tip jars"},
            {"donate", "tip jars"},
            {"gate", "paywalls and access gates"},
            {"ticket", "ticketed events"},
            {"event", "ticketed events"},
        };

        /// <summary>
        /// Returns a friendly description of the interactivity a GameObject's name implies,
        /// or empty string if no keyword matches.
        /// </summary>
        public static string GetInteractivityHint(string gameObjectName)
        {
            if (string.IsNullOrEmpty(gameObjectName)) return "";

            string lowerName = gameObjectName.ToLower();

            foreach (var kvp in nameToHint)
            {
                if (lowerName.Contains(kvp.Key))
                {
                    return kvp.Value;
                }
            }

            return "";
        }
    }
}
