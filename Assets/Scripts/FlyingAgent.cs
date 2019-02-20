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
        NumAgents--;
        Destroy(this.gameObject);

        var spawner = GameObject.FindObjectOfType<Spawner>();
        spawner?.Spawn();
    }
}
