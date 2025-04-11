using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;


public class SaveManager : MonoBehaviour
{
    //static readonly string FILEPATH = Application.persistentDataPath + "/Save.json";
    static readonly string FILEPATH = Application.persistentDataPath + "/Save.save";

    public static void Save(GameSaveState save)
    {
       // string json = JsonUtility.ToJson(save);
       // File.WriteAllText(FILEPATH, json);

        using (FileStream file = File.Create(FILEPATH))
        {
            new BinaryFormatter().Serialize(file, save);

        }
    }

    //Load the save file
    public static GameSaveState Load()
    {
        GameSaveState loadedSave = null;
        /*
        if (File.Exists(FILEPATH))
        {
            string json = File.ReadAllText(FILEPATH);
            loadedSave = JsonUtility.FromJson<GameSaveState>(json);            
        }
        */
        if (File.Exists(FILEPATH))
        {
            using (FileStream file = File.Open(FILEPATH, FileMode.Open))
            {
                loadedSave = (GameSaveState)new BinaryFormatter().Deserialize(file);
            }
        }
        return loadedSave;
    }

    public static bool HasSave()
    {
        return File.Exists(FILEPATH);
    }

}
