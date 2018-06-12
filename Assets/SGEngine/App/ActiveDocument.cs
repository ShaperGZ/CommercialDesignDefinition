using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDocument : MonoBehaviour {

    //delegates
    public UnityEngine.Events.UnityAction onObjectSelected;

    private GameObject _selectedObject;
    public GameObject selectedObject
    {
        get
        {
            return _selectedObject;
        }
        set
        {
            _selectedObject = value;
            if (onObjectSelected != null)
                onObjectSelected();
        }
    }

    public static ActiveDocument doc;

    private void Awake()
    {
        doc = this;
    }

}
