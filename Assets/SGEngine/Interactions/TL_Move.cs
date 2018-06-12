using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Blocks;
using Core.Behaviours;

namespace Interactions
{
    public class TL_Move : Tool
    {
        protected Vector3 dragOffset;
        protected Plane? dragPlane;
        protected int button = 0;
        
        protected GameBlock GetBlock(GameObject o)
        {
            MBDrawScope b = o.GetComponent<MBDrawScope>();
            GameBlock block = ((GameBlock)b.scope);
            return block;
        }

        public override void OnMouseDown(GameObject gameObject)
        {
            Vector3 mp = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            BoxCollider bc = gameObject.GetComponent<BoxCollider>();
            RaycastHit raycastHit;
            bc.Raycast(ray, out raycastHit, 1000000);
            var startPoint = raycastHit.point;
            dragOffset = gameObject.transform.position - startPoint;
            dragPlane = new Plane(Vector3.up, startPoint);
        }
        public override void OnMouseDrag(GameObject gameObject)
        {
            base.OnMouseDrag(gameObject);
            if (gameObject == null) return;

            Block block = GetBlock(gameObject);
            if (dragPlane.HasValue)
            {
                if (Input.GetMouseButton(button))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float d;
                    dragPlane.Value.Raycast(ray, out d);
                    Vector3 xp = ray.GetPoint(d);
                    //subject.transform.position = xp+dragOffset;
                    block.Position = xp + dragOffset;
                }
            }
        }
        public override void OnMouseUp(GameObject gameObject)
        {
            dragPlane = null;
        }

        


    }
}


