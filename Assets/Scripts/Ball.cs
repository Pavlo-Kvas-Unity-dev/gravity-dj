using System;
using UnityEngine;

namespace GravityDJ
{
    public class Ball : MonoBehaviour
    {
        private Movement movement;
        
        public event Action<Ball> flyAway;
    
        public static int NumAgents { get; private set; } = 0;

        void Awake()
        {
            NumAgents++;
            movement = GetComponent<Movement>();
        }

        public void OnTargetHit()
        {
            NumAgents--;
            Destroy(this.gameObject);
            flyAway.Invoke(this);
        }

        public void ApplyVelocity(Vector2 deltaVelocity)
        {
            movement.ApplyVelocity(deltaVelocity);
        }
    }
}
