using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgent : MonoBehaviour
{
    public delegate void OnAgentFlyThroughHoleEventHandler(FlyingAgent sender);

    public event OnAgentFlyThroughHoleEventHandler flyAway;
    
    public static int NumAgents { get; private set; } = 0;

    void Awake()
    {
        NumAgents++;
    }

    private void OnBecameInvisible()
    {
        NumAgents--;
        Destroy(this.gameObject);
        flyAway.Invoke(this);
    }
}
