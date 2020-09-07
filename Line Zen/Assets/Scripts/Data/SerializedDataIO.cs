using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedDataIO : MonoBehaviour
{
    public Events events;
    public static readonly string statsFile = "stats.txt";

    private void Start()
    {
        events.OnDataChanged += OnSerializedDataChange;
    }

    void OnSerializedDataChange(DataGeneral data)
    {
        SetData(data);
    }

    private string SavePath()
    {
        return Application.persistentDataPath + "/";
    }

    public void SetData(DataGeneral data)
    {
        string writeFile = SavePath() + statsFile;
        System.IO.File.WriteAllText(writeFile, data.ToString());
    }


    public DataGeneral GetData()
    {
        string readFile = SavePath() + statsFile;
        try
        {
            string text = System.IO.File.ReadAllText(readFile);

            if (text != null)
            {
                DataGeneral data = DataGeneral.Defaults();
                data.FromString(text);
                return data;
            }
        }
        catch(System.Exception)
        {
            Debug.Log("Couldn't find any save file - making a new one!");
        }

        DataGeneral dataNew = DataGeneral.Defaults();
        SetData(dataNew);
        return DataGeneral.Defaults();
    }
}
