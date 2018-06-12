using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Behaviours;
using System;
using Interactions;

namespace Core.Blocks
{
    public class GameBlock : Block
    {
        private BoxCollider _collider;
        private MBDrawScope _drawScope;
        public GameObject gameObject;
       
        public override Vector3 Position
        {
            get
            {
                if (gameObject != null)
                    _position = gameObject.transform.position;
                return _position;
                
            }

            set
            {
                _position = value;
                if(gameObject!=null)
                {
                    gameObject.transform.position = value;
                    InvalidateGameObject();
                }
                    
                GetParentPositionOffset();
                SetPosition(value);
                if (onPositionChanged != null)
                    onPositionChanged(value);
            }

        }
        public GameBlock()
        {
            gameObject = new GameObject();
            _collider = gameObject.AddComponent<BoxCollider>();
            _drawScope = gameObject.AddComponent<MBDrawScope>();
            gameObject.AddComponent<MB_MouseInteraction>();
            _drawScope.scope = this;
        }
        public GameBlock(Vector3 pos, Vector3 size):this()
        {
            Position = pos;
            Size = size;
        }
        protected override void SetSize(Vector3 newSize)
        {
            base.SetSize(newSize);
            if (gameObject != null)
            {
                _collider.size = newSize;
                _collider.center = newSize / 2;
                InvalidateGameObject();
            }
                
        }
        protected override void SetForward(Vector3 newForward)
        {
            base.SetForward(newForward);
            if (gameObject != null)
            {
                gameObject.transform.forward = newForward;
                InvalidateGameObject();
            }
        }
        public override void SetParent(TreeNode parent)
        {
            base.SetParent(parent);

            //if (parent.GetType() == typeof(GameBlock))
            //    gameObject.transform.parent = ((GameBlock)parent).gameObject.transform;

        }
        public virtual void InvalidateGameObject()
        {

        }

        

    }
}


