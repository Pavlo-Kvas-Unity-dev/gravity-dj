using System;
using UnityEngine;
using UnityEngine.UI;

public class GravityController : MonoBehaviour
{
    public FieldController FieldController;

    [SerializeField] private Slider gravityStrengthSlider;

    public double M = 500000;
    
    private float gravityStrengthZeroShiftModified = 0f;
    private float gravityStrengthSliderValue;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D collider;
    [SerializeField] private float gravityStrengthChangeSpeed;
    public float gravitationFalloffCoef = 2;
    public bool capAcceleration;
    [SerializeField] private Vector2 gravityStrengthRange;
    [SerializeField] private Vector2 allowedAccelerationRange;
    private readonly double gravityConstant = 6.67408E-11;
    [SerializeField] private float gravitySliderZeroThreshold = .15f;

    private void Awake()
    {
        collider = GetComponentInChildren<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        InitSliders();
    }

    private void InitSliders()
    {
        InitSlider(gravityStrengthSlider, gravityStrengthRange);
        SetGravityStrength(gravityStrengthZeroShiftModified, true);

        void InitSlider(Slider slider, Vector2 allowedValueRange)
        {
            if (Math.Abs(slider.minValue - allowedValueRange.x) > Mathf.Epsilon ||
                Math.Abs(slider.maxValue - allowedValueRange.y) > Mathf.Epsilon)
            {
                Debug.LogWarning($"slider's min/max values on {slider.gameObject.name} doesn't match settings, fixing it");
                slider.minValue = allowedValueRange.x;
                slider.maxValue = allowedValueRange.y;
            }
        }
    }

    public void OnStrengthChanged(float gravityStrengthNorm)
    {
        SetGravityStrength(gravityStrengthNorm, false);
    }
    
    public void SetGravityStrength(float newGravityStrengthNorm, bool updateUI)
    {
        gravityStrengthZeroShiftModified = gravityStrengthSliderValue = Mathf.Clamp(newGravityStrengthNorm, gravityStrengthRange.x, gravityStrengthRange.y);
        
        //interpret near zero values as zero
        if (gravityStrengthZeroShiftModified > 0)
        {
            gravityStrengthZeroShiftModified = Mathf.InverseLerp(gravitySliderZeroThreshold, gravityStrengthRange.y, gravityStrengthZeroShiftModified);
        }
        else
        {
            gravityStrengthZeroShiftModified = -1 * Mathf.InverseLerp(-gravitySliderZeroThreshold, gravityStrengthRange.x, gravityStrengthZeroShiftModified);
        }
        
        var posColor = Color.green;
        var negColor = Color.red;
        var neutralColor = Color.gray;

        var color = gravityStrengthZeroShiftModified > 0 ?
            Color.Lerp(neutralColor, posColor, gravityStrengthZeroShiftModified) : 
            Color.Lerp(neutralColor, negColor, Math.Abs(gravityStrengthZeroShiftModified));
        
        spriteRenderer.color = color;
        
        if (updateUI)
        {
            gravityStrengthSlider.value = gravityStrengthSliderValue;
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
        float acceleration = (float)(gravityConstant * M * gravityStrengthZeroShiftModified / (Mathf.Pow(distance.magnitude,gravitationFalloffCoef)));
        if (capAcceleration)
        {
            acceleration = Mathf.Clamp(acceleration, allowedAccelerationRange.x, allowedAccelerationRange.y);
        }
        Debug.Log(acceleration);
        
        collision.attachedRigidbody.velocity += (distance.normalized * (float)acceleration * Time.deltaTime);
    }

    private void Update()
    {
        //read input for changing gravity extents
        var hor = Input.GetAxis("Horizontal");
        
        //read input for changing gravity density
        var vert = Input.GetAxis("Vertical");
        if (Mathf.Abs(vert) > Mathf.Epsilon)
        {
            var deltaStrength = Time.deltaTime * gravityStrengthChangeSpeed * (vert > 0 ? 1 : -1 );
            SetGravityStrength(gravityStrengthSliderValue+deltaStrength, true);
            Debug.Log(deltaStrength);
        }
    }
}
