using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgent : MonoBehaviour
{
    public delegate void OnAgentFlyThroughHoleEventHandler(FlyingAgent sender);

    public event OnAgentFlyThroughHoleEventHandler flyAway;
    
    public static int NumAgents { get; private set; } = 0;

    private bool isQuitting = false;

    void Awake()
    {
        NumAgents++;
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnBecameInvisible()
    {
        if (isQuitting) return;
       
        NumAgents--;
        Destroy(this.gameObject);
        flyAway.Invoke(this);
    }
}
