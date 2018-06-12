using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Behaviours;

namespace Core.Blocks
{
    public class Block : Scope
    {
        public enum Alignment
        {
            N,S,W,E,C,T,B,SE,SW,NE,NW
        }
        public System.Guid guid = System.Guid.NewGuid();
        public Color lineColor = Color.white;
        public override Vector3 Position
        {
            get
            {
                return _position;
            }
        }

        public Block()
        {
            Position = Vector3.zero;
            Size = Vector3.one;
        }
        public Block(Vector3 pos, Vector3 size) : this()
        {
            Position = pos;
            Size = size;
        }
        public virtual Block Clone()
        {
            Block b = new Block(Position,Size);
            b.Forward = Forward;
            b.lineColor = lineColor;
            return b;
        }
        
        Vector3[] makePts()
        {
            Vector3[] pts = new Vector3[8];
            Vector3[] vects = this.Vects;
            Vector3[] magVects = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                magVects[i] = vects[i] * this.Size[i];
            }
            pts[0] = this.Position;
            pts[1] = pts[0] + magVects[0];
            pts[2] = pts[1] + magVects[2];
            pts[3] = pts[0] + magVects[2];

            for (int i = 4; i < 8; i++)
            {
                pts[i] = pts[i - 4] + magVects[1];
            }


            return pts;
        }
        public virtual void OnRenderObject()
        {
            Vector3[] pts = makePts();
            if (this.Size == null) return;
            Material mat = MBDrawScope.GetMat();
            mat.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINE_STRIP);
            GL.Color(lineColor);
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
            GL.Color(lineColor);
            for (int i = 0; i < 4; i++)
            {
                GL.Vertex(pts[i]);
                GL.Vertex(pts[i + 4]);
            }
            GL.End();
            GL.PopMatrix();


        }
        public void Scale(Vector3 orgSize, Vector3 newSize,Alignment align)
        {
            Vector3 pos = GetAnkerByAlignment(orgSize,newSize, align);
            Size = newSize;
            Position = pos;
        }
        public void Scale(float ratio, int axis, Alignment align)
        {
            Vector3 size = Size;
            size[axis] *= ratio;
            Vector3 pos = GetAnkerByAlignment(Size,size,align);
            Size = size;
            Position = pos;
        }
        private Vector3 GetAnkerByAlignment(Vector3 orgSize, Vector3 newSize, Alignment align)
        {
            float[] difs = new float[3];
            for (int i = 0; i < 3; i++)
            {
                difs[i]= newSize[i] - orgSize[i];
            }


            switch (align)
            {
                case Alignment.N:
                    Vector3 vect = Vects[2] * difs[2];
                    vect += Vects[0] * (difs[0] / 2);
                    return Position-vect;

                case Alignment.S:
                    vect = Vects[0] * (difs[0] / 2);
                    return Position - vect;

                case Alignment.W:
                    vect = Vects[2] * (difs[2] / 2);
                    return Position - vect;

                case Alignment.E:
                    vect = Vects[0] * difs[0];
                    vect += Vects[2] * (difs[2] / 2);
                    return Position - vect;

                case Alignment.C:
                    vect = Vects[0] * (difs[0]/2);
                    vect += Vects[2] * (difs[2] / 2);
                    return Position - vect;

                case Alignment.T:
                    vect = Vects[0] * (difs[0] / 2);
                    vect += Vects[2] * (difs[2] / 2);
                    vect += Vects[1] * difs[1];
                    return Position - vect;

                case Alignment.B:
                    vect = Vects[0] * (difs[0] / 2);
                    vect += Vects[2] * (difs[2] / 2);
                    return Position - vect;

                case Alignment.SE:
                    vect = Vects[0] * difs[0];
                    return Position - vect;

                case Alignment.SW:
                    return Position;

                case Alignment.NE:
                    vect = Vects[2] * difs[2];
                    vect += Vects[0] * difs[0];
                    return Position - vect;
                case Alignment.NW:
                    vect = Vects[2] * difs[2];
                    return Position - vect;
                default:
                    break;
            }
            return Position;
        }
        public Block[] DivideLength(float length, int axis=0)
        {
            Block[] outBlocks=new Block[2];
            Vector3 size = Size;
            if (length > size[axis] )
            {
                outBlocks[0] = Clone();
                return outBlocks;
            }
            else if (length == 0)
            {
                outBlocks[1] = Clone();
                return outBlocks;
            }
            float l1 = length;
            float l2 = size[axis] - length;
            size = Size;
            size[axis] = l1;
            outBlocks[0] = Clone();
            outBlocks[0].Size = size;

            size = Size;
            size[axis] = l2;
            outBlocks[1] = Clone();
            outBlocks[1].Size = size;
            outBlocks[1].Position = outBlocks[1].Position + Vects[axis] * l1;
            
            return outBlocks;
        }
    }
}

