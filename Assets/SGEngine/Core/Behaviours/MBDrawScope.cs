using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Behaviours
{
    public class MBDrawScope : MonoBehaviour
    {
        public Scope scope;
        public Color color = Color.white;


        Vector3[] makePts()
        {
            Vector3[] pts = new Vector3[8];
            Vector3[] vects = scope.Vects;
            Vector3[] magVects = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                magVects[i] = vects[i] * scope.Size[i];
            }
            pts[0] = scope.Position;
            pts[1] = pts[0] + magVects[0];
            pts[2] = pts[1] + magVects[2];
            pts[3] = pts[0] + magVects[2];

            for (int i = 4; i < 8; i++)
            {
                pts[i] = pts[i - 4] + magVects[1];
            }


            return pts;
        }
        public void SetScope(Scope scope)
        {
            this.scope = scope;
            
        }
        public static Material GetMat()
        {
            Material lineMaterial;
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            return lineMaterial;
        }
        private void OnRenderObject()
        {
            if (scope == null) return;
            Vector3[] pts = makePts();
            if (scope.Size == null) return;
            Material mat = GetMat();
            mat.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);
            for (int i = 0; i <= 4; i++)
            {
                int index = i;
                if (index >= 4) index = 0;
                GL.Vertex(pts[index]);
            }
            GL.End();
            GL.Begin(GL.LINE_STRIP);
            for (int i = 4; i <= 8; i++)
            {
                int index = i;
                if (index >= 8) index = 4;
                GL.Vertex(pts[index]);
            }
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(color);
            for (int i = 0; i < 4; i++)
            {
                GL.Vertex(pts[i]);
                GL.Vertex(pts[i+4]);
            }
            GL.End();
            GL.PopMatrix();


        }
    }
}

