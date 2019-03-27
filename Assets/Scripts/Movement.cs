using UnityEngine;

namespace GravityDJ
{
    public class Movement : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
    
        public void Init(int initialSpeed)
        {
            rb2D.velocity = Random.insideUnitCircle * initialSpeed;
        }
    
        public void ApplyVelocity(Vector2 deltaVelocity)
        {
            rb2D.velocity += deltaVelocity;
        }
    
    }
}
