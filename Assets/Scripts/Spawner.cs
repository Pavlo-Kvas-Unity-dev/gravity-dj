using System;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace GravityDJ
{
    public class Spawner : MonoBehaviour
    {
        [Inject] FieldController fieldController;

        private float ObjectRadius
        {
            get
            {
                if (agentsCircleCollider2D == null)
                {
                    agentsCircleCollider2D = settings.flyingAgentPrefab.GetComponentInChildren<CircleCollider2D>();
                }
                return agentsCircleCollider2D.radius;
            }
        }

        CircleCollider2D agentsCircleCollider2D;

        private GameController gameController;
        private Settings settings;
        private Ball ball;


        [Inject]
        public void Init(GameController gameController, Settings settings)
        {
            this.gameController = gameController;
            this.settings = settings;
        }

        // Start is called before the first frame update

        void Awake()
        {
            Assert.IsTrue(settings.maxDistanceFromBoundary > ObjectRadius);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                DrawSpawnBoundary();
            }
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

            //only one ball at time
            if (ball != null)
            {
                Destroy(ball.gameObject);
            }

            //spawn only if no Agent is on the screen

            var ballGO = Instantiate(settings.flyingAgentPrefab, spawnPos, Quaternion.identity);//todo use factory

            var movement = ballGO.GetComponent<Movement>();

            ball = ballGO.GetComponent<Ball>();
       
            ball.flyAway += (Ball sender) =>
            {
                gameController.OnTargetHit(sender);
                Spawn();
            };

            movement.Init(settings.InitialSpeed);

            float RandomCoordInsideBoundaries()
            {
                float coord = Random.Range(0f, spawnableFieldSize);
                return Mathf.Clamp(coord, ObjectRadius, spawnableFieldSize - ObjectRadius);
            }
        }

        private float MinAllowedDistanceFromCenter(float objectRadius)
        {
            return fieldController.FieldSize/2 - fieldController.BorderSize - settings.maxDistanceFromBoundary + objectRadius;
        }

        void Update()
        {
        
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Spawn();
            }
        }

        [Serializable]
        public class Settings
        {
            public GameObject flyingAgentPrefab;
            public float maxDistanceFromBoundary = 1;
            public int InitialSpeed = 5;
            public bool SpawnOnStart = true;
        }

        public bool TryGetBall(out Ball ball)
        {
            if (this.ball != null)
            {
                ball = this.ball;
                return true;
            }
            else
            {
                ball = null;
                return false;
            }
        
        }
    }
}
