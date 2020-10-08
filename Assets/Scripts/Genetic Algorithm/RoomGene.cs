using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPartitioning{

    public List<Vector2> genes;
    public Foundation footprint;

    public RoomPartitioning(){
        this.genes = new List<Vector2>();
        for(int i =0;i<3;i++){
            genes.Add(new Vector2(5.0f*Random.Range(-1.0f,1.0f),5.0f*Random.Range(-1.0f,1.0f)));
        }
    }
    public void setGenes(List<Vector2> newGenes){
        this.genes = newGenes;
    }

    public int getSize(){
        return genes.Count;
    }
    public void mutateAdd(){
        genes.Add(new Vector2(5.0f*Random.Range(0.0f,1.0f),5.0f*Random.Range(0.0f,1.0f)));

    }

    public void mutateRemove(){
        if(genes.Count>3){
            int purge = Random.Range(0,genes.Count);
            genes.RemoveAt(purge);
        }
    }

    public void mutateChange(float scale){
        int change = Random.Range(0,genes.Count);
        genes[change] = genes[change] + new Vector2(scale*Random.Range(-1.0f,1.0f),scale*Random.Range(-1.0f,1.0f));
    }

    public bool isPointInside(Vector3 point){
        int boundSize = footprint.getBoundary().Length;
        for(int i =0;i<boundSize;i++){
            Vector3 a = footprint.getBoundary()[(i+1)%boundSize]-footprint.getBoundary()[i];
            Vector3 b = point-footprint.getBoundary()[i];
            if(Vector3.Cross(a,b).magnitude <= 0 ){
                return false;
            }
        }
        return true;
    }

    //Code sourced from: https://wiki.unity3d.com/index.php/3d_Math_functions
    //Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
	//Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
	//same plane, use ClosestPointsOnTwoLines() instead.
	public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
 
		Vector3 lineVec3 = linePoint2 - linePoint1;
		Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
		Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
 
		float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
 
		//is coplanar, and not parrallel
		if(Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
		{
			float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
			intersection = linePoint1 + (lineVec1 * s);
			return true;
		}
		else
		{
			intersection = Vector3.zero;
			return false;
		}
	}

    // public Vector2[][] getPartitions(){
    //     List<Vector3> centers = new List<Vector3>();
    //     for(int i =0;i<genes.Count;i++){
    //         Vector3 point = new Vector3(genes[i].x,0.0f,genes[i].y);
    //         if (isPointInside(point)){
    //             centers.Add(point);                
    //         }
            
    //     }



    // }


}