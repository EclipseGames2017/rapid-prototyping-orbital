using System.IO;
using System.Text;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public static class SaveDataManager
    {
        static string dataPath = Application.persistentDataPath + "/savedata.dat";
        
        static SaveData _data;
        public static SaveData data
        {
            get
            {
                if (_data == null) LoadData();
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public static void LoadData()
        {
            if (!File.Exists(dataPath))
            {
                data = new SaveData();
                SaveData();
            }
            
            string json = File.ReadAllText(dataPath);

            data = JsonUtility.FromJson<SaveData>(json);
        }

        public static void SaveData()
        {
            if (data == null)
                throw new System.Exception("No data to save!");
            File.WriteAllText(dataPath, JsonUtility.ToJson(data), Encoding.UTF8);
        }
    }

    public class SaveData
    {
        public float highscore;
    }
}
