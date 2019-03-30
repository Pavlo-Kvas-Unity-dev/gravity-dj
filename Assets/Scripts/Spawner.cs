using System;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GravityDJ
{
    public class Spawner : MonoBehaviour
    {
        [Inject] FieldController fieldController;

        private float ObjectRadius => 0.5f;//todo

        private GameController gameController;
        private Settings settings;
        private Ball ball;
        private GravityController gravityController;
        private Ball.Factory ballFactory;


        [Inject]
        public void Init(GameController gameController, Settings settings, GravityController gravityController, Ball.Factory ballFactory)
        {
            this.gameController = gameController;
            this.settings = settings;
            this.gravityController = gravityController;
            this.ballFactory = ballFactory;
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

            ball = ballFactory.Create();
            ball.transform.position = spawnPos;


            ball.targetHit += (Ball sender) =>
            {
                gameController.OnTargetHit(sender);
                Spawn();
            };
            
            var movement = ball.GetComponent<Movement>();

            movement.Init(settings.InitialSpeed);
            gravityController.SetMovement(movement);
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
            public float maxDistanceFromBoundary = 1;
            public int InitialSpeed = 5;
        }
    }
}
