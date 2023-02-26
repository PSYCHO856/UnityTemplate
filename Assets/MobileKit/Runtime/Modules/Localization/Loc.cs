using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    
/*
    增加攻击力{0}点
    增加攻击力{0}点, 并减速{1}秒
    增加攻击力<color=red>{0}</color>点, 并减速<color=red>{1}</color>秒
    增加攻击力<color=#9370DB>{0}</color>点, 并减速<color=red>{1}</color>秒
    <color=#9370DB>增加攻击力{0}点</color>, 并减速{1}秒
    <b>增加攻击力</b>5点
*/
    
    public static class Loc
    {
        public static string GetModelText<T>(string term, T param)
        {
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param.ToString());
        }
        public static string GetModelText<T, U>(string term, T param0, U param1)
        {
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param0.ToString(), param1.ToString());
        }

        public static string GetModelText<T, U, V>(string term, T param0, U param1, V param2)
        {
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param0.ToString(), param1.ToString(), param2.ToString());
        }

        public static string GetModelText<T, U, V, W>(string term, T param0, U param1, V param2, W param3)
        {
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param0.ToString(), param1.ToString(), param2.ToString(), param3.ToString());
        }

        public static string GetModelText<T>(Localize localize, T param)
        {
            string term = localize?.mTerm;
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param.ToString());
        }
        public static string GetModelText<T, U>(Localize localize, T param0, U param1)
        {
            string term = localize?.mTerm;
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param0.ToString(), param1.ToString());
        }
        public static string GetModelText<T, U, V>(Localize localize, T param0, U param1, V param2)
        {
            string term = localize?.mTerm;
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param0.ToString(), param1.ToString(), param2.ToString());
        }
        public static string GetModelText<T, U, V, W>(Localize localize, T param0, U param1, V param2, W param3)
        {
            string term = localize?.mTerm;
            if( LocalizationManager.GetTranslation(term) == default ){
                Debug.LogWarning( $" IDT :{term} is Null "  );
                return "";
            }
            return string.Format(LocalizationManager.GetTranslation(term), param0.ToString(), param1.ToString(), param2.ToString(), param3.ToString());
        }

        public static string GetModelTextMultiReplace( string term, List<string> replaceList )
        {
            string result = LocalizationManager.GetTranslation(term);
            if( result == default || replaceList == default ){
                Debug.LogWarning( $" IDT :{term} is Null or replaceList is null "  );
                return "";
            }
            // string result = term;
            for (int i = 0; i < replaceList.Count; i++)
            {
                string tag = "{"+i+"}";
                if (result.Contains(tag))
                {
                    result = result.Replace( tag, replaceList[i] );                    
                }
            }
            return result;
        }

        // public static void ChangeLanguageTo(string language)
        // {
        //     LocalizationManager.CurrentLanguage = language;
        // }
        //
        // public static bool HasLanguage(string language)
        // {
        //     return LocalizationManager.HasLanguage(language);
        // }
    }
}