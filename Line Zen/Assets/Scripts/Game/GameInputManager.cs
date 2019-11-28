using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public Events events;
    public Camera sceneCam;

    public DataPoint PrimaryInputPosWorld()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = -sceneCam.transform.position.z;
        Vector3 world = sceneCam.ScreenToWorldPoint(screen);
        
        return new DataPoint(world.x, world.y);
    }

    public DataPoint ConvertToScreenPoint(DataPoint worldPoint)
    {
        Vector3 world = worldPoint;
        world.z = 0;
        Vector3 screen = sceneCam.WorldToScreenPoint(world);

        return new DataPoint(screen.x, screen.y);
    }

    public DataPoint ScreenSizeWorld()
    {
        Vector2 screen = new Vector2(Screen.width, Screen.height);
        Vector3 world = sceneCam.ScreenToWorldPoint(screen);

        return new DataPoint(world.x, world.y);
    }

    public bool PrimaryInputPressed()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        return false;
    }

    public bool PrimaryInputDown()
    {
        if(Input.GetMouseButton(0))
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        if(PrimaryInputPressed())
        {
            events.OnClick?.Invoke(new DataPoint(Input.mousePosition.x, Input.mousePosition.y));
        }
    }
}
