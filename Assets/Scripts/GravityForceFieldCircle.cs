using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace GravityDJ
{
    [RequireComponent	(typeof(LineRenderer))]
    public class GravityForceFieldCircle : MonoBehaviour
    {
        [SerializeField] private int vertexCount = 40;
        [SerializeField] private float lineWidth = 0.05f;
        [SerializeField] private float radius;

        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private string lineRendererShader = "Sprites/Default";
        
        [SerializeField] private bool drawGizmos;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        void Awake()
        {
            Assert.IsNotNull(lineRenderer);
            lineRenderer.material = new Material(Shader.Find(lineRendererShader));
            
            SetWidth(lineWidth);
            lineRenderer.positionCount = vertexCount;
        }

        private void OnValidate()
        {
            SetWidth(lineWidth);
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
        
            float angleStep = GetAngleStep();
            float angle = 0f;

            Vector2 prevVertexPos = Vector2.zero;
            
            Vector2 circleCenter = transform.position;
            
            for (int i = 0; i < vertexCount + 1; i++)
            {
                Vector2 vertexOffset = CircleVertPosFromAngle(angle, Radius);

                var vertexPos = circleCenter + vertexOffset;
                
                Gizmos.DrawLine(prevVertexPos, vertexPos);

                prevVertexPos = vertexPos;
                angle += angleStep;
            }
        }

        public void Init(float radius)
        {
            Radius = radius;

            float angleStep = GetAngleStep();
            float angle = 0;

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                Vector2 vertexOffset = CircleVertPosFromAngle(angle, Radius);
                
                lineRenderer.SetPosition(i, vertexOffset);
                angle += angleStep;
            }
        }

        private float GetAngleStep()
        {
            return (2f * Mathf.PI) / vertexCount;
        }

        private Vector2 CircleVertPosFromAngle(float angle, float radius)
        {
            return radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private void SetWidth(float circleWidth)
        {
            lineRenderer.widthMultiplier = circleWidth;
        }

        public void SetColor(Color color)
        {
            lineRenderer.material.color = color;
            lineRenderer.endColor = color;
            lineRenderer.startColor = color;
        }

        public class Factory:PlaceholderFactory<GravityForceFieldCircle>
        {
        }
    }
}
