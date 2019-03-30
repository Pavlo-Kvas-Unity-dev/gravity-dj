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
 
        private float gravityStrengthZeroShiftModified = 0f;

        private float gravityStrengthSliderValue;

        private readonly double gravityConstant = 6.67408E-11;

        private List<GravityForceFieldCircle> circles = new List<GravityForceFieldCircle>();
        private GravityForceFieldCircle.Factory circleFactory;


        public float GravityStrengthZeroShiftModified
        {
            set { gravityStrengthZeroShiftModified = value; }
            get { return gravityStrengthZeroShiftModified; }
        }

        public float GravityStrengthSliderValue
        {
            set { gravityStrengthSliderValue = value; }
            get { return gravityStrengthSliderValue; }
        }

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
            SetGravityStrength(GravityStrengthZeroShiftModified, true);
            
        }

        private void CreateCircles()
        {
            for (float curCircleRadius = settings.innerCircleRadius; curCircleRadius < settings.outerCircleRadius; curCircleRadius += .5f)
            {
                GravityForceFieldCircle circle = circleFactory.Create();

                circles.Add(circle);

                circle.Init(curCircleRadius);
            }
        }

        public void OnStrengthChanged(float gravityStrengthNorm)
        {
            SetGravityStrength(gravityStrengthNorm, false);
        }

        public void SetGravityStrength(float newGravityStrengthNorm, bool updateUI)
        {
            gravityStrengthZeroShiftModified = gravityStrengthSliderValue = Mathf.Clamp(newGravityStrengthNorm, settings.gravityStrengthRange.x, settings.gravityStrengthRange.y);
        
            //interpret near zero values as zero
            if (gravityStrengthZeroShiftModified > 0)
            {
                gravityStrengthZeroShiftModified = Mathf.InverseLerp(settings.gravitySliderZeroThreshold, settings.gravityStrengthRange.y, gravityStrengthZeroShiftModified);
            }
            else
            {
                gravityStrengthZeroShiftModified = -1 * Mathf.InverseLerp(-(float) settings.gravitySliderZeroThreshold, settings.gravityStrengthRange.x, gravityStrengthZeroShiftModified);
            }
        
            var posColor = Color.green;
            var negColor = Color.red;
            var neutralColor = Color.white;
            var targetColor = gravityStrengthZeroShiftModified > 0 ? posColor : negColor;

            var sliderColor = gravityStrengthZeroShiftModified > 0 ?
                Color.Lerp(neutralColor, posColor, gravityStrengthZeroShiftModified) : 
                Color.Lerp(neutralColor, negColor, Math.Abs(gravityStrengthZeroShiftModified));

            float minDist = 1f;
            float maxDist = Mathf.Sqrt(2)*(FieldController.FieldSize / 2 - FieldController.CellSize);
            float farthestDistCoef = CalcDistanceCoef(maxDist);
            float closestDistCoef  = CalcDistanceCoef(minDist);


            float CalcDistanceCoef(float dist)
            {
                return 1.0f/Mathf.Pow(dist, settings.gravitationFalloffCoef);
            }

            foreach (var circle in circles)
            {
                var distanceCoef = CalcDistanceCoef(circle.Radius);
            
                float t = Mathf.InverseLerp(farthestDistCoef, closestDistCoef, distanceCoef);

                var circleColor = Color.Lerp(neutralColor, targetColor, t);
            
                circleColor.a = Mathf.Abs(gravityStrengthZeroShiftModified);
            
                circle.SetColor(circleColor);
            }
        
            if (updateUI)
            {
                gravityStrengthSliderController.UpdateValue(GravityStrengthSliderValue);
            }
        }

        private float CalcAcceleration(float distance)
        {
            float acceleration = (float) (gravityConstant * settings.M * gravityStrengthZeroShiftModified /
                                          (Mathf.Pow(distance, settings.gravitationFalloffCoef)));

            acceleration = Mathf.Clamp(acceleration, settings.allowedAccelerationRange.x, settings.allowedAccelerationRange.y);
            return acceleration;
        }

        private void Update()
        {
            //read input for changing gravity extents
            var hor = Input.GetAxis("Horizontal");
        
            //read input for changing gravity density
            var vert = Input.GetAxis("Vertical");
            if (Mathf.Abs(vert) > Mathf.Epsilon)
            {
                var deltaStrength = Time.deltaTime * settings.gravityStrengthChangeSpeed * (vert > 0 ? 1 : -1 );
                SetGravityStrength(gravityStrengthSliderValue+deltaStrength, true);
                Debug.Log(deltaStrength);
            }
        
            UpdateAgent();
        }

        private void UpdateAgent()
        {
            if (spawner.TryGetAgent(out FlyingAgent agent))
            {
                Debug.Log("gravity contact");
                //add pulling force

                Vector2 agentsCenterOfMass = agent.transform.position;
                //acceleration due to gravity g = GM/r2
                var distance = ((Vector2) transform.position - agentsCenterOfMass);
                //gravitational constant G = 6.67408 × 10-11 m3 kg-1 s-2
                float acceleration = CalcAcceleration(distance.magnitude);
                Debug.Log(acceleration);

                agent.ApplyVelocity((distance.normalized * acceleration * Time.deltaTime));
            }
        }

        public void Reset()
        {
            gravityStrengthSliderController.Reset();
        }

        [Serializable]
        public class Settings
        {
            public float innerCircleRadius = 0.5f;
            public float outerCircleRadius = 10f;
            public float gravityStrengthChangeSpeed = 1.4f;
            public double M = 500000;
            public float gravitationFalloffCoef = 1.5f;
            public Vector2 allowedAccelerationRange = new Vector2(-150, 150);
            public Vector2 gravityStrengthRange = new Vector2(-1, 1);
            public float gravitySliderZeroThreshold = .15f;
        }
    }
}
