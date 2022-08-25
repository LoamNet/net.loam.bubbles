using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Data : MonoBehaviour
{
    public Events events;
    public SerializedDataIO dataIO;
    public UIConfirmationDialog confirmationDialog;

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
            confirmationDialog.Display(
            () => {
                int savedSeed = dataIO.GetData().seed;
                DataGeneral resetData = DataGeneral.Defaults();
                resetData.seed = savedSeed;
                SetDataGeneral(resetData);
            },
            () => {
                // Do nothing on cancel.
            }, null);
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
