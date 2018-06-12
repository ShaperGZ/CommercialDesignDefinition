using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Geometry;

namespace Core.Blocks
{
    class UnitDisplayBlock:GameBlock
    {
        float gap = 0.3f;
        MeshFilter _meshFilter;
        MeshRenderer _meshRenderer;
        public UnitDisplayBlock() : this(Vector3.zero,Vector3.one)
        {
        }

        public UnitDisplayBlock(Vector3 position, Vector3 size) : base(position, size)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshRenderer.material = DefaultMat();
            UpdateMesh();
            
        }
        public override void InvalidateGameObject()
        {
            if (_meshFilter == null) return;
            base.InvalidateGameObject();
            UpdateMesh();
        }

        public void UpdateMesh()
        {
            //Debug.Log("updateMesh");
            //if (gameObject == null || _meshFilter == null) return;
            Vector3[] vects = Vects;
            Vector3 size = Size;
            Vector3[] gapVects = new Vector3[4];
            for (int i = 0; i < 3; i++)
            {
                gapVects[i] = vects[i] * gap;
            }
            gapVects[3] = gapVects[0] + gapVects[1] + gapVects[2];


            Vector3[] magVects=new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                magVects[i] = (vects[i] * size[i]) - (gapVects[i] * 2);
            }

            Vector3[] pts = new Vector3[4];
            pts[0] = gapVects[3];
            pts[1] = pts[0] + magVects[0];
            pts[2] = pts[1] + magVects[2];
            pts[3] = pts[0] + magVects[2];
            
            Mesh mesh = MeshMaker.makeExtrusion(pts, magVects[1].magnitude);
            _meshFilter.mesh = mesh;
        }
        private Material DefaultMat()
        {
            Material m = new Material(Shader.Find("Standard"));
            m.color = Color.white;
            return m;
        }
    }
}
