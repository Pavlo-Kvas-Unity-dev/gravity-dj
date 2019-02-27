using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public int FieldSize = 12;
    [SerializeField] private GameObject boundaryPrefab;

    [SerializeField] private Transform spawnCenter;

    [SerializeField] private float cellSize = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        //spawn upper and lower boundaries
        for (int x = 0; x < FieldSize; x++)
        {
            
            Instantiate(boundaryPrefab, GetPosByFieldCoordinates(x, FieldSize - 1), Quaternion.identity);
            Instantiate(boundaryPrefab, GetPosByFieldCoordinates(x, 0), Quaternion.identity);
        }
        
        //spawn left and right boundaries
        for (int y = 0; y < FieldSize; y++)
        {
            Instantiate(boundaryPrefab, GetPosByFieldCoordinates(0, y), Quaternion.identity);
            Instantiate(boundaryPrefab, GetPosByFieldCoordinates(FieldSize - 1, y), Quaternion.identity);
        }
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
