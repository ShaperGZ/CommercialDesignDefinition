using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public delegate void VectorUpdate(Vector3 vect);

    public class Scope : TreeNode, ITransform
    {
        public VectorUpdate onPositionChanged;
        public VectorUpdate onSizeChanged;
        public VectorUpdate onRotationChanged;
        public VectorUpdate onForwardChanged;

        protected Vector3 _size=Vector3.one;
        protected Vector3 _position=Vector3.zero;
        protected Vector3 _rotation = Vector3.zero;
        protected Vector3 _forward= new Vector3(0, 0, 1);
        protected Vector3 _parentPosOffset =Vector3.zero;
        protected Vector3[] _vects=new Vector3[] 
        {
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(0,0,1)
        };
        public Vector3 ParentPosOffset { get { return _parentPosOffset; } }

        public virtual Vector3 Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
                SetSize(value);
                if(onSizeChanged!=null)
                    onSizeChanged(value);

            }
        }

        public virtual Vector3 Position {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
                GetParentPositionOffset();
                SetPosition(value);
                if(onPositionChanged!=null)
                    onPositionChanged(value);
            }

        }
        public virtual Vector3 Rotation
        {
            get
            {
                return _rotation;
            }

            set
            {
                _rotation = value;
                SetRotation(value);
                if(onRotationChanged!=null)
                    onRotationChanged(value);
            }
        }
        public Vector3 Forward
        {
            get { return _forward; }
            set
            {
                _forward = value.normalized;
                _vects[0] = Vector3.Cross(Vector3.up, _forward);
                _vects[1] = Vector3.up;
                _vects[2] = _forward;
                SetForward(_forward);
                if(onForwardChanged!=null)
                    onForwardChanged(_forward);
            }
        }
        public Vector3[] Vects
        {
            get
            {
                return _vects;
            }
        }
        public void Translate(Vector3 pos)
        {
            Position = Position+ pos;
            foreach(TreeNode n in children)
            {
                ((Scope)n).Translate(pos);
            }
        }
        protected virtual void SetPosition(Vector3 newPos)
        {
            foreach (TreeNode child in children)
            {
                Scope s = (Scope)child;
                s.Position = newPos + s.ParentPosOffset;
            }
        }

        protected void GetParentPositionOffset()
        {
            if(parent!=null)
                _parentPosOffset = Position - ((Scope)parent).Position;
        }

        protected virtual void SetSize(Vector3 newSize)
        {

        }
        protected virtual void SetForward(Vector3 newForward)
        {
            foreach(TreeNode t in children)
            {
                ((Scope)t).Forward = newForward;
            }
        }
        protected virtual void SetRotation(Vector3 newRot)
        {

        }
        public override void SetParent(TreeNode parent)
        {
            base.SetParent(parent);
            GetParentPositionOffset();
        }
       public Vector3 GetStackedOffset()
        {
            if (parent == null) return Vector3.zero;
            else{
                return ParentPosOffset + ((Scope)parent).GetStackedOffset();
            }
            
        }

    }

}

