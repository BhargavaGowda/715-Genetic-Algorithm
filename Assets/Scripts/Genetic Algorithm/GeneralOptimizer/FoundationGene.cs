using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foundation:Gene<Foundation>{

    public List<Vector2> genes;

    public Foundation(List<Vector2> genes){
        this.genes = genes;
    }

    public Foundation(){
        List<Vector2> randomGenes = new List<Vector2>();
        for(int i =0;i<3;i++){
            randomGenes.Add(new Vector2(5.0f*Random.Range(-1.0f,1.0f),5.0f*Random.Range(-1.0f,1.0f)));
        }
        this.genes = randomGenes;

    }

    public override void setGenes(List<Vector2> newGenes){
        this.genes = newGenes;
    }

    public override int getSize(){
        return genes.Count;
    }
    public override void mutateAdd(){
        genes.Add(new Vector2(5.0f*Random.Range(0.0f,1.0f),5.0f*Random.Range(0.0f,1.0f)));

    }

    public override void mutateRemove(){
        if(genes.Count>3){
            int purge = Random.Range(0,genes.Count);
            genes.RemoveAt(purge);
        }
    }

    public override void mutateChange(float scale){
        int change = Random.Range(0,genes.Count);
        genes[change] = genes[change] + new Vector2(scale*Random.Range(-1.0f,1.0f),scale*Random.Range(-1.0f,1.0f));
    }

    public override Foundation createOffspring(Foundation otherParent){
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

    public List<Vector3> getBoundary(){
        List<Vector3> verts = new List<Vector3>();
        for(int i =0;i<genes.Count;i++){
            verts.Add(new Vector3(genes[i].x,0.0f,genes[i].y));
        }
        return verts;
    }


}


