using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Blocks
{
    public class ModuleGrid:Block
    {
        public BuildingGridTag tag = BuildingGridTag.General;
        public int floor = 0;
        public int uIndex;
        public int vIndex;

        public ModuleGrid(Vector3 pos, Vector3 size) : base(pos, size) { }
        public override Block Clone()
        {
            ModuleGrid b = new ModuleGrid(Position, Size);
            b.Forward = Forward;
            b.lineColor = lineColor;
            b.floor = floor;
            b.tag = tag;
            b.uIndex = uIndex;
            b.vIndex = vIndex;
            return b;
        }
    }
}

