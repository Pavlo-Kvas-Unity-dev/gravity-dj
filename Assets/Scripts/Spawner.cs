using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject FlyingAgentPrefab;

    public float SpawnRadius;

    public Transform SpawnCenter;
    public int InitialSpeed = 5;
    public bool SpawnOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (SpawnOnStart)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return null;

        //spawn only if no Agent is on the screen
        var spawnPos = (Vector2) SpawnCenter.position + Random.insideUnitCircle * SpawnRadius;

        var movement = Instantiate(FlyingAgentPrefab, spawnPos, Quaternion.identity).GetComponent<Movement>();
        movement.Init(InitialSpeed);
        yield break;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }
}
