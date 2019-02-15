using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public void Init(int initialSpeed)
    {
        var rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = Random.insideUnitCircle * initialSpeed;
    }
}
