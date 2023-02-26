using System.IO;
using UnityEngine;

namespace MobileKit
{
    public class RecordManager : Singleton<RecordManager>
    {
        public static RecordData Data => Instance.data;
        private RecordData data;
        
        private string filePath;
        public bool Initted { get; set; }
        public void Init()
        {
            string path = Path.Combine(Application.persistentDataPath, "Save");
            IOUtils.CreateDirectory(path);
            filePath = Path.Combine(path, "SaveData.sav");
            Debug.Log(filePath);
            Load();
            Initted = true;

            // Data.SetFirstDate();
        }

        private void Save()
        {
            if (!Initted) return;
            // if (NoviceManager.IsNovice) return;
            IOUtils.SaveData(filePath, data);
        }

        private void Load()
        {
            if (!IOUtils.IsFileExists(filePath))
            {
                data = new RecordData();
                return;
            }
            data = (RecordData)IOUtils.ReadData(filePath, typeof(RecordData));
        }
        
        private void OnApplicationQuit()
        {
            Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Save();
            } 
        }
    }
}