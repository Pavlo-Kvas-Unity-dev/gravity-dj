using System;
using UnityEngine;
using Zenject;

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

        public class Factory:PlaceholderFactory<Ball>
        {
        }
    }
}
