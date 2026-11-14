using System;
using Unity.VisualScripting;

namespace DQHieu.Framework.Data
{
    using UnityEngine;

    public class DataSaveKey
    {
        public const string SampleDataSaveKey = "SAMPLE_DATA_SAVE_KEY";
    }
    
    public class DataManager : DesignPattern.Singleton<DataManager>
    {
        public SampleData SampleData;
        
        public void LoadData()
        {
            SampleData = new();
            if(PlayerPrefs.HasKey(DataSaveKey.SampleDataSaveKey))  SampleData = JsonUtility.FromJson<SampleData>(PlayerPrefs.GetString(DataSaveKey.SampleDataSaveKey));
        }

        public void SaveData()
        {
            PlayerPrefs.SetString(DataSaveKey.SampleDataSaveKey, JsonUtility.ToJson(SampleData));
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveData();
            }
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
        
    }
    
}