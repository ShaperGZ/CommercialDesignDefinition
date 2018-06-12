using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class UT_Tree : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TreeNode t1 = new TreeNode("t1");
        TreeNode t2 = new TreeNode("t2",t1);
        TreeNode t3 = new TreeNode("t3",t1);
        TreeNode t4 = new TreeNode("t4",t2);
        TreeNode t5 = new TreeNode("t5",t4);

        print(t1.Format());
        if (t1.isRoot != true) throw new System.Exception("t1 should be true");
        if(t2.isRoot != false) throw new System.Exception("t2 should be false");

        List<TreeNode> lv1 = t1.leaves;
        List<TreeNode> lv2 = t2.leaves;
        List<TreeNode> lv3 = t3.leaves;

        string txt = "";
        foreach (TreeNode n in lv1) txt +=  n.name;
        foreach (TreeNode n in lv2) txt +=  n.name;
        foreach (TreeNode n in lv3) txt +=  n.name;
        print(txt);
        if (txt != "t5t3t5t3") throw new System.Exception("txt should = t5t3t5t3");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
