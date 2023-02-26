using UnityEditor;

namespace MobileKit.Editor
{
    public static class SymbolsManager
    {
        public static void AddToPlatform(string symbol, BuildTargetGroup target)
        {
            Modify(symbol, false, target);
        }

        public static void RemoveFromPlatform(string symbol, BuildTargetGroup target)
        {
            Modify(symbol, true, target);
        }
        
        private static void Modify(string symbol, bool remove, BuildTargetGroup target)
        {
            string textToWrite = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            if (remove)
            {
                if (textToWrite.Contains(symbol))
                {
                    textToWrite = textToWrite.Replace(symbol, "");  
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);
                }
            }
            else
            {
                if (!textToWrite.Contains(symbol))
                {
                    if (textToWrite == "")
                    {
                        textToWrite += symbol;
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);
                    }
                    else
                    {
                        textToWrite += "," + symbol;
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);
                    }
                }
            }
        }
    }
}