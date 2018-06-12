using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface ITransform
    {
        Vector3 Size { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
    }
}

