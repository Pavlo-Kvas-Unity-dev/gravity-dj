using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GravityDJ
{
    public class FieldController : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            public int fieldSize = 12;
            public float cellSize = 1f;
            public List<Vector2Int>  holeCoordList = new List<Vector2Int>();
            public Vector2 fieldCenter = new Vector2(0,0);
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
        private Boundary.Factory boundaryFactory;
        private Target.Factory targetFactory;

        public GameObject this[Vector2Int coord]
        {
            get { return field[coord.x, coord.y]; }
            set { field[coord.x, coord.y] = value; }      
        }

        public GameObject this[int x, int y]
        {
            get => field[x, y];
            set => field[x, y] = value;
        }
    
        [Inject]
        void Init(Settings settings, Boundary.Factory boundaryFactory, Target.Factory targetFactory)
        {
            this.settings = settings;
            this.boundaryFactory = boundaryFactory;
            this.targetFactory = targetFactory;
        }
    
        void Awake()
        {
            field = new GameObject[FieldSize,FieldSize];
        
            foreach (var holeCoord in settings.holeCoordList)
            {
                TrySpawnTarget(holeCoord);
            }

            for (int x = 0, y = 0; x < FieldSize && y < FieldSize; x++, y++)
            {
                TrySpawnBorderTile(x, FieldSize - 1);
                TrySpawnBorderTile(x, 0);
                TrySpawnBorderTile(0, y);
                TrySpawnBorderTile(FieldSize-1, y);
            }
        }

        private bool TrySpawnTarget(Vector2Int holeCoord)
        {
            if (this[holeCoord] != null) return false;
        
            var target = targetFactory.Create();
            target.transform.position = GetPosByFieldCoordinates(holeCoord);
            this[holeCoord] = target.gameObject;

            return true;
        }

        private bool TrySpawnBorderTile(int x, int y)
        {
            if (this[x, y] != null) return false;
        
            Boundary boundary = boundaryFactory.Create();
            boundary.transform.position = GetPosByFieldCoordinates(x, y);
        
            field[x,y] = boundary.gameObject;
        
            return true;
        }

        private Vector2 GetPosByFieldCoordinates(Vector2Int coord)
        {
            return GetPosByFieldCoordinates(coord.x, coord.y);
        }

        private Vector2 GetPosByFieldCoordinates(int x, int y)
        {
            return GetBottomLeftCoord() + Vector2.up *(y * CellSize) + Vector2.right*(x * CellSize);
        }

        private Vector2 GetBottomLeftCoord()
        {
            return settings.fieldCenter + (Vector2.down  + Vector2.left)*(FieldSize/2 - CellSize/2);
        }
    }
}
