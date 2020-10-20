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
        List<Vector3> points = new List<Vector3>();
       
        // points.Add(new Vector3(5,0,5));
        // points.Add(new Vector3(0,0,0));
        // points.Add(new Vector3(0,0,5));
        // points.Add(new Vector3(5,0,0));
        for(int i = 0;i<10;i++){
            points.Add(new Vector3(Random.Range(1,11),Random.Range(0f,3f),Random.Range(1,11)));
        }

        

        MeshRenderer meshr = gameObject.GetComponent<MeshRenderer>();
        MeshFilter meshf = gameObject.GetComponent<MeshFilter>();
        points = Helpers.reorder(points);
        // foreach(Vector3 i in points){
        //     print(i);
        // }
        meshf.mesh = Helpers.triangulate(points);
        meshr.material = material;
        meshf.mesh.RecalculateNormals();

    }

   
}
