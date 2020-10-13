using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class tests : MonoBehaviour
{

    public Material material;
    public Shader shader;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("getting footprint");
        Foundation footprint = new Foundation();
        footprint.setGenes(new List<Vector2>( new Vector2[] {
        new Vector2(0,0),
        new Vector2(2,3),
        new Vector2(-2,5),
        new Vector2(5,5),
        new Vector2(5,0)
        }));
        Debug.Log("getting partitions");
        RoomPartitioning partition = new RoomPartitioning(footprint);
        partition.setGenes(new List<Vector2>( new Vector2[] {new Vector2(1,4),new Vector2(3,2), new Vector2(3,4)}));

        // Line test1 = new Line(new Vector3(1,0,4),new Vector3(5,0,0));
        // Line test2 = new Line(new Vector3(2,0,3),new Vector3(5,0,3));
        // Vector3 output;
        // print(test1.getIntersection(out output,test2));


        display(partition);
        
    }

    void display(RoomPartitioning partition){
        List<List<Vector3>> partitionLists = partition.getPartitions();
        List<GameObject> displayHolders = new List<GameObject>();

        print("displaying");
        for(int i=0;i<partitionLists.Count;i++){
            GameObject dis = new GameObject();
            dis.AddComponent<MeshFilter>();
            dis.AddComponent<MeshRenderer>().material = material;
            dis.GetComponent<MeshRenderer>().material.shader = shader;
            displayHolders.Add(dis);
        }

        for(int i=0;i<partitionLists.Count;i++){

            Vector2[] verts2d = partitionLists[i].Select(x=> new Vector2(x.x,x.z)).ToArray();
            Triangulator tr = new Triangulator(verts2d);
            Vector3[] verts = partitionLists[i].ToArray();
            int[] tris = tr.Triangulate();
            
            Mesh mesh = new Mesh();
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();


            
            displayHolders[i].GetComponent<MeshFilter>().mesh = mesh;
        }


    }

   
}
