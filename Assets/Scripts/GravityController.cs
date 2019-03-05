using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GravityController : MonoBehaviour
{
    public FieldController FieldController;
    
    [SerializeField] private Slider gravitySizeSlider;
    
    [SerializeField] private Slider gravityStrengthSlider;
    
    private float initialSizeNorm = .5f;

    public double M = 500000;
    /// <summary>
    /// the value is in range (0,1)
    /// </summary>
    private float gravityStrengthNorm = 1.0f;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D collider;
    public float ChangeSizeSensitivity = 1.5f;
    [SerializeField] private float gravityStrengthChangeSpeed;
    public int gravitationFalloffCoef = 2;
    public bool capAcceleration;
    public float maxAcceleration = 50;

    private void Awake()
    {
        collider = GetComponentInChildren<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
    }

    private void Start()
    {
        gravitySizeSlider.value = initialSizeNorm;
        
        gravityStrengthSlider.value = gravityStrengthNorm;
    }

    public void OnStrengthChanged(float gravityStrengthNorm)
    {
        SetGravityStrength(gravityStrengthNorm, false);
    }
    
    public void SetGravityStrength(float gravityStrengthNorm, bool updateUI)
    {
        this.gravityStrengthNorm = gravityStrengthNorm = Mathf.Clamp01(gravityStrengthNorm);
        
        var color = spriteRenderer.color;
        color.a = gravityStrengthNorm;
        spriteRenderer.color = color;
        
        if (updateUI)
        {
            gravityStrengthSlider.value = gravityStrengthNorm;
        }
    }

    public void OnSizeChanged(float sizeNorm)
    {
        SetSize(sizeNorm, false);
    }
    
    public void SetSize(float sizeNorm, bool updateUI)
    {
        collider.transform.localScale = FieldController.FieldSize * sizeNorm * Vector3.one;
        
        if (updateUI)
        {
            gravitySizeSlider.value = sizeNorm;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("gravity contact");
        //add pulling force
        var agentsCenterOfMass = collision.attachedRigidbody.worldCenterOfMass;
        //acceleration due to gravity g = GM/r2
        var distance = ((Vector2)transform.position - agentsCenterOfMass);
        //gravitational constant G = 6.67408 × 10-11 m3 kg-1 s-2
        double G = 6.67408E-11;
        double acceleration = G * M * gravityStrengthNorm / (Mathf.Pow(distance.magnitude,gravitationFalloffCoef));
        if (capAcceleration)
        {
            acceleration = Math.Min(acceleration, maxAcceleration);
        }
        Debug.Log(acceleration);
        
        collision.attachedRigidbody.velocity += (distance.normalized * (float)acceleration * Time.deltaTime);
    }

    private void Update()
    {
        //read input for changing gravity extents
        var hor = Input.GetAxis("Horizontal");
        if (Math.Abs(hor) > Mathf.Epsilon)
        {
            float sizeNorm = GetSizeNorm() + hor * ChangeSizeSensitivity * Time.deltaTime;
             
            sizeNorm = Mathf.Clamp01(sizeNorm);
            SetSize(sizeNorm, true);
        }
        
        //read input for changing gravity density
        var vert = Input.GetAxis("Vertical");
        if (Mathf.Abs(vert) > Mathf.Epsilon)
        {
            var deltaStrength = Time.deltaTime * gravityStrengthChangeSpeed * (vert > 0 ? 1 : -1 );
            SetGravityStrength(gravityStrengthNorm+deltaStrength, true);
            Debug.Log(deltaStrength);
        }
    }

    private float GetSizeNorm() => collider.transform.localScale.x / FieldController.FieldSize;
}
