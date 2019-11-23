using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public Camera sceneCam;

    public DataPoint PrimaryInputPosWorld()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = -sceneCam.transform.position.z;
        Vector3 world = sceneCam.ScreenToWorldPoint(screen);

        return new DataPoint(world.x, world.y);
    }

    public bool PrimaryInputDown()
    {
        if(Input.GetMouseButton(0))
        {
            return true;
        }

        return false;
    }
}
