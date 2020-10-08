using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foundation{

    public List<Vector2> genes;

    public Foundation(){
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

    public Foundation createOffspring(Foundation otherParent){
        Foundation child = new Foundation();
        child.genes = new List<Vector2>(this.genes);
        for(int i = 0;i<child.genes.Count;i++){
            if(Random.value > 0.5f){
                int swap = Mathf.Min(i,otherParent.genes.Count-1);
                child.genes[i] = otherParent.genes[swap];
            }
        }

        // Debug.Log(string.Join(",", this.genes));
        // Debug.Log(string.Join(",", otherParent.genes));
        // Debug.Log(string.Join(",", child.genes));

        return child;

    }

    public Mesh getMesh(){

        Vector3[] verts = new Vector3[genes.Count];
        int trisnum = 3*(genes.Count-2);
        int[] tris = new int[trisnum];
        
        for(int i =0;i<genes.Count;i++){
            verts[i] = new Vector3(genes[i].x,0.0f,genes[i].y);
        }
        for(int i =0;i<genes.Count-2;i++){
            int orig=0;
            int old = i+1;
            int newp = i+2;

            Vector3 oldVec = verts[old] - verts[orig];
            Vector3 newVec = verts[newp] - verts[orig];
            
            if(Vector3.Dot(Vector3.Cross(oldVec,newVec),new Vector3(0,1,0)) > 0){
                tris[3*i] = 0;
                tris[3*i+1] = old;
                tris[3*i+2] = newp;

            }else{
                tris[3*i] = 0;
                tris[3*i+1] = newp;
                tris[3*i+2] = old;

            }
        }

        Mesh floorMesh = new Mesh();
        floorMesh.vertices = verts;
        floorMesh.triangles = tris;
        floorMesh.RecalculateNormals();

        return floorMesh;



    }

    public Vector3[] getBoundary(){
        Vector3[] verts = new Vector3[genes.Count];
        for(int i =0;i<genes.Count;i++){
            verts[i] = new Vector3(genes[i].x,0.0f,genes[i].y);
        }
        return verts;
    }


}


