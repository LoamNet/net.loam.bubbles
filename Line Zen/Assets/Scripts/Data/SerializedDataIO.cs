using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedDataIO : MonoBehaviour
{
    public Events events;
    public static readonly string statsFile = "stats.txt";

    private void Start()
    {
        events.OnSerializedDataChange += OnSerializedDataChange;
    }

    void OnSerializedDataChange(SerializedData data)
    {
        SetData(data);
    }

    private string SavePath()
    {
        return Application.persistentDataPath + "/";
    }

    public void SetData(SerializedData data)
    {
        string writeFile = SavePath() + statsFile;
        System.IO.File.WriteAllText(writeFile, data.ToString());
    }


    public SerializedData GetData()
    {
        string readFile = SavePath() + statsFile;
        try
        {
            string text = System.IO.File.ReadAllText(readFile);

            if (text != null)
            {
                SerializedData data = new SerializedData();
                data.FromString(text);
                return data;
            }
        }
        catch(System.Exception)
        {
            Debug.Log("Couldn't find any save file - making a new one!");
            SetData(new SerializedData());
        }

        return new SerializedData();
    }
}
