using System;
using System.Collections;
using System.Collections.Generic;
using GravityDJ;
using UnityEngine;

public class FlyingAgent : MonoBehaviour
{
    public bool isAlive = true;
    private Movement movement;

    public delegate void OnAgentFlyThroughHoleEventHandler(FlyingAgent sender);

    public event OnAgentFlyThroughHoleEventHandler flyAway;
    
    public static int NumAgents { get; private set; } = 0;

    void Awake()
    {
        NumAgents++;
        movement = GetComponent<Movement>();
    }

    public void OnTargetHit()
    {
        NumAgents--;
        Destroy(this.gameObject);
        flyAway.Invoke(this);
    }

    public void ApplyVelocity(Vector2 deltaVelocity)
    {
        movement.ApplyVelocity(deltaVelocity);
    }
}
