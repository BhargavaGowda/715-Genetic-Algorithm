/*
HOW THE CODE WORKS:
1. find rectangular boundary of Room.
2. Generate random point in rectangle boundary
3. check if point falls in shape if yes proceed
3.1. this is done by expanding points x,y direction until collision.
4. repeat 2-3 until n points inside the shape
5. expand points at random by x squares in x,y direction until collision with boundary or other rooms
6. expand all points at random in x or y direction to fill gaps
7. now the rectangular boundary is filled
8. remove room parts that are outside of the building.
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomPartition : MonoBehaviour
{
    public int popSize = 10;
    public float mutateAddChance = 0.1f;
    public float mutateRemoveChance = 0.1f;
    public float mutateChangeChance = 0.1f;
    public float mutateScale = 1f;
    public int initIter = 10;
    public Material displayMat;
    Foundation[] population;
    Constraint<Foundation>[] constraintList;

    public int[] consEnables = new int[]{1,1};



    // Start is called before the first frame update
    void Start()
    {
        population = new Foundation[popSize];
        constraintList = new Constraint<Foundation>[]{new FloorSmoothConstraint(),new FloorOrientationConstraint()};
        for(int i = 0; i<popSize;i++){
            population[i] = new Foundation();
        }

        StartCoroutine("process");


        }

    IEnumerator process(){
        for(int i =0;i<initIter;i++){
            iterate();
            //Debug.Log("iteration: " + i);
            yield return null;
        }
        displayMesh();
    }
    float applyConstraints(Foundation f){

        float score = 0f;
        for(int i=0;i<constraintList.Length;i++){
            score+=constraintList[i].getScore(f)*consEnables[i];
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

		//get mesh vertices FIRST FLOOR IN MESH
		Mesh mesh;
		Vector3[] vertices;
		mesh = Helpers.triangulate(sortedFloors[0].getBoundary());
        vertices = mesh.vertices;
		Bounds bounds = mesh.bounds;
    /*
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
    meshCollider.sharedMesh = mesh;
		meshCollider.convex = true;
		meshCollider.isTrigger = true;
    */
    Vector2[] vertices2D;
    vertices2D = ConvertArray(vertices);

		//Vector3 room;

		//print vertices
		string result = "List contents: ";
		foreach (var item in vertices)
		{
			result += item.ToString() + ", ";
		}
		Debug.Log(result);

		for (int i=0; i<5;i++){
		//find random point inside the bouding box
			Vector3 position = new Vector3(Random.Range(bounds.min[0], bounds.max[0]), Random.Range(bounds.min[1], bounds.max[1]), Random.Range(bounds.min[2], bounds.max[2]));
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			sphere.transform.position = position;
      Vector2 position2D = new Vector2(position.x, position.z);

      if (ContainsPoint(vertices2D, position)){
			     Debug.Log(position + "HIT");
      }
      else{
        Destroy(sphere);
      }
			//Collider meshColliders = gameObject.AddComponent<Collider>();
			//meshColliders.sharedMesh = sphere;
			//meshColliders.isTrigger = true;

		}

        for(int i=0; i<1;i++){
            GameObject display = new GameObject("display");
            //display.GetComponent<Transform>().position = new Vector3(-20f+(40f/4)*i,0,0);
            MeshFilter meshf = display.AddComponent<MeshFilter>();
            MeshRenderer meshr = display.AddComponent<MeshRenderer>();
            meshf.sharedMesh = Helpers.triangulate(sortedFloors[i].getBoundary());
            meshr.material = displayMat;
      }


    }

    Vector2[] ConvertArray(Vector3[] v3){
        Vector2 [] v2 = new Vector2[v3.Length];
        for(int i = 0; i <  v3.Length; i++){
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.z);
        }
        return v2;
    }

    public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
        {
            var j = polyPoints.Length - 1;
            var inside = false;
            for (int i = 0; i < polyPoints.Length; j = i++)
            {
                var pi = polyPoints[i];
                var pj = polyPoints[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                    (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;
            }
            return inside;
        }

	/*
	private bool IsInsideMesh(Vector3 point)
     {
         Physics.queriesHitBackfaces = true;
         int hitsUp = Physics.RaycastNonAlloc(point, Vector3.up, _hitsUp);
         int hitsDown = Physics.RaycastNonAlloc(point, Vector3.down, _hitsDown);
         Physics.queriesHitBackfaces = false;
         for (var i = 0; i < hitsUp; i++)
             if (_hitsUp[i].normal.y > 0)
                 for (var j = 0; j < hitsDown; j++)
                     if (_hitsDown[j].normal.y < 0 && _hitsDown[j].collider == _hitsUp[i].collider)
                         return true;

         return false;
     }
	 */
}
