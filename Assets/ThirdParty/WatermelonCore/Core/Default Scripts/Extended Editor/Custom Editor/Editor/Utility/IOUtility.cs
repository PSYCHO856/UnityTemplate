using System.IO;
using System.Text;
using UnityEngine;

namespace Watermelon
{
    public static class IOUtility
    {
        public static string GetPersistentDataPath()
        {
            return Application.persistentDataPath + "/";
        }

        public static void WriteToFile(string filePath, string content)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream, Encoding.ASCII))
                {
                    streamWriter.WriteLine(content);
                }
            }
        }

        public static string ReadFromFile(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.ASCII))
                {
                    var content = streamReader.ReadToEnd();
                    return content;
                }
            }
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static string GetPathRelativeToProjectFolder(string fullPath)
        {
            var indexOfAssetsWord = fullPath.IndexOf("\\Assets");
            var relativePath = fullPath.Substring(indexOfAssetsWord + 1);

            return relativePath;
        }
    }
}