using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Blocks;
using Core.Behaviours;

public class selectionDisplay : MonoBehaviour {
    public Text textObject;
	// Use this for initialization
	void Start () {
        ActiveDocument.doc.onObjectSelected += OnSelected;
	}

    void OnSelected()
    {
        GameObject g = ActiveDocument.doc.selectedObject;
        try
        {
            MBDrawScope b = g.GetComponent<MBDrawScope>();
            textObject.text = ((GameBlock)b.scope).guid.ToString();
        }
        catch {  }
    }

}
