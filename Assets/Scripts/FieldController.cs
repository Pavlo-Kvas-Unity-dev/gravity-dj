using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public int FieldSize = 12;
    [SerializeField] private GameObject boundaryPrefab;

    [SerializeField] private Transform spawnCenter;

    [SerializeField] private float cellSize = 1f;
    [SerializeField] private List<Vector2Int>  holeCoordList = new List<Vector2Int>();
    private GameObject[,] field;

    public GameObject this[Vector2Int coord]
    {
        get { return field[coord.x, coord.y]; }
        set { field[coord.x, coord.y] = value; }      
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        field = new GameObject[FieldSize,FieldSize];
        for (int x = 0, y = 0; x < FieldSize && y < FieldSize; x++, y++)
        {
            SpawnBorderTile(x, FieldSize - 1);
            SpawnBorderTile(x, 0);
            SpawnBorderTile(0, y);
            SpawnBorderTile(FieldSize-1, y);
        }

        foreach (var holeCoord in holeCoordList)
        {
            var borderTile = this[holeCoord];
            if (borderTile != null)
            {
                Destroy(borderTile);   
            }
        }
    }

    private void SpawnBorderTile(int x, int y)
    {
        field[x,y] = Instantiate(boundaryPrefab, GetPosByFieldCoordinates(x, y), Quaternion.identity);
    }

    private Vector2 GetPosByFieldCoordinates(int x, int y)
    {
        return GetBottomLeftCoord() + Vector2.up *(y * cellSize) + Vector2.right*(x * cellSize);
    }

    private Vector2 GetBottomLeftCoord()
    {
        return (Vector2)spawnCenter.transform.position + (Vector2.down  + Vector2.left)*(FieldSize/2 - cellSize/2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
