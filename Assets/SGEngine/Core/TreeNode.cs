using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public delegate void TraverseCallback(TreeNode node, ref Dictionary<string, object> io, int depth = 0);
    public class TreeNode
    {
        public TreeNode parent;
        public string name="";
        public List<TreeNode> children = new List<TreeNode>();
        public bool isRoot
        {
            get
            {
                if (parent ==null) return true;
                return false;
            }
        }
        public bool isLeaf
        {
            get
            {
                if (children.Count == 0) return true;
                return false;
            }
        }
        public TreeNode root
        {
            get { return _GetRoot(); }
        }
        public List<TreeNode> leaves
        {
            get { return _GetLeaves(); }
        }

        public TreeNode() { }
        public TreeNode(string name) { this.name = name; }
        public TreeNode(string name, TreeNode parent) : this(name)
        {
            this.SetParent(parent);
        }
        public TreeNode(TreeNode parent)
        {
            this.SetParent(parent);
        }

        public virtual void SetParent(TreeNode parent)
        {
            this.parent = parent;
            if(!this.parent.children.Contains(this))
                this.parent.children.Add(this);
        }
        public virtual void AddChild(TreeNode child)
        {
            if (!children.Contains(child))
            {
                children.Add(child);
                child.parent = this;
            }
                

        }

        public object traverse(TraverseCallback callback, ref Dictionary<string, object> io, int depth = 0,bool fromRoot = false)
        {
            if (isRoot) fromRoot = true;
            callback(this, ref io, depth);
            depth++;
            for (int i = 0; i < children.Count; i++)
            {
                children[i].traverse(callback, ref io, depth, fromRoot);
            }
            return null;
        }
        protected TreeNode _GetRoot()
        {
            if (isRoot) return this;
            return parent._GetRoot();
        }
        private void _GetLeavesTravers(TreeNode n, ref Dictionary<string, object> io, int depth = 0)
        {
            if (n.isLeaf) ((List<TreeNode>)io["leaves"]).Add(n);
        }
        private List<TreeNode> _GetLeaves()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["leaves"] = new List<TreeNode>();
            traverse(_GetLeavesTravers, ref dict);
            return (List<TreeNode>)dict["leaves"];
        }
        private void _FormatTraverse(TreeNode n, ref Dictionary<string, object> io, int depth = 0)
        {
            string txt = (string)io["name"] + "\n";
            for (int i = 0; i < depth; i++)
            {
                txt += "    ";
            }
            txt += n.name;
            io["name"] = txt;
        }
        public string Format()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["name"] = "";
            traverse(_FormatTraverse, ref dict);
            return (string)dict["name"];
        }
    }


}

