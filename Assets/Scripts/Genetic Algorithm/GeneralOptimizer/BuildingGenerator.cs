using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make sure all mesh points are specified clockwise.
class BuildingGenerator:MonoBehaviour{

    public List<Vector3> lot;
    public int popSize = 10;
    public float mutateAdd = 0.1f;
    public float mutateRemove = 0.1f;
    public float mutateChange = 0.1f;
    public float mutateScale = 1f;
    public int iter = 10;

    void Start(){
        generateBuilding();
    }

    void generateBuilding(){

        displayLot();        
        Foundation footprint = getFootprint();
        displayFootprint(footprint);
        //Debug.Log(new LotPointsConstraint(lot).getScore(footprint));
        RoomPartitioning partitioning = getPartitioning(footprint);
        displayRooms(partitioning);
		displayOuterWalls(footprint);


    }

    Foundation getFootprint(){
        List<Constraint<Foundation>> footprintConstraints = new List<Constraint<Foundation>>();
        footprintConstraints.Add(new FloorSmoothConstraint());
        footprintConstraints.Add(new FloorOrientationConstraint());
        footprintConstraints.Add(new LotCoverageConstraint(lot));
        footprintConstraints.Add(new LotPointsConstraint(lot));

        List<Foundation> seedPop = new List<Foundation>();
        for(int i =0;i<popSize;i++){
            seedPop.Add(new Foundation());
        }
        Optimizer<Foundation> footprintOptimizer = new Optimizer<Foundation>(
            seedPop,
            footprintConstraints,
            iter,
            mutateAdd,
            mutateRemove,
            mutateChange,
            mutateScale
            );

        return footprintOptimizer.getOptimizedResult();

    }

    RoomPartitioning getPartitioning(Foundation footprint){
        List<Constraint<RoomPartitioning>> roomConstraints = new List<Constraint<RoomPartitioning>>();
        //roomConstraints.Add(new AreaProportionConstraint());
        roomConstraints.Add(new CenterPositionConstraint());
        List<RoomPartitioning> seedPop = new List<RoomPartitioning>();
        for(int i =0;i<popSize;i++){
            seedPop.Add(new RoomPartitioning(footprint));
        }
        Optimizer<RoomPartitioning> roomOptimizer = new Optimizer<RoomPartitioning>(
            seedPop,
            roomConstraints,
            iter,
            mutateAdd,
            mutateRemove,
            mutateChange,
            mutateScale
            );

        return roomOptimizer.getOptimizedResult();


    }
    
    void displayFootprint(Foundation footprint){
        GameObject floor = new GameObject();
        floor.name = "Footprint";
        floor.transform.parent = gameObject.transform;
        floor.transform.position = new Vector3(0,1,0);
        MeshRenderer meshr = floor.AddComponent<MeshRenderer>();
        MeshFilter meshf = floor.AddComponent<MeshFilter>();
        meshf.mesh = Helpers.triangulate(footprint.getBoundary());
        meshr.material.SetColor("_Color",Color.grey);

    }

    void displayLot(){
        GameObject lotDis = new GameObject();
        lotDis.name = "Lot";
        lotDis.transform.parent = gameObject.transform;
        MeshRenderer meshr = lotDis.AddComponent<MeshRenderer>();
        MeshFilter meshf = lotDis.AddComponent<MeshFilter>();
        meshf.mesh = Helpers.triangulate(this.lot);
        meshr.material.SetColor("_Color",Color.white);

    }

    void displayRooms(RoomPartitioning partitioning){
        GameObject partitionsContainer = new GameObject();
        partitionsContainer.name = "Rooms";
        partitionsContainer.transform.parent = gameObject.transform;
        partitionsContainer.transform.position = new Vector3(0,2,0);
        List<List<Vector3>> rooms = partitioning.getPartitions();
        List<GameObject> roomObjects = new List<GameObject>();
        for(int i = 0;i<rooms.Count;i++){
            GameObject room = new GameObject();
            room.name = string.Format("Room {0}",i+1);
            room.transform.parent = partitionsContainer.transform;
            room.transform.position = new Vector3(0,2,0);
            MeshRenderer meshr = room.AddComponent<MeshRenderer>();
            MeshFilter meshf = room.AddComponent<MeshFilter>();
            meshf.mesh = Helpers.triangulate(Helpers.reorder(rooms[i]));
            meshr.material.SetColor("_Color",Color.HSVToRGB(i*1f/rooms.Count,1,1));
        }
    }
	
	void displayOuterWalls(Foundation footprint){
		List<Vector3> vertices = footprint.getBoundary();
		Vector3 position2;
		Vector3 position;
		float rotato;
		for (int i=0; i<vertices.Count;i++){
			if (i==vertices.Count-1){
				position = vertices[i];
				position2 = vertices[0];
			}
			else{
				position = vertices[i];
				position2 = vertices[i+1];
			}
			Vector3 between = position2 - position;
			float distance = between.magnitude;
			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
			wall.name = "Outer Wall";
			//wall.GetComponent<Renderer>().material.mainTexture = myTexture;
			wall.transform.localScale = new Vector3(distance, 2.5f, 0.1f);
			if (position.z>=position2.z){
				rotato = Mathf.Acos((position2.x-position.x)/distance)* 180/Mathf.PI;
			}
			else{
				rotato = Mathf.Acos((position.x-position2.x)/distance)* 180/Mathf.PI;
			}
			wall.transform.Rotate(0,rotato,0);
			wall.transform.position = position + (between/2);
			wall.transform.position += new Vector3(0f,3.25f,0);
		}
	}
}