using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public void LeaveSession()
    {
        ConnectionManager._instance.LeaveSession();
    }
}
