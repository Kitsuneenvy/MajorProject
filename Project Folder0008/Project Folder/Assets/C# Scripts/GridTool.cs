using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;
public class GridTool : MonoBehaviour {
	
	
	//Keeps the position of the A* grid, and all its other important components
	float gridPosx;
	float gridPosy;
	float gridPosz;
	float gridWidth;
	float gridLength;
	float nodeWidthCount;
	float nodeLengthCount;
	float tempPosx;
	public GameObject theCollider; //The collider which we replicate to make the grids
	public GameObject astarGrid; //The grid itself
	GameObject temp; //The gameobject that stores the created unit later
	
	Vector3 positionValues; //The position of each grid square
	Quaternion rotationValues; //The rotation of each grid square
	
	int createdCount = 0; //How many grids have been made
	int depthCounter = 1; //How low we have gone
	int count = 0;
	int nameNumber = 0;
	bool atStart = true;
	bool reset = false;
	bool createGrid = true;
	bool missionInit = false;
	GridGraph graphObject;
	MissionReader mReaderObject;
	
	List<GameObject> gridColliders = new List<GameObject>();
	
	
	GameObject instantiatedCollider;
	GameObject gridParent;
	
	// Use this for initialization
	void Start () {
		graphObject = AstarPath.active.graphs[0] as GridGraph;
		mReaderObject = GameObject.Find("A*").GetComponent<MissionReader>();
	}
	//Create the grid squares
	void Update()
	{
		if(mReaderObject.returnLayoutCompleted() == true)
		{
			if(gridParent == null)
			{
				gridParent = GameObject.Instantiate(Resources.Load("GridParent")) as GameObject;
			}
			if(depthCounter==graphObject.depth){
				createGrid = false;
				}
			if(createGrid){
				gridParent.transform.position = astarGrid.GetComponent<AstarPath>().astarData.gridGraph.center;
				for(int i = 0; i < graphObject.width; i++)
				{
					
					instantiatedCollider = Instantiate(theCollider) as GameObject;
					createdCount++;
					
					if(atStart)
					{
						positionValues.y = astarGrid.transform.position.y+0.25f;
						positionValues.x = astarGrid.transform.position.x + graphObject.nodeSize/2 - graphObject.unclampedSize.x/2;
						positionValues.z = astarGrid.transform.position.z - graphObject.nodeSize/2 + graphObject.unclampedSize.y/2;
						atStart = false;
					}
					else if(!atStart && !reset)
					{
						if(positionValues.x+graphObject.nodeSize >= astarGrid.transform.position.x + graphObject.unclampedSize.x/2)
						{
							depthCounter++;
							positionValues.z -= graphObject.nodeSize;
							positionValues.x = astarGrid.transform.position.x + graphObject.nodeSize/2 - graphObject.unclampedSize.x/2;
						}
						else
						{
							positionValues.x += graphObject.nodeSize;
						}
					}
				 	instantiatedCollider.transform.position = positionValues;
					instantiatedCollider.name = instantiatedCollider.name + createdCount.ToString();
					instantiatedCollider.GetComponent<Grid>().setXY(i+1,depthCounter);
					instantiatedCollider.transform.parent = gridParent.transform;
					gridColliders.Add(instantiatedCollider);
				}
				
				//This is mostly debug, we will be doing this with the mission creator later
			} else if(!missionInit){
				if(astarGrid.GetComponent<MissionReader>().rotate == true){
					gridParent.transform.rotation = Quaternion.Euler(astarGrid.GetComponent<AstarPath>().astarData.gridGraph.rotation);

				}
				//foreach(GameObject gridSquare in gridColliders)
				//{
//					gridSquare.transform.localRotation = Quaternion.identity;
					//gridSquare.transform.parent = gridParent.transform;
				//}
				missionInit = true;
				mReaderObject.PositionUnits();
				/*//generate mission 1
				//Creates an object based on a prefab and initialises the unit type.
				temp = GameObject.Instantiate(Resources.Load("ChefChar"),GameObject.Find("GridSquare(Clone)6").transform.position,Quaternion.identity) as GameObject;
						GameObject.Find("GridSquare(Clone)6").GetComponent<Grid>().heldUnit = temp;
						temp.GetComponent<UnitGenerics>().Initialise(2);
						temp.GetComponent<UnitGenerics>().setGrid(GameObject.Find("GridSquare(Clone)6").GetComponent<Grid>());
				temp.tag="Enemy";
				temp = GameObject.Instantiate(Resources.Load("ChefChar"),GameObject.Find("GridSquare(Clone)24").transform.position,Quaternion.identity) as GameObject;
						GameObject.Find("GridSquare(Clone)24").GetComponent<Grid>().heldUnit = temp;
						temp.GetComponent<UnitGenerics>().Initialise(3);
						temp.GetComponent<UnitGenerics>().setGrid(GameObject.Find("GridSquare(Clone)24").GetComponent<Grid>());
				temp.tag="Enemy";
				temp = GameObject.Instantiate(Resources.Load("ChefChar"),GameObject.Find("GridSquare(Clone)64").transform.position,Quaternion.identity) as GameObject;
						GameObject.Find("GridSquare(Clone)64").GetComponent<Grid>().heldUnit = temp;
						temp.GetComponent<UnitGenerics>().Initialise(1);
						temp.GetComponent<UnitGenerics>().setGrid(GameObject.Find("GridSquare(Clone)64").GetComponent<Grid>());
						missionInit = true;
				*/
			}
		}
			if(mReaderObject.returnNewMission() == true && gridColliders.Count > 0)
			{
				gridParent.transform.rotation = Quaternion.identity;
				int gridSquareCounter = 0;
				foreach(GameObject gridSquare in gridColliders)
				{
					DestroyObject(gridColliders[gridSquareCounter]);
					gridSquareCounter++;
				}
				graphObject = null;
				depthCounter = 1;
				atStart = true;
				createGrid = true;
				createdCount = 0;
				missionInit = false;
				gridColliders.Clear();
				
			}
		if(mReaderObject.returnLayoutCompleted() == false){
			if(graphObject == null)
			{
				graphObject = AstarPath.active.graphs[0] as GridGraph;
			}
		}
	}
	//Return all the colliders for other logic stuff
	public List<GameObject> returnGridColliders(){
		return gridColliders;
	}
}