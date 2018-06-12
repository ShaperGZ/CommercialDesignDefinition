using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBSelectObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        ActiveDocument.doc.selectedObject = gameObject;
    }
}
