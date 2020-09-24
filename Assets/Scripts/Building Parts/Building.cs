using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    Vector2Int size;
	Wing[] wings;

	public Vector2Int size {get {return size;}}
	public Wing[] Wings {get{return wings;}}

	public Buildings(int sizeX, int sizeY, Wings[] wings){
		size = new Vector2int(sizeX, sizeY);
		this.wings = wings;
	}

	public override string ToString()
	{
		string bldg = "Building:(" + size.ToString() + "; " + wings.Length + ")\n";
		foreach (Wing w in wings){
			bldg += "\t + w.ToString() + \n";
		}
		return bldg;
	}
}
