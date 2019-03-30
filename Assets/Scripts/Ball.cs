using System;
using UnityEngine;

namespace GravityDJ
{
    public class Ball : MonoBehaviour
    {
        public event Action<Ball> targetHit;

        public void OnTargetHit()
        {
            Destroy(this.gameObject);
            targetHit.Invoke(this);
        }
    }
}
