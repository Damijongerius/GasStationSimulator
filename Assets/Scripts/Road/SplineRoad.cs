using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

namespace Road
{
    [ExecuteInEditMode()]
    public class SplineRoad : MonoBehaviour
    {
        
        [SerializeField]
        private SplineSampler m_sampler;
        
        [SerializeField]
        private MeshFilter m_meshFileter;
        
        [SerializeField]
        private int resolution;
        
        List<Vector3> m_vertsP1;
        List<Vector3> m_vertsP2;

        private void OnEnable()
        {
            Spline.Changed += OnSplineChanged;
            
            GetVerts();
            BuildMesh();
        }
        
        private void OnDisable()
        {
            Spline.Changed -= OnSplineChanged;
        }

        private void OnSplineChanged(Spline spline, int index, SplineModification modification)
        {
            GetVerts();
            BuildMesh();
        }

        private void GetVerts()
        {
            m_vertsP1 = new List<Vector3>();
            m_vertsP2 = new List<Vector3>();
            
            float step = 1f / (float)resolution;

            for (int i = 0; i < resolution; i++)
            {
                float t = step * i;
                m_sampler.SampleSplineWidth(t, out Vector3 p1, out Vector3 p2);
                m_vertsP1.Add(p1);
                m_vertsP2.Add(p2);
            }
        }

        private void BuildMesh()
        {
            Mesh m = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            int offset = 0;
            
            int length = m_vertsP2.Count;
            
            for (int i = 1; i <= length; i++)
            {
                Vector3 p1 = m_vertsP1[i-1];
                Vector3 p2 = m_vertsP2[i-1];
                Vector3 p3;
                Vector3 p4;

                if (i == length)
                {
                    p3 = m_vertsP1[0];
                    p4 = m_vertsP2[0];
                }
                else
                {
                    p3 = m_vertsP1[i];
                    p4 = m_vertsP2[i];
                }

                offset = 4 * (i - 1);
                
                int t1 = offset + 0;
                int t2 = offset + 2;
                int t3 = offset + 3;
                
                int t4 = offset + 3;
                int t5 = offset + 1;
                int t6 = offset + 0;
                
                verts.AddRange(new List<Vector3> {p1,p2, p3, p4});
                tris.AddRange(new List<int> {t1, t2, t3, t4, t5, t6});
            }
            
            m.SetVertices(verts);
            m.SetTriangles(tris, 0);
            m_meshFileter.mesh = m;
        }

        private void OnDrawGizmos()
        {
            if (m_vertsP1 == null || m_vertsP2 == null)
                return;
            
            Gizmos.color = Color.red;
            for (int i = 0; i < m_vertsP1.Count - 1; i++)
            {
                Gizmos.DrawLine(m_vertsP1[i], m_vertsP1[i + 1]);
                Gizmos.DrawLine(m_vertsP2[i], m_vertsP2[i + 1]);
            }
        }
    }
}