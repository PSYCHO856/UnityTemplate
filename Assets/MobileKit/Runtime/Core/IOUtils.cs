#pragma warning disable 219

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace MobileKit
{
    public static class IOUtils
    {

        public static void CreateDirectory(string dirName)
        {
            //文件夹存在则返回
            if(IsDirectoryExists(dirName))
                return;
            Directory.CreateDirectory(dirName);
        }
        
        /// <summary>
        /// 待验证. Creating all folders in the path if they don't exist
        /// </summary>
        public static void CreatePath(string path, char separator = '/')
        {
            if (Directory.Exists(path))
                return;

            bool pathCreated = false;

            string[] pathFolders = path.Split(separator);
            for (int i = 0; i < pathFolders.Length; i++)
            {
                string tempPath = "";

                for (int j = 0; j < i; j++)
                {
                    tempPath += pathFolders[j] + "/";
                }

                if (!Directory.Exists(tempPath + pathFolders[i]))
                {
                    Directory.CreateDirectory(tempPath + pathFolders[i]);

                    pathCreated = true;
                }
            }

#if UNITY_EDITOR
            if (!Application.isPlaying && pathCreated)
                UnityEditor.AssetDatabase.Refresh();
#endif
        }
        
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        public static bool IsFileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        public static bool IsDirectoryExists(string fileName)
        {
            return Directory.Exists(fileName);
        }

        public static void Move(string source, string dest)
        {
            Directory.Move(source, dest);
        }
        /// <summary>
        /// 保存文本文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public static void SaveData(string path, string text)
        {
            StreamWriter streamWriter = File.CreateText(path);
            streamWriter.Write(text);
            streamWriter.Close();
        }
        
        /// <summary>
        /// 保存加密后的对象文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static void SaveData(string path, object obj)
        {
            string data = SerializeObject(obj);
            if (BuildConfig.Instance.Debug)
            {
                Debug.Log("SaveData: " + data);
            }
            data = RijndaelEncrypt(data);
            SaveData(path, data);
        }

        public static string ReadData(string path)
        {
            StreamReader reader = File.OpenText(path);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }


        public static object ReadData(string path, Type pType)
        {
            string data = ReadData(path);
            data = RijndaelDecrypt(data);
            MLog.Log(nameof(IOUtils), "ReadData: " + data);
            return DeserializeObject(data, pType);
        }
        
        
        /// <summary>
        /// 将一个对象序列化为字符串
        /// </summary>
        public static string SerializeObject(object pObject)
        {
            return JsonConvert.SerializeObject(pObject, Formatting.Indented);
        }

        /// <summary>
        /// 将一个字符串反序列化为对象
        /// </summary>
        public static object DeserializeObject(string pString,Type pType)
        {
            object deserializedObject = JsonConvert.DeserializeObject(pString,pType, new JsonSerializerSettings()
            {
                ContractResolver = new JsonPrivateResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
            return deserializedObject;
        }
        
        private const string encryptKey = "A60A5770FE5E7AB200BA9CFC94E4E8B0";
        private static RijndaelManaged rijndaelManaged;
        private static ICryptoTransform enCryptor;
        private static ICryptoTransform deCryptor;
        
        /// <summary>
        /// Rijndael加密算法
        /// </summary>
        public static string RijndaelEncrypt(string pString)
        {
            if (string.IsNullOrEmpty(pString)) return "";
            if (rijndaelManaged == null)
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(encryptKey);
                rijndaelManaged = new RijndaelManaged
                {
                    Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7
                };
            }
            enCryptor ??= rijndaelManaged.CreateEncryptor();
            
            byte[] sourceStr = UTF8Encoding.UTF8.GetBytes(pString);
            byte[] resultStr = enCryptor.TransformFinalBlock(sourceStr, 0, sourceStr.Length);
            return Convert.ToBase64String(resultStr, 0, resultStr.Length);
        }

        /// <summary>
        /// Rijndael解密算法
        /// </summary>
        public static string RijndaelDecrypt(string pString)
        {
            if (string.IsNullOrEmpty(pString)) return "";
            if (rijndaelManaged == null)
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(encryptKey);
                rijndaelManaged = new RijndaelManaged
                {
                    Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7
                };
            }
            deCryptor ??= rijndaelManaged.CreateDecryptor();

            byte[] sourceStr = Convert.FromBase64String(pString);
            byte[] resultStr = deCryptor.TransformFinalBlock(sourceStr, 0, sourceStr.Length);
            return UTF8Encoding.UTF8.GetString(resultStr);
        }
    }
    
    public class JsonPrivateResolver : DefaultContractResolver {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (!prop.Writable) {
                var property = member as PropertyInfo;
                var hasPrivateSetter = property?.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter;
            }
            return prop;
        }
    }
}
