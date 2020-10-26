using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tests2 : MonoBehaviour{

    public List<Vector3> lotPoints;

    void Start(){

        // List<Foundation> testFootprints = new List<Foundation>();
        // List<Constraint<Foundation>> cons = new List<Constraint<Foundation>>();

        // for(int i = 0;i<10;i++){
        //     testFootprints.Add(new Foundation());
        // }
        // cons.Add(new FloorSmoothConstraint());
        // cons.Add(new FloorOrientationConstraint());
        // cons.Add(new LotCoverageConstraint(lotPoints));

        // Optimizer<Foundation> optimizer = new Optimizer<Foundation>(
        //     testFootprints,
        //     cons,
        //     100,
        //     0.1f,
        //     0.1f,
        //     0.1f,
        //     1
        // );

        // Foundation output = optimizer.getOptimizedResult();

        // List<RoomPartitioning> testRooms = new List<RoomPartitioning>();
        // List<Constraint<RoomPartitioning>> cons2 = new List<Constraint<RoomPartitioning>>();

        // for(int i = 0;i<10;i++){
        //     testRooms.Add(new RoomPartitioning(output));
        // }
        // cons2.Add(new AreaProportionConstraint());
       

        // Optimizer<RoomPartitioning> optimizer2 = new Optimizer<RoomPartitioning>(
        //     testRooms,
        //     cons2,
        //     100,
        //     0.1f,
        //     0.1f,
        //     0.1f,
        //     1
        // );

        // RoomPartitioning bestPartitions = optimizer2.getOptimizedResult();

        // List<List<Vector3>> partitions = bestPartitions.getPartitions();

        // GameObject floor = new GameObject();
        // floor.transform.parent = gameObject.transform;
        // floor.AddComponent<MeshRenderer>();
        // MeshFilter meshf = floor.AddComponent<MeshFilter>();
        // meshf.mesh = Helpers.triangulate(output.getBoundary());

        // GameObject rooms = new GameObject();
        // rooms.transform.parent = gameObject.transform;
        // for(int i = 0;i<partitions.Count;i++){
        //     GameObject room = new GameObject();
            
        // }





    }
        
}