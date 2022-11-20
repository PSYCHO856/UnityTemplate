using System;
using System.Linq;
using UnityEngine;

namespace Watermelon
{
    public static partial class GameSettingsPrefs
    {
        private const string PREFS_PREFIX = "settings_";

        static GameSettingsPrefs()
        {
            //Load all settings
            foreach (var settingName in settings.Keys.ToList())
            {
                var prefObject = Get(settingName, settings[settingName]);

                settings[settingName] = prefObject;
            }
        }

        private static T Get<T>(string key, T sourceObject)
        {
            if (!settings.ContainsKey(key))
            {
                Debug.LogError("[GameSettings]: Unknown setting key!");

                return default;
            }

            var value = PlayerPrefs.GetString(PREFS_PREFIX + key, null);

            if (string.IsNullOrEmpty(value)) return (T) settings[key];

            return (T) Convert.ChangeType(value, sourceObject.GetType());
        }

        public static T Get<T>(string key)
        {
            if (!settings.ContainsKey(key))
            {
                Debug.LogError("[GameSettings]: Unknown setting key!");

                return default;
            }

            return (T) settings[key];
        }

        public static void Set(string key, object value)
        {
            if (!settings.ContainsKey(key))
            {
                Debug.LogError("[GameSettings]: Unknown setting key!");

                return;
            }

            settings[key] = value;

            PlayerPrefs.SetString(PREFS_PREFIX + key, value.ToString());
        }
    }
}