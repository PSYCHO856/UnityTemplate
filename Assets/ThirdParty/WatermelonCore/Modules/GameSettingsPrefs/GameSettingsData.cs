using System.Collections.Generic;

namespace Watermelon
{
    public static partial class GameSettingsPrefs
    {
        private static readonly Dictionary<string, object> settings = new()
        {
            {"current_level_id", 0},
            {"actual_level_id", 0},
            {"coins_count_id", 0},
            {"keys_count_id", 0},

            {"last_key_pickup_level_id", 0},

            {"volume", 1.0f},
            {"vibration", true},

            {"no_ads", false}
        };
    }
}