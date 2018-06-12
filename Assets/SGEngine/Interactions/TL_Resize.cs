using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Blocks;

namespace Interactions
{
    class TL_Resize:TL_Move
    {
        Block.Alignment align;
        Vector3 startPoint;
        Vector3 startPosition;
        Vector3 startSize;
        Vector3 direction;
        Plane plane2;
        int axis=0;

        public override void OnMouseDown(GameObject gameObject)
        {
            Vector3 mp = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            BoxCollider bc = gameObject.GetComponent<BoxCollider>();
            RaycastHit raycastHit;
            bc.Raycast(ray, out raycastHit, 1000000);
            Vector3 normal = raycastHit.normal;
            direction = normal;

            Block block = GetBlock(gameObject);
            startPoint = raycastHit.point;
            startSize = block.Size;
            startPosition = block.Position;
            Vector3[] vects = block.Vects;

            if (direction == -vects[0])
            {
                axis = 0;
                align = Block.Alignment.E;
            }
            else if (direction == vects[0])
            {
                axis = 0;
                align = Block.Alignment.W;
            }
            else if (direction == -vects[2])
            {
                axis = 2;
                align = Block.Alignment.N;
            }
            else if (direction == vects[2])
            {
                axis = 2;
                align = Block.Alignment.S;
            }
            else if (direction == vects[1])
            {
                axis = 1;
                align = Block.Alignment.B;

            }




            dragOffset = gameObject.transform.position - startPoint;
            if(direction == vects[1])
            {
                dragPlane = new Plane(vects[2], startPoint);
            }
            else
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

                    Plane catchPLane = new Plane(direction, xp);
                    float mag = -catchPLane.GetDistanceToPoint(startPoint);
                    bool impossible = mag < 0 && mag >= block.Size[axis];
                    if (!impossible)
                    {
                        Vector3 size = startSize;
                        float magRatio = 1 + (mag / size[axis]);
                        

                        size[axis] = size[axis] * magRatio;
                        block.Position = startPosition;
                        block.Scale(startSize,size,align);
                        //block.Size = size;
                        //block.Position = startPosition;
                    }
                    
                }
            }
        }
    }
}
