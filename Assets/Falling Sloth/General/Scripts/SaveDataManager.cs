using System.IO;
using System.Text;
using UnityEngine;

namespace FallingSloth
{
    /// <summary>
    /// Manages save data stored in a JSON file in the persistent data path.
    /// </summary>
    /// <typeparam name="T">The type of the save data class used to serialize the save data.  Must inherit from <see cref="FallingSloth.SaveData"/>.</typeparam>
    public static class SaveDataManager<T> where T : SaveData, new()
    {
        static string dataPath = Application.persistentDataPath + "/savedata.dat";
        
        static T _data;
        public static T data
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

        /// <summary>
        /// Loads the save data from file on disk.  If no file is found, creates a new instance of <see cref="T"/>.  Result is stored in <see cref="data"/>.
        /// </summary>
        public static void LoadData()
        {
            if (!File.Exists(dataPath))
            {
                data = new T();
            }
            else
            {
                string json = File.ReadAllText(dataPath);

                data = JsonUtility.FromJson<T>(json);
            }
        }

        /// <summary>
        /// Saves the data in <see cref="data"/> to file on disk.
        /// </summary>
        public static void SaveData()
        {
            File.WriteAllText(dataPath, JsonUtility.ToJson(data), Encoding.UTF8);
        }
    }

    /// <summary>
    /// Base class for save data.  Must be extended to contain the required data.
    /// </summary>
    public abstract class SaveData
    {
    }
}
