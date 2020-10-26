using System.Collections;
using System.Collections.Generic;
using UnityEngine;
abstract public class Constraint<T>{

    abstract public float getScore(T input);
}

public class FloorRegularizationConstraint:Constraint<Foundation>{
    public override float getScore(Foundation input)
    {
        return (float)Mathf.Clamp(1f-0.2f*input.genes.Count,-1f,1f);
    
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

public class LotPointsConstraint:Constraint<Foundation>{
    List<Vector3> lotPoints;

    public LotPointsConstraint(List<Vector3> lotPoints){
        this.lotPoints = lotPoints;
    }

    public override float getScore(Foundation input)
    {
        List<Vector3> verts = input.getBoundary();
        // if(!Helpers.isPointInside(verts[0],lotPoints)){
        //     return -1f;
        // }

        float score = 0f;
        foreach(Vector3 i in verts){
            if(Helpers.isPointInside(i,lotPoints)){
                score+=1f;
            }else{
                score-=1f;
            }
        }

        return score/verts.Count;
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

            if(
                Helpers.isPointInside(verts[orig],lotPoints) && 
                Helpers.isPointInside(verts[old],lotPoints) && 
                Helpers.isPointInside(verts[newp],lotPoints)
            ){
                Vector3 oldVec = verts[old] - verts[orig];
                Vector3 newVec = verts[newp] - verts[orig];
                area += 0.5f * Vector3.Cross(oldVec,newVec).magnitude; 
            }      
        }
        //Debug.Log(string.Format("Footprint Area is {0}",area));     
        return area/lotArea;
    }
}

public class CenterPositionConstraint:Constraint<RoomPartitioning>{
    Foundation footprint;

    public override float getScore(RoomPartitioning partitioning){
        this.footprint = partitioning.footprint;
        List<Vector3> centers = partitioning.getCenters();
        float score = 0f;
        foreach(Vector3 i in centers){
            if(Helpers.isPointInside(i,footprint.getBoundary())){
                score+=1f;
            }else{
                score-=1f;
            }
        }

        return score/centers.Count;
        
    }

}
public class AreaProportionConstraint:Constraint<RoomPartitioning>{

    Foundation footprint;

    public override float getScore(RoomPartitioning partitioning){
        this.footprint = partitioning.footprint;
        List<List<Vector3>> rooms = partitioning.getPartitions();
        float score = 1f;
        float proportion = 1f/rooms.Count;
        float totalArea = Helpers.getArea(footprint.getBoundary());

        foreach(List<Vector3> room in rooms){
            score -= Mathf.Abs(proportion - (Helpers.getArea(room)/totalArea));

        }

        return score;     
    }
}