using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FieldController : MonoBehaviour
{
    [Inject(Id="boundaryParent")] private Transform boundaryParent;

    [Inject(Id="fieldCenter")] private Transform spawnCenter;

    [Serializable]
    public class Settings
    {
        public GameObject boundaryPrefab;
        
        public int fieldSize = 12;
        public float cellSize = 1f;
        public List<Vector2Int>  holeCoordList = new List<Vector2Int>();
    }


    public float BorderSize => CellSize;


    public int FieldSize//todo inline
    {
        get { return settings.fieldSize; }
        set { settings.fieldSize = value; }
    }

    public float CellSize
    {
        get { return settings.cellSize; }
        set { settings.cellSize = value; }
    }

    private GameObject[,] field;

    private Settings settings;
    
    public GameObject this[Vector2Int coord]
    {
        get { return field[coord.x, coord.y]; }
        set { field[coord.x, coord.y] = value; }      
    }

    [Inject]
    void Init(Settings settings)
    {
        this.settings = settings;
    }
    
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

        foreach (var holeCoord in settings.holeCoordList)
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
        var boundaryTile = Instantiate(settings.boundaryPrefab, GetPosByFieldCoordinates(x, y), Quaternion.identity);
        boundaryTile.transform.parent = boundaryParent;
        field[x,y] = boundaryTile;
    }

    private Vector2 GetPosByFieldCoordinates(int x, int y)
    {
        return GetBottomLeftCoord() + Vector2.up *(y * CellSize) + Vector2.right*(x * CellSize);
    }

    private Vector2 GetBottomLeftCoord()
    {
        return (Vector2)spawnCenter.transform.position + (Vector2.down  + Vector2.left)*(FieldSize/2 - CellSize/2);
    }
}
