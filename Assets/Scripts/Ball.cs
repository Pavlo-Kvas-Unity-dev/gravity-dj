using System;
using UnityEngine;
using Zenject;

namespace GravityDJ
{
    public class Ball : MonoBehaviour
    {
        private bool isTargetHit = false;
        
        public event Action targetHit;

        public void OnTargetHit()
        {
            if (isTargetHit) return;

            isTargetHit = true;
            Destroy(this.gameObject);
            targetHit.Invoke();
        }

        public class Factory:PlaceholderFactory<Ball>
        {
        }
    }
}
