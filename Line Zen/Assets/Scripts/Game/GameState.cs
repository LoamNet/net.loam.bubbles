using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GameState
{
    Startup = 0,
    TutorialOne,
    TutorialTwo,
    Options,
    GameLoadLevel,
    GameUnlimited,
    Exit
}

