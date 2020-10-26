using System.Collections;
using System.Collections.Generic;
using UnityEngine;
abstract public class Constraint<T>{

    abstract public float getScore(T input);
}

public class FloorRegularizationConstraint:Constraint<Foundation>{
    public override float getScore(Foundation input)
    {
        return (float)Mathf.Clamp(-0.1f*input.genes.Count,-1f,1f);
    
    }
}
public class FloorSmoothConstraint:Constraint<Foundation>{

    public override float getScore(Foundation floor){
        float score = 0.0f;
        List<Vector3> verts = floor.getBoundary();
        if(verts.Count<3){
            return 0f;
        }
        Vector3 vec1 = verts[1]-verts[0];
        Vector3 vec2 = verts[verts.Count-1]-verts[0];

        score += -1 * Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(vec1,vec2));
        for(int i =1;i<verts.Count-1;i++){
            vec1 = verts[i-1]-verts[i];
            vec2 = verts[i+1]-verts[i];
            score += -1 * Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(vec1,vec2));
        }
        vec1 = verts[0]-verts[verts.Count-1];
        vec2 = verts[verts.Count-2]-verts[verts.Count-1];
        score += -1 * Mathf.Cos(Mathf.Deg2Rad*Vector3.Angle(vec1,vec2));

        return score/(float)verts.Count;

    }
}

public class FloorBlockyConstraint:Constraint<Foundation>{
    public override float getScore(Foundation input)
    {
        List<Vector3> boundary = Helpers.reorder(input.getBoundary());
        float score = 0f;
        for(int i =0; i<boundary.Count;i++){
            Vector3 a = boundary[(i+1)%boundary.Count]-boundary[i];
            Vector3 b = new Vector3(1,0,0);
            score+=getAngleScore(Vector3.Angle(a,b));
        }

        return score/boundary.Count;        

    }

    public float getAngleScore(float angle){
        float angleMod = angle%90;
        if(Mathf.Abs(angleMod-90)<5){
            return 1f;
        }
        if(Mathf.Abs(angleMod)<5){
           return 1f;
        }
        return Mathf.Max(Mathf.Cos(angleMod),Mathf.Sin(angleMod))/2;
    }
}
public class FloorOrientationConstraint:Constraint<Foundation>{

    public override float getScore(Foundation floor){
        List<Vector3> verts = floor.getBoundary();
        if(verts.Count<3){
            return 0f;
        }
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
public class LotCoverageConstraint:Constraint<Foundation>{

    List<Vector3> lotPoints;
    

    public LotCoverageConstraint(List<Vector3> lotPoints){
        this.lotPoints = lotPoints;
    }

    public override float getScore(Foundation footprint){

        float area = 0f;
        float lotArea = Helpers.getArea(lotPoints);
        //Debug.Log(string.Format("Lot Area is {0}",lotArea));
        List<Vector3> verts = footprint.getBoundary();
        if(verts.Count<3){
            return 0f;
        }

        for(int i =0;i<verts.Count-2;i++){
            int orig=0;
            int old = i+1;
            int newp = i+2;

            Vector3 oldVec = verts[old] - verts[orig];
            Vector3 newVec = verts[newp] - verts[orig];
            area += 0.5f * Vector3.Cross(oldVec,newVec).magnitude; 
                  
        }
        //Debug.Log(string.Format("Footprint Area is {0}",area)); 
        float output =  Mathf.Clamp(area/lotArea,0f,1f);
        return output*2 -1;
    }
}

public class RoomsRegularizationConstraint:Constraint<RoomPartitioning>{

    public override float getScore(RoomPartitioning input)
    {
        return (float)Mathf.Clamp(-0.1f*input.genes.Count,-1f,1f);
    
    }

}
public class AreaProportionConstraint:Constraint<RoomPartitioning>{

    Foundation footprint;

    public override float getScore(RoomPartitioning partitioning){
        this.footprint = partitioning.footprint;
        List<List<Vector3>> rooms = partitioning.getPartitions();
        if(rooms.Count==1){
            return 0f;
        }
        float score = 1f;
        float proportion = 1f/rooms.Count;
        float totalArea = Helpers.getArea(footprint.getBoundary());

        foreach(List<Vector3> room in rooms){
            score -= Mathf.Abs(proportion - (Helpers.getArea(room)/totalArea));

        }

        return score;     
    }
}