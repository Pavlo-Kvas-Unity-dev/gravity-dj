using System;
using System.Collections.Generic;
using GravityDJ.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GravityDJ
{
    public class GravityController : MonoBehaviour
    {
        private Settings settings;

        private GravityStrengthSliderController gravityStrengthSliderController;

        [Inject] FieldController FieldController;
        
        [Inject] private Spawner spawner;
 
        private float gravityNorm = 0f;

        private readonly double gravityConstant = 6.67408E-11;

        private List<GravityForceFieldCircle> circles = new List<GravityForceFieldCircle>();
        
        private GravityForceFieldCircle.Factory circleFactory;
        
        private Movement movement;

        [Inject]
        void Init(GravityStrengthSliderController gravityStrengthSliderController, Settings settings, GravityForceFieldCircle.Factory circleFactory)
        {
            this.gravityStrengthSliderController = gravityStrengthSliderController;
            this.settings = settings;
            this.circleFactory = circleFactory;
        }

        private void Awake()
        {
            CreateCircles(); 
        }

        private void Start()
        {
            SetGravity(gravityNorm, true);
        }

        private void Update()
        {
            UpdateMovement();
        }

        private void CreateCircles()
        {
            for (float radius = settings.innerCircleRadius;
                radius < settings.outerCircleRadius; 
                radius += settings.circleRadiusStep)
            {
                var circle = circleFactory.Create();
                
                circle.Init(radius);
                circles.Add(circle);
            }
        }

        public void OnGravitySliderValueChanged(float gravitySliderValue)
        {
            SetGravity(gravitySliderValue, false);
        }

        private void SetGravity(float newGravitySliderValue, bool updateUI)
        {
            //check bounds
            newGravitySliderValue = Mathf.Clamp(newGravitySliderValue, -1, 1);

            if (updateUI)
            {
                gravityStrengthSliderController.UpdateValue(newGravitySliderValue);
            }

            gravityNorm = CalcGravityNorm(newGravitySliderValue);

            UpdateGravityVisuals();
        }

        private void UpdateGravityVisuals()
        {
            var pullingGravityColor = Color.green;
            var pushingGravityColor = Color.red;
            var noGravityColor = Color.white;
            var gravityColor = gravityNorm > 0 ? pullingGravityColor : pushingGravityColor;

            float minDistCoef = DistanceCoef(FieldController.FarthestFieldPointRadius);
            float maxDistCoef = DistanceCoef(settings.maxGravityRadius); //todo

            foreach (var circle in circles)
            {
                var distanceCoef = DistanceCoef(circle.Radius);

                float t = Mathf.InverseLerp(minDistCoef, maxDistCoef, distanceCoef);

                var color = Color.Lerp(noGravityColor, gravityColor, t);

                color.a = Mathf.Abs(gravityNorm);

                circle.SetColor(color);
            }
        }

        private float DistanceCoef(float dist)
        {
            return 1.0f / Mathf.Pow(dist, settings.gravitationFalloffCoef);
        }

        /// <summary>
        /// map gravity slider value range (-gravitySliderZeroThreshold, gravitySliderZeroThreshold) to zero
        /// </summary>
        /// <param name="sliderValue"></param>
        private float CalcGravityNorm(float sliderValue)
        {
            return Mathf.Sign(sliderValue) *
                   Mathf.InverseLerp(settings.gravitySliderZeroThreshold, 1, Mathf.Abs(sliderValue));
        }

        private void UpdateMovement()
        {
            if (movement == null) return;
            
            Vector2 ballsCenterOfMass = movement.transform.position;

            var toCenterVector = ((Vector2) transform.position - ballsCenterOfMass);
            
            float acceleration = CalcAcceleration(toCenterVector.magnitude);

            movement.ApplyVelocity(toCenterVector.normalized * acceleration * Time.deltaTime);
        }
        /// <summary>
        /// acceleration due to gravity g = GM/r2
        /// gravitational constant G = 6.67408 × 10-11 m3 kg-1 s-2
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        
        private float CalcAcceleration(float distance)
        {
            double G = gravityConstant * gravityNorm;

            float acc = (float) ( G * settings.M * DistanceCoef(distance));

            acc = Mathf.Clamp(
                acc, 
                settings.allowedAccelerationRange.x, 
                settings.allowedAccelerationRange.y);
            return acc;
        }

        public void Reset()
        {
            gravityStrengthSliderController.Reset();
        }

        public void SetMovement(Movement movement)
        {
            this.movement = movement;
        }

        [Serializable]
        public class Settings
        {
            public float circleRadiusStep = .5f;
            public float innerCircleRadius = 0.5f;
            public float outerCircleRadius = 10f;
            public double M = 500000;
            public float gravitationFalloffCoef = 1.5f;
            public Vector2 allowedAccelerationRange = new Vector2(-150, 150);
            public float gravitySliderZeroThreshold = .15f;
            public float maxGravityRadius = 1f;
        }
    }
}
