using System;
using UnityEngine;

namespace GravityDJ
{
    public class Ball : MonoBehaviour
    {
        public event Action<Ball> flyAway;
    
        public static int NumAgents { get; private set; } = 0;

        void Awake()
        {
            NumAgents++;
        }

        public void OnTargetHit()
        {
            NumAgents--;
            Destroy(this.gameObject);
            flyAway.Invoke(this);
        }
    }
}
