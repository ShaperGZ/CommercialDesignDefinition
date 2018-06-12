using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Blocks;

public class Design1Entry : MonoBehaviour {
    List<Block> blocks = new List<Block>();
    public BuildingBlock b1;
    public Slider sldPos;
    public Slider sldSizeX;
    public Slider sldSizeY;
    public Slider sldSizeZ;
    
	// Use this for initialization
	void Start () {

        b1 = new BuildingBlock(Vector3.zero, new Vector3(40,30,15));
        b1.Forward = new Vector3(0.2f, 0, 1);
        blocks.Add(b1);


        UnitDisplayBlock b2 = new UnitDisplayBlock(new Vector3(50, 0, 20), new Vector3(20, 20, 20));

        sldPos.onValueChanged.AddListener(SetPos);
        sldSizeX.onValueChanged.AddListener(SetSizeX);
        sldSizeY.onValueChanged.AddListener(SetSizeY);
        sldSizeZ.onValueChanged.AddListener(SetSizeZ);
    }
	
	public void SetSizeX(float val)
    {
        Vector3 size = b1.Size;
        size[0] = val;
        b1.Size = size;
    }
    public void SetSizeY(float val)
    {
        Vector3 size = b1.Size;
        size[1] = val;
        b1.Size = size;
    }
    public void SetSizeZ(float val)
    {
        Vector3 size = b1.Size;
        size[2] = val;
        b1.Size = size;
    }

    public void SetPos(float val)
    {
        b1.Position = new Vector3(val,0,0);
    }
    private void OnRenderObject()
    {
        foreach(Block b in blocks)
        {
            b.OnRenderObject();
        }
        
    }

}
