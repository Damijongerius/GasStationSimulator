using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Road
{
    [ExecuteInEditMode()]
    public class SplineSampler : MonoBehaviour
    {
        [SerializeField] 
        private SplineContainer m_splineContainer;

        [SerializeField] private int m_splineIndex;

        [SerializeField]
        [Range(0f,1f)]
        private float m_time;
        
        [SerializeField]
        private float m_width;

        float3 position;
        float3 tangent;
        float3 upVector;
        
        public void SampleSplineWidth(float time, out Vector3 p1, out Vector3 p2)
        {
            m_splineContainer.Evaluate(m_splineIndex, time, out position, out tangent, out upVector);
            
            float3 right = Vector3.Cross(tangent, upVector).normalized;
            p1 = position + (right * m_width) ;
            p2 = position + (-right * m_width);
        }
    }
}