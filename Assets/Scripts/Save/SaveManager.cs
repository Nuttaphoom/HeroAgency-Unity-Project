using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    [ContextMenu("SaveGame")]
    public void Save()
    {
        Dictionary<string, object> states = LoadFile();
        CaptureState(states);
        SaveFile(states);
    }

    [ContextMenu("LoadGame")]
    public void Load()
    {
        Dictionary<string, object> states = LoadFile();
        RestoreState(states);
    }

    public Dictionary<string, object> LoadFile()
    {
        string SavePath = Application.persistentDataPath + "/TestSave";

        if (!File.Exists(SavePath))
            return new Dictionary<string, object>();

        using (Stream stream = File.Open(SavePath, FileMode.Open))
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (Dictionary<string, object>)bf.Deserialize(stream);
        }


    }

    public void SaveFile(object state)
    {
        string SavePath = Application.persistentDataPath + "/TestSave";

        using (Stream stream = File.Open(SavePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }


    }


    //Call CaptrueState in every savable objeccts
    public void CaptureState(Dictionary<string, object> states)
    {
        foreach (var savable in FindObjectsOfType<SavableObject>())
        {
            states[savable.SaveID] = savable.CaptureState();
        }

    }
    //Call RestoreState in every savable objeccts
    public void RestoreState(Dictionary<string, object> states)
    {
        foreach (var savable in FindObjectsOfType<SavableObject>())
        {
            if (states.TryGetValue(savable.SaveID, out var data))
            {
                savable.RestoreState(data);
            }
        }
    }

}
