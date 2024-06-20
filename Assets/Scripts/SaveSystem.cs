using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(GameController controller)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + "/blockSave.txt";
        FileStream stream = new(path, FileMode.OpenOrCreate);

        SaveData data = new(controller);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/blockSave.txt";
        if (File.Exists(path))
        {
            FileStream stream = new(path, FileMode.Open);
            BinaryFormatter formatter = new();
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
