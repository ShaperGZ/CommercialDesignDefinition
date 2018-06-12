using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Geometry
{
    public class MeshMaker
    {
        public static Mesh joinMeshes(Mesh[] meshs, bool mergeSubMesh = false)
        {
            Mesh outMesh = new Mesh();
            List<Vector3> pts = new List<Vector3>();
            List<int> tris = new List<int>();
            int vertsCount = 0;

            foreach (Mesh mesh in meshs)
            {
                vertsCount = pts.Count;
                foreach (Vector3 p in mesh.vertices)
                {
                    pts.Add(p);
                }
                List<int> mtris = new List<int>();
                foreach (int t in mesh.triangles)
                {
                    int nt = t + vertsCount;
                    tris.Add(nt);
                    mtris.Add(nt);

                }

            }

            outMesh.vertices = pts.ToArray();
            outMesh.triangles = tris.ToArray();
            outMesh.RecalculateNormals();

            return outMesh;
        }
        public static Mesh makeQuad(Vector3[] verts)
        {
            Mesh m = new Mesh();
            int[] triangles = new int[]
            {
            0,1,2,3
            };
            m.vertices = verts;
            m.triangles = triangles;
            m.RecalculateNormals();
            return m;
        }
        public static Mesh makeTriangle(Vector3[] verts)
        {
            Mesh m = new Mesh();
            int[] triangles = new int[]
                {
            0,1,2
                };
            m.vertices = verts;
            m.triangles = triangles;
            m.RecalculateNormals();
            return m;
        }
        public static Mesh makePolygon(Vector3[] verts)
        {
            Mesh m = new Mesh();
            TriangulatorV3 tr = new TriangulatorV3(verts);
            int[] triangles = tr.Triangulate();
            m.vertices = verts;
            m.triangles = triangles;
            m.RecalculateNormals();
            m.RecalculateBounds();
            return m;
        }
        public static Mesh makeExtrusion(Vector3[] iverts, float height, bool cap = true)
        {
            Mesh m = new Mesh();
            Vector3 up = Vector3.up * height;
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            for (int i = 0; i < iverts.Length; i++)
            {
                int j = i + 1;
                if (j >= iverts.Length)
                {
                    j -= iverts.Length;
                }
                int vc = verts.Count;
                verts.Add(iverts[i]);
                verts.Add(iverts[j]);
                verts.Add(iverts[j] + up);
                verts.Add(iverts[i] + up);

                tris.Add(vc);
                tris.Add(vc + 2);
                tris.Add(vc + 1);
                tris.Add(vc);
                tris.Add(vc + 3);
                tris.Add(vc + 2);
            }
            m.vertices = verts.ToArray();
            m.triangles = tris.ToArray();

            //make caps
            if (cap)
            {
                Vector3[] upbound = MeshMaker.addVects(iverts, up);
                Mesh top = makePolygon(upbound);
                Mesh bot = makePolygon(iverts);
                m = joinMeshes(new Mesh[] { m, top, bot });
            }

            m.RecalculateBounds();
            m.RecalculateNormals();
            return m;
        }

        public static Vector3[] addVects(Vector3[] vects, Vector3 vect)
        {
            List<Vector3> outVects = new List<Vector3>();
            foreach (Vector3 v in vects)
            {
                outVects.Add(vect + v);
            }
            return outVects.ToArray();
        }
        public static Vector3[] addVects(Vector3 vect, Vector3[] vects)
        {
            return addVects(vects, vect);
        }
        public static Vector3[] addVects(Vector3[] vects1, Vector3[] vects2)
        {
            List<Vector3> outVects = new List<Vector3>();
            for (int i = 0; i < vects1.Length; i++)
            {
                outVects.Add(vects1[i] + vects2[i]);
            }
            return outVects.ToArray();
        }

    }
    public class TriangulatorV3
    {
        private List<Vector3> m_points = new List<Vector3>();

        public TriangulatorV3(Vector3[] points)
        {
            points = flipPoints(points);
            m_points = new List<Vector3>(points);

        }

        public Vector3[] flipPoints(Vector3[] ipts)
        {
            Vector3[] pts = new Vector3[ipts.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                float x = ipts[i].x;
                float y = ipts[i].z;
                float z = 0;
                pts[i] = new Vector3(x, y, z);
            }
            return pts;
        }

        public int[] Triangulate()
        {
            List<int> indices = new List<int>();

            int n = m_points.Count;
            if (n < 3)
                return indices.ToArray();

            int[] V = new int[n];
            if (Area() > 0)
            {
                for (int v = 0; v < n; v++)
                    V[v] = v;
            }
            else
            {
                for (int v = 0; v < n; v++)
                    V[v] = (n - 1) - v;
            }

            int nv = n;
            int count = 2 * nv;
            for (int m = 0, v = nv - 1; nv > 2;)
            {
                if ((count--) <= 0)
                    return indices.ToArray();

                int u = v;
                if (nv <= u)
                    u = 0;
                v = u + 1;
                if (nv <= v)
                    v = 0;
                int w = v + 1;
                if (nv <= w)
                    w = 0;

                if (Snip(u, v, w, nv, V))
                {
                    int a, b, c, s, t;
                    a = V[u];
                    b = V[v];
                    c = V[w];
                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(c);
                    m++;
                    for (s = v, t = v + 1; t < nv; s++, t++)
                        V[s] = V[t];
                    nv--;
                    count = 2 * nv;
                }
            }

            indices.Reverse();
            return indices.ToArray();
        }

        private float Area()
        {
            int n = m_points.Count;
            float A = 0.0f;
            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector3 pval = m_points[p];
                Vector3 qval = m_points[q];
                A += pval.x * qval.y - qval.x * pval.y;
            }
            return (A * 0.5f);
        }

        private bool Snip(int u, int v, int w, int n, int[] V)
        {
            int p;
            Vector3 A = m_points[V[u]];
            Vector3 B = m_points[V[v]];
            Vector3 C = m_points[V[w]];
            if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
                return false;
            for (p = 0; p < n; p++)
            {
                if ((p == u) || (p == v) || (p == w))
                    continue;
                Vector3 P = m_points[V[p]];
                if (InsideTriangle(A, B, C, P))
                    return false;
            }
            return true;
        }

        private bool InsideTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
            float cCROSSap, bCROSScp, aCROSSbp;

            ax = C.x - B.x; ay = C.y - B.y;
            bx = A.x - C.x; by = A.y - C.y;
            cx = B.x - A.x; cy = B.y - A.y;
            apx = P.x - A.x; apy = P.y - A.y;
            bpx = P.x - B.x; bpy = P.y - B.y;
            cpx = P.x - C.x; cpy = P.y - C.y;

            aCROSSbp = ax * bpy - ay * bpx;
            cCROSSap = cx * apy - cy * apx;
            bCROSScp = bx * cpy - by * cpx;

            return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
        }
    }

}
