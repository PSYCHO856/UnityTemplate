using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MobileKit
{
    public static class GameSettingsPrefs
    {
        private const string PREFS_PREFIX = "settings_";

        private static Dictionary<string, int> intSettings;
        private static Dictionary<string, float> floatSettings;
        private static Dictionary<string, bool> boolSettings;
        private static Dictionary<string, string> stringSettings;
        private static Dictionary<string, DateTime> dateTimeSettings;
        
        public static int GetInt(string key, int defaultValue)
        {
            string newKey = PREFS_PREFIX + key;
            intSettings ??= new Dictionary<string, int>();
            if (!intSettings.TryGetValue(newKey, out var value))
            {   
                value = PlayerPrefs.GetInt(newKey, defaultValue);
                intSettings.Add(newKey, value);
            }
            return value;
        }

        public static float GetFloat(string key, float defaultValue)
        {
            string newKey = PREFS_PREFIX + key;
            floatSettings ??= new Dictionary<string, float>();
            if (!floatSettings.TryGetValue(newKey, out var value))
            {
                value = PlayerPrefs.GetFloat(newKey, defaultValue);
                floatSettings.Add(newKey, value);
            }
            return value;
        }
        
        public static bool GetBool(string key, bool defaultValue)
        {
            string newKey = PREFS_PREFIX + key;
            boolSettings ??= new Dictionary<string, bool>();
            if (!boolSettings.TryGetValue(newKey, out var value))
            {
                value = PlayerPrefs.GetInt(newKey, defaultValue ? 1 : 0) == 1;
                boolSettings.Add(newKey, value);
            }
            return value;
        }
        
        public static string GetString(string key, string defaultValue)
        {
            string newKey = PREFS_PREFIX + key;
            stringSettings ??= new Dictionary<string, string>();
            if (!stringSettings.TryGetValue(newKey, out var value))
            {
                value = PlayerPrefs.GetString(newKey, defaultValue);
                stringSettings.Add(newKey, value);
            }
            return value;
        }

        public static DateTime GetDateTime(string key, DateTime defaultValue)
        {
            string newKey = PREFS_PREFIX + key;
            dateTimeSettings ??= new Dictionary<string, DateTime>();
            if (!dateTimeSettings.TryGetValue(newKey, out var value))
            {
                string time = PlayerPrefs.GetString(newKey, defaultValue.ToString());
                value = DateTime.Parse(time);
                dateTimeSettings.Add(newKey, value);
            }
            return value;
        }
        
        public static void SetInt(string key, int value)
        {
            string newKey = PREFS_PREFIX + key;
            intSettings ??= new Dictionary<string, int>();
            intSettings[newKey] = value;
            PlayerPrefs.SetInt(newKey, value);
        }
        
        
        public static void SetFloat(string key, float value)
        {
            string newKey = PREFS_PREFIX + key;
            floatSettings ??= new Dictionary<string, float>();
            floatSettings[newKey] = value;
            PlayerPrefs.SetFloat(newKey, value);
        }
        
        public static void SetBool(string key, bool value)
        {
            string newKey = PREFS_PREFIX + key;
            boolSettings ??= new Dictionary<string, bool>();
            boolSettings[newKey] = value;
            PlayerPrefs.SetInt(newKey, value ? 1 : 0);
        }
        
        public static void SetString(string key, string value)
        {
            string newKey = PREFS_PREFIX + key;
            stringSettings ??= new Dictionary<string, string>();
            stringSettings[newKey] = value;
            PlayerPrefs.SetString(newKey, value);
        }

        public static void SetDateTime(string key, DateTime value)
        {
            string newKey = PREFS_PREFIX + key;
            dateTimeSettings ??= new Dictionary<string, DateTime>();
            dateTimeSettings[newKey] = value;
            PlayerPrefs.SetString(newKey, value.ToString());
        }
    }
}