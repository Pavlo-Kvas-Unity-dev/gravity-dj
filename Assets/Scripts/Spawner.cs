using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
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

    private GameController gameController;

    [Inject]
    public void Init(GameController gameController)
    {
        this.gameController = gameController;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsTrue(MaxDistanceFromBoundary > ObjectRadius);
    }

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
        float spawnableFieldSize = fieldController.FieldSize - 2 * fieldController.BorderSize;

        var xPos = RandomCoordInsideBoundaries();
        var yPos = RandomCoordInsideBoundaries();

        var spawnPos = new Vector2(xPos, yPos);

        spawnPos -= Vector2.one * spawnableFieldSize/2;

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

        float RandomCoordInsideBoundaries()
        {
            float coord = Random.Range(0f, spawnableFieldSize);
            return Mathf.Clamp(coord, ObjectRadius, spawnableFieldSize - ObjectRadius);
        }
    }

    private float MinAllowedDistanceFromCenter(float objectRadius)
    {
        return fieldController.FieldSize/2 - fieldController.BorderSize - MaxDistanceFromBoundary + objectRadius;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Spawn();
        }
    }
}
