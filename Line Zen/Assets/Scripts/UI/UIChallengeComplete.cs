using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChallengeComplete : MonoBehaviour
{
    public bool Visible => confirmationDialog.Visible;

    public UIConfirmationDialog confirmationDialog; // this dialog, abstractly
    public UIChallengeEntry challengeEntry; // re-used from level list
    public UIChallengeEntry challengeBest;
}
