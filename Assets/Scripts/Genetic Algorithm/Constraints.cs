using System.Collections;
using System.Collections.Generic;
using UnityEngine;
abstract public class FloorConstraint{

    public abstract float getScore(Foundation floor);


}

public class FloorSmoothConstraint:FloorConstraint{

    public override float getScore(Foundation floor){
        Mesh mesh = floor.getMesh();
        float score = 0.0f;
        Vector3 vec1 = mesh.vertices[1]-mesh.vertices[0];
        Vector3 vec2 = mesh.vertices[mesh.vertices.Length-1]-mesh.vertices[0];
        score += -1 * Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(vec1,vec2));
        for(int i =1;i<mesh.vertices.Length-1;i++){
            vec1 = mesh.vertices[i-1]-mesh.vertices[i];
            vec2 = mesh.vertices[i+1]-mesh.vertices[i];
            score += -1 * Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(vec1,vec2));
        }
        vec1 = mesh.vertices[0]-mesh.vertices[mesh.vertices.Length-1];
        vec2 = mesh.vertices[mesh.vertices.Length-2]-mesh.vertices[mesh.vertices.Length-1];
        score += -1 * Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(vec1,vec2));

        return score/(float)mesh.vertices.Length;

    }
}

public class FloorOrientationConstraint:FloorConstraint{

    public override float getScore(Foundation floor){
        Mesh mesh = floor.getMesh();
        Vector3[] verts = mesh.vertices;
        float score = 0f;
        for(int i =0;i<floor.genes.Count-2;i++){
            int orig=0;
            int old = i+1;
            int newp = i+2;

            Vector3 oldVec = verts[old] - verts[orig];
            Vector3 newVec = verts[newp] - verts[orig];
            
            if(Vector3.Dot(Vector3.Cross(oldVec,newVec),new Vector3(0,1,0)) > 0){
                score+=1f;

            }else{
                score -=2f;

            }
        }

        return score/(float)(floor.genes.Count-2);

        

       
    }
}

public class FloorAreaConstraint:FloorConstraint{

    public override float getScore(Foundation floor){
        Mesh mesh = floor.getMesh();
        Vector3[] verts = mesh.vertices;
        float area = 0f;
        for(int i =0;i<floor.genes.Count-2;i++){
            int orig=0;
            int old = i+1;
            int newp = i+2;

            Vector3 oldVec = verts[old] - verts[orig];
            Vector3 newVec = verts[newp] - verts[orig];

            area += 0.5f* Vector3.Cross(oldVec,newVec).magnitude;            
            
        }        

        return area/100f;

        

       
    }
}