using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
/*
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
    FloorConstraint[] constraintList;

    // Start is called before the first frame update
    void Start()
    {
        //constraintList = new FloorConstraint[]{new FloorSmoothConstraint(),new FloorOrientationConstraint()};
    }
}
*/