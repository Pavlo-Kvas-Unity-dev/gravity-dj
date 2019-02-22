using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityController : MonoBehaviour
{
    public FieldController FieldController;
    public Slider gravitySizeSlider;
    private float initialSizeNorm = .5f;

    public double M = 500000;
    private float gravityStrengthCoef = 1.0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gravitySizeSlider.onValueChanged.AddListener(SetSize);
        gravitySizeSlider.value = initialSizeNorm;
    }

    public void OnGravityStrengthChanged(float gravityStrengthCoef)
    {
        this.gravityStrengthCoef = gravityStrengthCoef;
        var spriteRendererColor = spriteRenderer.color;
        spriteRendererColor.a = gravityStrengthCoef;
        spriteRenderer.color = spriteRendererColor;
    }

    public void SetSize(float sizeNorm)
    {
        var collider = GetComponentInChildren<CircleCollider2D>();
        collider.transform.localScale = FieldController.FieldSize * sizeNorm * Vector3.one;
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
        double acceleration = G * M * gravityStrengthCoef / (distance.magnitude * distance.magnitude);
        collision.attachedRigidbody.velocity += (distance.normalized * (float)acceleration * Time.deltaTime);
    }

}
