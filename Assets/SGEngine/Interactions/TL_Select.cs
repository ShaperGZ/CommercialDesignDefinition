using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class TL_Select:Tool
    {
        public override void OnMouseDown(GameObject gameObject)
        {
            ActiveDocument.doc.selectedObject = gameObject;
        }

    }
}
