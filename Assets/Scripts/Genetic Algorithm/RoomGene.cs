using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomPartitioning{

    public List<Vector2> genes;
    public Foundation footprint;
    List<Line> outerWalls;
    List<Line> innerWalls;

    public RoomPartitioning(Foundation input){
        this.footprint = input;
        this.genes = new List<Vector2>();
        for(int i =0;i<3;i++){
            genes.Add(new Vector2(5.0f*Random.Range(-1.0f,1.0f),5.0f*Random.Range(-1.0f,1.0f)));
        }
        outerWalls = new List<Line>();
        innerWalls = new List<Line>();
        Vector3[] boundary = footprint.getBoundary();
        for(int i =0;i<boundary.Length;i++){
            outerWalls.Add(new Line(boundary[i],boundary[(i+1)%boundary.Length]));
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

    bool needInnerWall(Vector3 c1, Vector3 c2){
        bool output = true;
        Line testLine = new Line(c1,c2);
        Vector3 _ = new Vector3();
        foreach(Line wall in innerWalls){
            if (testLine.getIntersection(out _,wall)){
                return false;
            }
        }

        return output;
    }

    bool visible(Vector3 c1, Vector3 c2){
        bool output = true;
        Line testLine = new Line(c1,c2);
        Vector3 _ = new Vector3();
        foreach(Line wall in innerWalls){
            if (testLine.getIntersection(out _,wall)){ 
               if(_ != c1 && _ != c2){
                    Debug.Log(string.Format("intersection of {0} and outer wall {1} at {2}",testLine,wall,_));
                    return false;
                } 
            }
        }
        foreach(Line wall in outerWalls){
            if (testLine.getIntersection(out _,wall)){
                if(_ != c1 && _ != c2){
                    Debug.Log(string.Format("intersection of {0} and outer wall {1} at {2}",testLine,wall,_));
                    return false;
                }
            }
        }

        return output;
    }

    Line buildWall(Vector3 c1, Vector3 c2){
        bool horizontal = Mathf.Abs(c1.z-c2.z)>=Mathf.Abs(c1.x-c2.x);
        Vector3 center = (c1+c2)/2;
        Line division;
        if (horizontal){
            division = new Line(new Vector3(-100,0,center.z),new Vector3(100,0,center.z));
        }else{
            division = new Line(new Vector3(center.x,0,-100),new Vector3(center.x,0,100));
        }

        List<Vector3> intersections = new List<Vector3>();

       


        
        foreach(Line wall in innerWalls){
            Vector3 inter = new Vector3();
            if (division.getIntersection(out inter,wall)){
                intersections.Add(inter);                               
            }
        }
        foreach(Line wall in outerWalls){
            Vector3 inter = new Vector3();
            if (division.getIntersection(out inter,wall)){
                intersections.Add(inter);                              
            }
        }

        intersections = intersections.Distinct().ToList();

        
        Debug.Log("Intersections..");
        foreach(Vector3 i in intersections){
            Debug.Log(i);
        }

        List<Vector3> visibleIntersections = new List<Vector3>();

        foreach(Vector3 i in intersections){
            if(visible(i,c1)||visible(i,c2)){
                visibleIntersections.Add(i);
            }
        }
        Debug.Log("Visible Intersections..");
        foreach(Vector3 i in visibleIntersections){
            Debug.Log(i);
        }

        return(new Line(visibleIntersections[0],visibleIntersections[1]));
    }   



    

    public List<List<Vector3>> getPartitions(){

        List<Vector3> boundary = new List<Vector3>(footprint.getBoundary());        
        List<Vector3> centers = new List<Vector3>();
        List<List<Vector3>> output = new List<List<Vector3>>();


        for(int i =0;i<genes.Count;i++){
            Vector3 point = new Vector3(genes[i].x,0.0f,genes[i].y);
            if (isPointInside(point)){
                centers.Add(point);                
            }      
        }
        Debug.Log("centers are...");
        foreach(Vector3 center in centers){
            Debug.Log(center);
        }

        if(centers.Count <= 1){  
            output.Add(boundary);
            return output;
        }

        for(int i = 0; i<centers.Count-1;i++){
            for(int j = i+1;j<centers.Count;j++){
                if(needInnerWall(centers[i],centers[j])){
                    Line newInnerWall = buildWall(centers[i],centers[j]);
                    boundary.Add(newInnerWall.start);
                    boundary.Add(newInnerWall.end);
                    innerWalls.Add(newInnerWall);                                     
                }
            }
        }

        boundary = boundary.Distinct().ToList();

        // Debug.Log("Boundary Points...");
        // foreach(Vector3 boundaryPoint in boundary){
        //     Debug.Log(boundaryPoint);
        // }

        Debug.Log("inner walls..");
        foreach(Line i in innerWalls){
            Debug.Log(i);
        }

         Debug.Log("outer walls..");
        foreach(Line i in outerWalls){
            Debug.Log(i);
        }

        
        

        foreach(Vector3 roomCenter in centers){
            List<Vector3> points = new List<Vector3>();
            foreach(Vector3 boundaryPoint in boundary){
                if(visible(roomCenter,boundaryPoint)){
                    points.Add(boundaryPoint);
                }
            }
            output.Add(points);
        }

        Debug.Log("Partitions...");
        foreach(List<Vector3> room in output){
            Debug.Log("Room Points...");
            foreach(Vector3 roomPoint in room){
                Debug.Log(roomPoint);
            }
        }

        return output;

    }
}