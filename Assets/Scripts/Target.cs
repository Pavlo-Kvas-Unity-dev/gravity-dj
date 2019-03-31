using System.Text;
using UnityEngine;
using Zenject;

namespace GravityDJ
{
    public class Target : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var ball = other.gameObject.GetComponent<Ball>();
            if (ball == null) return;
            
            ball.OnTargetHit();
        }
        
        public class Factory:PlaceholderFactory<Target>//todo add UsedImplicitlyAttribute 
        {
        }
    }
}