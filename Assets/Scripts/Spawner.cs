using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.XR.Interaction;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GravityDJ
{
    public class Spawner : MonoBehaviour
    {
        public event Action<Ball> ballSpawned;
        [Inject] FieldController fieldController;


        private GameController gameController;
        private Settings settings;
        private Ball ball;
        private GravityController gravityController;
        private Ball.Factory ballFactory;

        private float ObjectRadius => settings.objectRadius;

        [Inject]
        public void Init(GameController gameController, Settings settings, GravityController gravityController, Ball.Factory ballFactory)
        {
            this.gameController = gameController;
            this.settings = settings;
            this.gravityController = gravityController;
            this.ballFactory = ballFactory;
        }

        void Awake()
        {
            Assert.IsTrue(settings.maxDistanceFromBoundary > ObjectRadius);
        }

        void Update()
        {
            #if UNITY_EDITOR
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Spawn();
            }
            
            #endif
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
            //only one ball at time
            if (ball != null)
            {
                Destroy(ball.gameObject);
            }

            ball = ballFactory.Create();
            ball.transform.position = RandomSpawnPos();
            ball.targetHit += Spawn;
            
            var movement = ball.GetComponent<Movement>();

            movement.Init(settings.InitialSpeed);
            
            ballSpawned.Invoke(ball);
        }

        private Vector2 RandomSpawnPos()
        {
            float movableFieldSize = fieldController.MovableFieldSize;

            var xPos = RandomCoordInsideBoundaries(movableFieldSize);

            var yPos = RandomCoordInsideBoundaries(movableFieldSize);

            float RandomCoordInsideBoundaries(float coordRange)
            {
                float coord = Random.Range(0f, coordRange);
                return Mathf.Clamp(coord, ObjectRadius, coordRange - ObjectRadius);
            }

            var spawnPos = new Vector2(xPos, yPos);

            spawnPos -= Vector2.one * movableFieldSize / 2;

            float cappedMagnitude = Mathf.Max(spawnPos.magnitude, MinAllowedDistanceFromCenter(ObjectRadius) + ObjectRadius);

            spawnPos = spawnPos.normalized * cappedMagnitude;

            return spawnPos;
        }

        private float MinAllowedDistanceFromCenter(float objectRadius)
        {
            return fieldController.MovableFieldSize/2 - settings.maxDistanceFromBoundary + objectRadius;
        }

        [Serializable]
        public class Settings
        {
            public float objectRadius = 0.5f;
            public float maxDistanceFromBoundary = 1;
            public int InitialSpeed = 5;
        }
    }
}
