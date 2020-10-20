using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class Helpers{

    public static bool isPointInside(Vector3 point, List<Vector3> boundary){
        int boundSize = boundary.Count;
        for(int i =0;i<boundSize;i++){
            Vector3 a = boundary[(i+1)%boundSize]-boundary[i];
            Vector3 b = point-boundary[i];
            if(Vector3.Dot(Vector3.Cross(a,b),new Vector3(0,1,0))<0){
                return false;
            }
        }
        return true;
    }

    public static bool isPointInside(Vector2 point2d, List<Vector3> boundary){
        Vector3 point = new Vector3(point2d.x,0,point2d.y);
        int boundSize = boundary.Count;
        for(int i =0;i<boundSize;i++){
            Vector3 a = boundary[(i+1)%boundSize]-boundary[i];
            Vector3 b = point-boundary[i];
            if(Vector3.Dot(Vector3.Cross(a,b),new Vector3(0,1,0))<0){
                return false;
            }
        }
        return true;
    }

    public static float getArea(List<Vector3> boundary){
        float area = 0f;
        int boundSize = boundary.Count;
        Vector3 orig = boundary[0];
        for(int i =1;i<boundSize-1;i++){
            Vector3 a = boundary[i]-orig;
            Vector3 b = boundary[i+1]-orig;
            area += Vector3.Cross(a,b).magnitude/2f;
        }
        return area;
    }

    public static Mesh triangulate(List<Vector3> verts){

        int trisnum = 3*(verts.Count-2);
        int[] tris = new int[trisnum];
        
        for(int i =0;i<verts.Count-2;i++){
            int orig=0;
            int old = i+1;
            int newp = i+2;

            tris[3*i] = orig;
            tris[3*i+1] = old;
            tris[3*i+2] = newp;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        return mesh;
    }

    public static List<Vector3> reorder(List<Vector3> points){
        List<Vector3> output = new List<Vector3>(points.ToArray());
        bool redo = true;
        Vector3 orig = output[0];
        while(redo){
            redo = false;
            for(int i =1;i<output.Count-1;i++){
                Vector3 a = output[i]-orig;
                Vector3 b = output[i+1]-orig;
                if(Vector3.Dot(Vector3.Cross(a,b),new Vector3(0,1,0))<0){
                    
                    redo = true;
                    output[i] = b+orig;
                    output[i+1] = a+orig;
                }
            }
        }

        return output;

    }
}