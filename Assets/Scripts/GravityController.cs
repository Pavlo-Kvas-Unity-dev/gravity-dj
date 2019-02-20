using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public double M = 500000;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("gravity contact");
        //add pulling force
        var agentsCenterOfMass = collision.attachedRigidbody.worldCenterOfMass;
        //acceleration due to gravity g = GM/r2
        var distance = ((Vector2)transform.position - agentsCenterOfMass);
        //gravitational constant G = 6.67408 × 10-11 m3 kg-1 s-2
        double G = 6.67408E-11;
        double acceleration = G * M / (distance.magnitude * distance.magnitude);
        collision.attachedRigidbody.velocity += (distance.normalized * (float)acceleration * Time.deltaTime);
    }

}
