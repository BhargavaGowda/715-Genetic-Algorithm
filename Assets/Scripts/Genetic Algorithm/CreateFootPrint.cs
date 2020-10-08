using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateFootPrint : MonoBehaviour
{
    public int popSize = 10;
    public float mutateAddChance = 0.1f;
    public float mutateRemoveChance = 0.1f;
    public float mutateChangeChance = 0.1f;
    public float mutateScale = 1f;
    public int initIter = 10;
    public Material displayMat;
    Foundation[] population;
    FloorConstraint[] constraintList;

    public float[] constraintWeights = new float[]{1f,1f,1f};


    
    // Start is called before the first frame update
    void Start()
    {
        population = new Foundation[popSize];
        constraintList = new FloorConstraint[]{new FloorSmoothConstraint(),new FloorOrientationConstraint(), new FloorAreaConstraint()};
        for(int i = 0; i<popSize;i++){
            population[i] = new Foundation();
        }

        StartCoroutine("process");
        
        
        }

    IEnumerator process(){
        for(int i =0;i<initIter;i++){
            iterate();
            Debug.Log("iteration: " + i);
            yield return null;
        }
        displayMesh();
    }
    float applyConstraints(Foundation f){

        float score = 0f;
        for(int i=0;i<constraintList.Length;i++){
            score+=constraintList[i].getScore(f)*constraintWeights[i];
        }
        return score;
    }
    void iterate(){
        Foundation[] sortedFloors = population.OrderBy(f => -1*applyConstraints(f)).ToArray();
        List<Foundation> newGen = new List<Foundation>();
        int split = popSize/2;
        for(int i =0;i<split;i++){
            int parent1 = Random.Range(0,split);
            int parent2 = Random.Range(0,popSize);
            newGen.Add(sortedFloors[i].createOffspring(sortedFloors[parent1]));
            newGen.Add(sortedFloors[i].createOffspring(sortedFloors[parent2]));

        }

        foreach(Foundation individual in newGen){
            if(Random.value<mutateAddChance){
                individual.mutateAdd();
            }
            if(Random.value<mutateChangeChance){
                individual.mutateChange(mutateScale);
            }
            if(Random.value<mutateRemoveChance){
                individual.mutateRemove();
            }
        }

        population = newGen.ToArray();

        

    }
    void displayMesh(){
        Foundation[] sortedFloors = population.OrderBy(f => -1*applyConstraints(f)).ToArray();

        for(int i=0; i<3;i++){
            GameObject display = new GameObject("display");
            display.GetComponent<Transform>().position = new Vector3((60f/2f)*i,0,0);
            MeshFilter meshf = display.AddComponent<MeshFilter>();
            MeshRenderer meshr = display.AddComponent<MeshRenderer>();
            meshf.sharedMesh = sortedFloors[i].getMesh();
            meshr.material = displayMat;

            Debug.Log(constraintList[1].getScore(sortedFloors[i]));

        }

        

    }
    
    
}
