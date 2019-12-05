using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Data : MonoBehaviour
{
    public Events events;
    public SerializedDataIO dataIO;

    private DataGeneral data;

    public void Initialize()
    {
        if (dataIO != null)
        {
            SetDataGeneral(dataIO.GetData());
        }
        else
        {
            SetDataGeneral(DataGeneral.Defaults());
            Debug.LogWarning("no serialization system was specified for data. Constructing dummy data only.");
        }
        
        events.OnClearSavedData += () => {
            SetDataGeneral(DataGeneral.Defaults());
        };
    }

    public DataGeneral GetDataGeneral()
    {
        return new DataGeneral(data);
    }

    public void SetDataGeneral(DataGeneral newData)
    {
        this.data = new DataGeneral(newData);
        events.OnDataChanged?.Invoke(data);
    }
}
