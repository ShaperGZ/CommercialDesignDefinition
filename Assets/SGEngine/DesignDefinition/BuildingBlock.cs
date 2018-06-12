using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Blocks
{
    public class BuildingBlock : GameBlock
    {
        //design properties
        float bayModuleWidth = 4.2f;
        float bayModuleHeight = 3.5f;

        List<ModuleGrid> moduleCells = new List<ModuleGrid>();

        List<ModuleGrid> dewllingCells = new List<ModuleGrid>();
        List<ModuleGrid> serviceCells = new List<ModuleGrid>();

        List<UnitDisplayBlock> UDP_wet = new List<UnitDisplayBlock>();
        List<UnitDisplayBlock> UDP_dry = new List<UnitDisplayBlock>();

        public BuildingBlock() : base()
        {
            CreateGrid();
        }
        public BuildingBlock(Vector3 pos, Vector3 size) : base(pos, size)
        {
            CreateGrid();
        }
        protected override void SetSize(Vector3 newSize)
        {
            base.SetSize(newSize);
            Invalidate();
        }
        protected override void SetPosition(Vector3 newPos)
        {
            base.SetPosition(newPos);
            Invalidate();
        }
        protected override void SetForward(Vector3 newForward)
        {
            base.SetForward(newForward);
            Invalidate();
        }

        public void Invalidate()
        {
            CreateGrid();//update grid
            GenProgram();
        }
        private void GenProgram()
        {
            dewllingCells.Clear();
            serviceCells.Clear();
            
            foreach(ModuleGrid c in moduleCells)
            {
                Vector3 pos = c.Position;
                Vector3[] vects = c.Vects;
                Vector3 size = c.Size;
                Color dewllingColor = new Color(0.8f, 0.6f, 0);

                if (size[2] < 12)
                {
                    GenDellingCase1(c, dewllingColor);
                }
                else
                    GenDellingCase2(c, dewllingColor);

            }

            int dif = UDP_wet.Count - dewllingCells.Count ;
            ClearUDPList(ref UDP_wet,dif);
            for( int i=0; i<dewllingCells.Count;i++)
            {
                ModuleGrid m = dewllingCells[i];
                if (i <= UDP_wet.Count)
                {
                    UnitDisplayBlock udpb = new UnitDisplayBlock(m.Position, m.Size);
                    udpb.Forward = m.Forward;
                    UDP_wet.Add(udpb);
                }
                else
                {
                    UDP_wet[i].Size = m.Size;
                    UDP_wet[i].Position = m.Position;
                }
                
            }
        }

        public void GenDellingCase1(ModuleGrid c, Color dewllingColor)
        {
            float length = Size[2] - 2;


            Block[] blocks = c.DivideLength(length, 2);
            if (blocks[0] != null)
            {
                Block dewlling = blocks[0];
                dewlling.lineColor = dewllingColor;
                dewllingCells.Add((ModuleGrid)dewlling);

            }
            if (blocks[1] != null)
            {
                Block service = blocks[1];
                serviceCells.Add((ModuleGrid)service);
            }
        }
        public void GenDellingCase2(ModuleGrid c, Color dewllingColor)
        {
            float length = (Size[2] - 2 )/2;
            Block[] blocks = c.DivideLength(length, 2);
            if (blocks[0] != null)
            {
                Block dewlling = blocks[0];
                dewlling.lineColor = dewllingColor;
                dewllingCells.Add((ModuleGrid)dewlling);

            }
            if (blocks[1] != null)
            {
                blocks = blocks[1].DivideLength(2, 2);
                Block service = blocks[0];
                Block dewlling = blocks[1];
                dewlling.lineColor = dewllingColor;
                serviceCells.Add((ModuleGrid)service);
                dewllingCells.Add((ModuleGrid)dewlling);
            }
        }

        public void CreateGrid()
        {
            CreateGrid(bayModuleWidth, bayModuleHeight, Position, Size);
        }

        public void CreateGrid(float uWidth,float vWidth,Vector3 org, Vector3 size)
        {
            moduleCells.Clear(); 
            int uCount = Mathf.CeilToInt(Size[0] / uWidth);
            int vCount = Mathf.CeilToInt(Size[1] / vWidth);
            //Debug.LogFormat("({0},{1})", uCount, vCount);

            float adjuW = size[0] / (float)uCount;
            float adjvW = size[1] / (float)vCount;
            float adjwW = size[2];

            Vector3 uVect = Vects[0];
            Vector3 vVect = Vects[1];
            Vector3 sz = new Vector3(adjuW, adjvW, adjwW);
            for (int i = 0; i < vCount; i++)
            {
                for (int j = 0; j < uCount; j++)
                {
                    float fi = (float)i;
                    float fj = (float)j;
                    Vector3 ps = org + (uVect * j * adjuW) + (vVect * i * adjvW);
                    ModuleGrid b = new ModuleGrid(ps, sz);
                    //Debug.LogFormat("#{0}:{1}", gridCells.Count, ps);
                    b.Forward = Forward;
                    moduleCells.Add(b);
                }
            }//for i
            //Debug.Log("gridcount=" + gridCells.Count);
        }
        public override void OnRenderObject()
        {
            base.OnRenderObject();
            //foreach (Block b in moduleCells) b.OnRenderObject();
            foreach (Block b in dewllingCells) b.OnRenderObject();
            foreach (Block b in serviceCells) b.OnRenderObject();
        }
        private void ClearUDPList(ref List<UnitDisplayBlock> blocks, int? count=null)
        {
            if(!count.HasValue)count = blocks.Count;
            if (count.Value < 0) return;

            for (int i = 0; i < count.Value; i++)
            {
                UnitDisplayBlock gb = blocks[blocks.Count-1];
                GameObject.Destroy(gb.gameObject);
                blocks.RemoveAt(blocks.Count - 1);
            }
        }

    }

}
