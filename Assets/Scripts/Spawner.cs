using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject FlyingAgentPrefab;
    public FieldController fieldController;

    private float ObjectRadius
    {
        get
        {
            if (agentsCircleCollider2D == null)
            {
                agentsCircleCollider2D = FlyingAgentPrefab.GetComponentInChildren<CircleCollider2D>();
            }
            return agentsCircleCollider2D.radius;
        }
    }

     

    public float MaxDistanceFromBoundary = 1;

    public Transform SpawnCenter;
    public int InitialSpeed = 5;
    public bool SpawnOnStart = true;
    CircleCollider2D agentsCircleCollider2D;

    [SerializeField] private GameController gameController;

    // Start is called before the first frame update


    void Start()
    {
        if (SpawnOnStart)
        {
            Spawn();
        }
    }

    private void OnDrawGizmos()
    {
        DrawSpawnBoundary();
    }

    private void DrawSpawnBoundary()
    {
        var prevColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, MinAllowedDistanceFromCenter(ObjectRadius));
        Gizmos.color = prevColor;
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

    private float MinAllowedDistanceFromCenter(float objectRadius)
    {
        return fieldController.FieldSize/2 - fieldController.BorderSize - MaxDistanceFromBoundary + objectRadius;
    }

    private void SpawnImmediate()
    {
        Assert.IsTrue(MaxDistanceFromBoundary > ObjectRadius);

        var xPos = Random.Range(0f, fieldController.FieldSize);
        var yPos = Random.Range(0f, fieldController.FieldSize);

        CheckBoundaries(ref xPos, ObjectRadius, fieldController.FieldSize);
        CheckBoundaries(ref yPos, ObjectRadius, fieldController.FieldSize);

        var spawnPos = new Vector2(xPos, yPos);
        spawnPos -= Vector2.one * (fieldController.FieldSize/2 - fieldController.BorderSize);
        float cappedMagnitude = Mathf.Max(spawnPos.magnitude, MinAllowedDistanceFromCenter(ObjectRadius) + ObjectRadius);
        spawnPos = spawnPos.normalized * cappedMagnitude;

        //spawn only if no Agent is on the screen
        var flyingAgentGO = Instantiate(FlyingAgentPrefab, spawnPos, Quaternion.identity);
        var movement = flyingAgentGO.GetComponent<Movement>();
        var flyingAgent = flyingAgentGO.GetComponent<FlyingAgent>();

        flyingAgent.flyAway += new FlyingAgent.OnAgentFlyThroughHoleEventHandler(sender =>
        {
            gameController.OnAgentFlewAway(sender);
            Spawn();
        });
        movement.Init(InitialSpeed);
    }

    private void CheckBoundaries(ref float coord, float objectRadius, int fieldSize)
    {
        coord = Mathf.Max(coord, objectRadius);
        coord = Mathf.Min(coord, fieldSize - objectRadius);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Spawn();
        }
    }
}
