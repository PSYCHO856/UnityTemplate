using UnityEngine;
using I2.Loc;

namespace MobileKit
{
    public enum DefaultLanguage
    {
        FollowSystem,
        Chinese_Traditional,
        English,
        French,
        German,
        Italian,
        Portuguese,
        Spanish,
        Japanese,
        Malay,
        Indonesian,
        Russian,
        Chinese_Simplified,
    }
    
    public static class LanguageManager
    {
        public static void Init()
        {
            // ChangeLanguageTo(BuildConfig.Instance.Language);
            if ( SelectedLanguage == DefaultLanguage.FollowSystem )
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.ChineseSimplified:
                    case SystemLanguage.ChineseTraditional:
                        SelectedLanguage = DefaultLanguage.Chinese_Traditional;
                        break;
                    case SystemLanguage.French:
                        SelectedLanguage = DefaultLanguage.French;
                        break;
                    case SystemLanguage.German:
                        SelectedLanguage = DefaultLanguage.German;
                        break;
                    case SystemLanguage.Italian:
                        SelectedLanguage = DefaultLanguage.Italian;
                        break;
                    case SystemLanguage.Portuguese:
                        SelectedLanguage = DefaultLanguage.Portuguese;
                        break;
                    case SystemLanguage.Spanish:
                        SelectedLanguage = DefaultLanguage.Spanish;
                        break;
                    case SystemLanguage.Japanese:
                        SelectedLanguage = DefaultLanguage.Japanese;
                        break;
                    case SystemLanguage.Indonesian:
                        SelectedLanguage = DefaultLanguage.Indonesian;
                        break;
                    case SystemLanguage.Russian:
                        SelectedLanguage = DefaultLanguage.Russian;
                        break;
                    default:
                        SelectedLanguage = DefaultLanguage.English;
                        break;
                }
            }
            ChangeLanguageTo(SelectedLanguage);
            if (BuildConfig.Instance.Debug)
            {
                GMManager.OnDrawInstruct += RegisterGMHelper;
            }
        }

        public static DefaultLanguage SelectedLanguage
        {
            get
            {
                return (DefaultLanguage)(GameSettingsPrefs.GetInt("SelectedLanguage", (int)BuildConfig.Instance.Language));
            }
            set
            {

                GameSettingsPrefs.SetInt("SelectedLanguage", (int)value);
            }
        }

        public static void ChangeLanguageTo(DefaultLanguage language)
        {
            SelectedLanguage = language;

            if (language == DefaultLanguage.Chinese_Simplified)
            {
                LocalizationManager.CurrentLanguage = "Chinese (Simplified)";
            // } else if (BuildConfig.Instance.EnableTraditionalChinese && language == DefaultLanguage.Chinese_Traditional)
            } else if (language == DefaultLanguage.Chinese_Traditional)
            {
                LocalizationManager.CurrentLanguage = "Chinese (Traditional)";
            }else if (language == DefaultLanguage.FollowSystem)
            {
                string systemLanguage = Application.systemLanguage.ToString();
                Debug.Log($" ChangeLanguageTo systemLanguage: {systemLanguage} ");
                systemLanguage = systemLanguage switch
                {
                    "ChineseSimplified" => "Chinese (Simplified)",
                    "ChineseTraditional" => "Chinese (Traditional)",
                    _ => systemLanguage
                };
                ChangeLanguageTo(systemLanguage);
            } else 
            {
                ChangeLanguageTo(language.ToString());
            }
        }

        private static void ChangeLanguageTo(string language)
        {
            string validLanguage = LocalizationManager.GetSupportedLanguage(language, true);
            if (!string.IsNullOrEmpty(validLanguage))
            {
                LocalizationManager.SetLanguageAndCode(validLanguage, LocalizationManager.GetLanguageCode(validLanguage), false);
            }
            else
            {
                LocalizationManager.CurrentLanguage = language;
            }
        }
        
        
        private static void RegisterGMHelper()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("切换英文"))
            {
                ChangeLanguageTo("English");
            }

            if (GUILayout.Button("切换中文(简体)"))
            {
                ChangeLanguageTo("Chinese (Simplified)");
            }

            if (GUILayout.Button("切换中文(繁体)"))
            {
                ChangeLanguageTo("Chinese (Traditional)");
            }
            GUILayout.EndHorizontal();
        }
    }
}