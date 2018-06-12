using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class MB_MouseInteraction : MonoBehaviour
    {
        string K_SELECT = "q";
        string K_MOVE = "w";
        string K_ROTATE = "e";
        string K_RESIZE = "r";
        
        public static Tool tool=new TL_Move();
        bool listen = true;

        // Use this for initialization
        void Start()
        {
            tool.Start(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (listen)
            {
                if (Input.GetKeyDown(K_SELECT)) tool = new TL_Select();
                else if (Input.GetKeyDown(K_MOVE)) tool = new TL_Move();
                else if (Input.GetKeyDown(K_RESIZE)) tool = new TL_Resize();
            }
            
            tool.Update(gameObject);
        }

        private void OnMouseDown()
        {
            tool.OnMouseDown(gameObject);
        }
        private void OnMouseUp()
        {
            tool.OnMouseUp(gameObject);
        }
        private void OnMouseDrag()
        {
            tool.OnMouseDrag(gameObject);
        }
        private void OnMouseOver()
        {
            tool.OnMouseOver(gameObject);
        }
        private void OnMouseEnter()
        {
            tool.OnMouseEnter(gameObject);
        }
        private void OnMouseExit()
        {
            tool.OnMouseExit(gameObject);
        }
        private void OnRenderObject()
        {
            tool.OnRenderObject(gameObject);
        }
    }
}

