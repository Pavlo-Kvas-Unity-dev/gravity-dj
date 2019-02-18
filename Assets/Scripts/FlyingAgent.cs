using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgent : MonoBehaviour
{
    public static int NumAgents { get; private set; } = 0;

    void Awake()
    {
        NumAgents++;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        NumAgents--;
        GameObject.FindObjectOfType<Spawner>().Spawn(); //todo
    }
}
