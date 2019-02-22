using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject FlyingAgentPrefab;

    public float FieldSize = 5;
    private float FieldHalfSize => FieldSize / 2;

    public float MaxDistanceFromBoundary = 1;

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

        SpawnImmediate();
        yield break;
    }

    private void SpawnImmediate()
    {
        float objectRadius = FlyingAgentPrefab.GetComponentInChildren<CircleCollider2D>().radius;
        Assert.IsTrue(MaxDistanceFromBoundary > objectRadius);

        var xPos = Random.Range(0, FieldSize);
        var yPos = Random.Range(0, FieldSize);

        CheckBoundaries(ref xPos, objectRadius, FieldSize);
        CheckBoundaries(ref yPos, objectRadius, FieldSize);

        var spawnPos = new Vector2(xPos, yPos);
        spawnPos -= Vector2.one * FieldHalfSize;
        float minAllowedDistanceFromCenter = FieldHalfSize - MaxDistanceFromBoundary;
        float cappedMagnitude = Mathf.Max(spawnPos.magnitude, minAllowedDistanceFromCenter);
        spawnPos = spawnPos.normalized * cappedMagnitude;

        //spawn only if no Agent is on the screen
        var movement = Instantiate(FlyingAgentPrefab, spawnPos, Quaternion.identity).GetComponent<Movement>();
        movement.Init(InitialSpeed);
    }

    private void CheckBoundaries(ref float coord, float objectRadius, float fieldSize)
    {
        coord = Mathf.Max(coord, objectRadius);
        coord = Mathf.Min(coord, fieldSize - objectRadius);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }
}
