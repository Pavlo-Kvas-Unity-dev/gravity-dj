using UnityEngine;
using Zenject;

namespace GravityDJ
{
    public class Target : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var flyingAgent = other.gameObject.GetComponent<FlyingAgent>();
            if (flyingAgent == null) return;
            
            if (flyingAgent.isAlive)
            {
                flyingAgent.OnTargetHit();
            }
        }
        
        public class Factory:PlaceholderFactory<Target>//todo add UsedImplicitlyAttribute 
        {
        }
    }
}