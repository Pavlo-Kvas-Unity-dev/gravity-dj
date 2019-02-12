using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject FlyingAgentPrefab;

    public float SpawnRadius;

    public Transform SpawnCenter;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        var spawnPos = (Vector2)SpawnCenter.position + Random.insideUnitCircle * SpawnRadius;

        Instantiate(FlyingAgentPrefab, spawnPos, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }
}
