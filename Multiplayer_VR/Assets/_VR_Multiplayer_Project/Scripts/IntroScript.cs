using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public void StartNewSession()
    {
        ConnectionManager._instance.StartNewSession();
    }

    public void JoinExistingSession()
    {
        ConnectionManager._instance.JoinExistingSession();
    }
}
