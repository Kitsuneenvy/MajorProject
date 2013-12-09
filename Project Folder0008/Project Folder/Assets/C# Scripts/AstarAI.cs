using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;

public class AstarAI : MonoBehaviour
{
	//The point to move to
	public Vector3 targetPosition;
	private Seeker seeker;
	private CharacterController controller;
 
	//The calculated path
	public Path path;
    
	//The AI's speed per second
	public float speed = 200;
    
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 0.01f;
 
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
 	
	//Create a layermask for grid colliders
	public LayerMask squareMask;
	float journeyLength;
	float fracJourney;
	float timeValue;
	bool moveUnit = false;
	public bool myTurn = false;
	
	List<GameObject> flowerAdjacentTiles = new List<GameObject>();
	List<GameObject> moveTiles = new List<GameObject>();
	List<float> tempList = new List<float>();
	List<GameObject> finalPath = new List<GameObject>();
	List<GameObject> newFinalPath = new List<GameObject>();
	
	GameObject tempTile;
	int counter = 0;
	GameObject targetGrid = null;
	bool minusComPoint = false;
	
	public void Start ()
	{
		//set seeker
		seeker = GetComponent<Seeker> ();
	}

	void Update ()
	{
		/*
		//if right clicked
		if(Input.GetMouseButton(1))
		{ //create a ray from mouse position
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			//create raycasthit variable to check where hit
			RaycastHit rayHit;
			//cast the ray and output hit location to rayhit
			if(Physics.Raycast(mouseRay, out rayHit))
			{
				//check if the collided object has tag "square"
				if(rayHit.collider.gameObject.tag == "Square")
				{
				}
			}
		}*/
	}

	public void move (GameObject newGrid, List <GameObject> moveSquares)
	{
		moveTiles = moveSquares;//think is redundant now
		GameObject.Find ("Panel").GetComponent<DropDownMenu> ().resetSelectedUnit ();
		Debug.Log("Next Grid to check: " + newGrid.ToString());
		if (GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints > 0) {
			if(targetGrid == null)
			{
				//set target position to location of collided raycast
				targetPosition = newGrid.transform.position;
				targetGrid = newGrid;
				finalPath.Add(targetGrid);
				Debug.Log("Set targetGrid: " + targetGrid.ToString());
			}
			//check adjacents of passed tile
			List<GameObject> adjacentTiles = this.GetComponent<UnitGenerics>().checkAdjacentGrids(newGrid);
			List<GameObject> possiblePaths = new List<GameObject>();
			int adjCounter = 0;
			foreach(GameObject adjTile in adjacentTiles)
			{
				adjCounter++;
			
				//check if already adjacent to target and move
				if(adjTile == this.GetComponent<UnitGenerics>().onGrid.gameObject && finalPath.Count == 0)
				{
					if(GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints == 3)
					{
						GameObject.Find("CommPt3").GetComponent<UITexture>().material = null;
					}
					else if(GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints == 2)
					{
						GameObject.Find("CommPt2").GetComponent<UITexture>().material = null;
					}
					else
					{
						GameObject.Find("CommPt1").GetComponent<UITexture>().material = null;
					}
					GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints--;
					
					targetGrid = null;
					//Start a new path to the targetPosition, return the result to the OnPathComplete function
					seeker.StartPath (transform.position, finalPath[0].transform.position, OnPathComplete);
					moveUnit = true;
					break;
				}
				else if(adjTile != this.GetComponent<UnitGenerics>().onGrid.gameObject && moveUnit == false)
				{
					//else if a moveable square and not already in finalpath (to stop backtracking)
					if(moveTiles.Contains(adjTile))
					{
						if(finalPath.Count != 0)
						{
							if(!finalPath.Contains(adjTile) && adjTile != targetGrid)
							{
								possiblePaths.Add(adjTile);
							}
						}
						else
						{
								possiblePaths.Add(adjTile);
						}
					}
					//if hit a dead end
					else if(!moveTiles.Contains(adjTile) && adjCounter == adjacentTiles.Count && possiblePaths.Count == 0)
					{
						
						if(newGrid != targetGrid)
						{
							moveTiles.Remove(newGrid);
							if(finalPath.Contains(newGrid))
							{
							finalPath.Remove(newGrid);
							}
						}
						move (finalPath[finalPath.Count-1],moveTiles);
					}
				}
				else if(adjTile == this.GetComponent<UnitGenerics>().onGrid.gameObject && moveUnit == false)
				{
					targetGrid = null;
					if(GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints == 3)
					{
						GameObject.Find("CommPt3").GetComponent<UITexture>().material = null;
					}
					else if(GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints == 2)
					{
						GameObject.Find("CommPt2").GetComponent<UITexture>().material = null;
					}
					else
					{
						GameObject.Find("CommPt1").GetComponent<UITexture>().material = null;
					}
					GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints--;
					finalPath.Reverse();
					seeker.StartPath (this.transform.position, finalPath[0].transform.position, OnPathComplete);
					moveUnit = true;
					break;
				}
			}
			if(possiblePaths.Count != null && moveUnit == false)
			{
				//check tiles closeness to end
				foreach(GameObject tile in possiblePaths)
				{
					counter++;
					float distance = Vector3.Distance(this.GetComponent<UnitGenerics>().onGrid.gameObject.transform.position,tile.transform.position);
					//closest tile added to list
					if(tempList.Count > 0 && distance < tempList[0])
					{
						tempList.Add(distance);
						tempTile = tile;
					}
					//if first tile automatically add to list as closest distance
					else if(tempList.Count == 0)
					{
						tempList.Add(distance);
						tempTile = tile;
					}
					//once finished checking each tile add to final path
					if(tile == possiblePaths[possiblePaths.Count-1])
					{
						finalPath.Add(tempTile);
						tempList.Clear();
						counter = 0;
						move (finalPath[finalPath.Count-1],moveTiles);
					}
				}
				
			}
			
			//set y above terrain so as to not have the character Lerp into the ground
			//targetPosition.y = targetPosition.y + 0.5f*this.GetComponentInChildren<Renderer>().renderer.bounds.size.y;
			
		} else {
			foreach(GameObject gridObject in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()){
				gridObject.renderer.material = Resources.Load ("Transparent") as Material;
				gridObject.renderer.material.color = Color.black;
			}
			GameObject.Find("Game Manager").GetComponent<gameManage>().nextTurn();
		}
	}

	public void OnPathComplete (Path p)
	{
		
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
 
	public void FixedUpdate ()
	{
		if(finalPath.Count != 0)
		{
			if(Vector3.Distance(this.transform.position,finalPath[0].transform.position)<=1f){
				finalPath.Remove(finalPath[0]);
				//moveUnit = false;
				if(finalPath.Count != 0)
				{
					seeker.StartPath (transform.position, finalPath[0].transform.position, OnPathComplete);
					//moveUnit = true;
				}
				else
				{
					moveUnit=false;
				}
			}
		}
		if(moveUnit==false){
			if(myTurn == true){
				myTurn = false;
				if(GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().returnTurn()==false){
					GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().toggleTurn();
				}
				if(this.tag == "Enemy")
				{
					if(this.GetComponent<UnitGenerics>().statsIncreased == true)
					{
						switch(this.GetComponent<UnitGenerics>().unitType)
						{
						case (0):
						{
							this.GetComponent<UnitGenerics>().attack = 20;
							break;
						}
						case (1):
						{
							this.GetComponent<UnitGenerics>().attack = 40;
							break;
						}
						case (2):
						{
							this.GetComponent<UnitGenerics>().attack = 20;
							break;
						}
						case (3):
						{
							this.GetComponent<UnitGenerics>().attack = -30;
							break;
						}
						default:
							break;
						}
						this.GetComponent<UnitGenerics>().statsIncreased = false;
					}
					if(GameObject.Find("A*").GetComponent<MissionReader>().flowerUnits.Count > 0)
					{
						foreach( GameObject flower in GameObject.Find("A*").GetComponent<MissionReader>().flowerUnits)
						{
							flowerAdjacentTiles = this.GetComponent<UnitGenerics>().checkAdjacentGrids(flower.GetComponent<UnitGenerics>().onGrid.gameObject);
							
							foreach(GameObject tile in flowerAdjacentTiles)
							{
								Grid gridObject = tile.GetComponent<Grid>();
								if(gridObject.heldUnit != null && gridObject.heldUnit == this.gameObject && gridObject.heldUnit.tag != "PlayerUnit" && (!gridObject.heldUnit.name.Contains("Flower") || !gridObject.heldUnit.name.Contains("Florist")))
								{
									this.GetComponent<UnitGenerics>().statsIncreased = true;
									//int unitType = this.GetComponent<UnitGenerics>().unitType;
									switch(this.GetComponent<UnitGenerics>().unitType)
									{
									case (0):
									{
										this.GetComponent<UnitGenerics>().attack = 30;
										break;
									}
									case (1):
									{
										this.GetComponent<UnitGenerics>().attack = 50;
										break;
									}
									case (2):
									{
										this.GetComponent<UnitGenerics>().attack = 30;
										break;
									}
									case (3):
									{
										this.GetComponent<UnitGenerics>().attack = 40;
										break;
									}
									default:
										break;
									}
								}
							}
						}
					}
				}
			}
				if(this.animation.isPlaying == false&&this.GetComponent<UnitGenerics>().health>0)
				{
					this.animation.Play("Idle1");
				}
		}
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		//if current waypoint is equal to or greater than 1
		if (currentWaypoint >= path.vectorPath.Count) {
//            Debug.Log ("End Of Path Reached");
			//attempt at moving closer
			//Lerp closer to location
		//	transform.position = Vector3.Lerp (this.transform.position, targetPosition, (1 / (Vector3.Distance (this.transform.position, targetPosition))) * 0.1f);
			
			return;
		}
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;//DONT THINK IS REQUIRED ANYMORE CHECK LATER
		
		
		 if(moveUnit== true){
			//set timevalue
		timeValue = (1 / (finalPath[0].transform.position - this.transform.position).magnitude);
			if (DistanceCalculation (this.transform.position, finalPath[0].transform.position) == true) { 
				
				this.animation.Play("Walk");
				
				//Lerp to location of raycasted position.z
				this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, finalPath[0].transform.position.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, finalPath[0].transform.position.z)))) * 0.1f);
					
				if (this.transform.position.z <= finalPath[0].transform.position.z + 0.2f && this.transform.position.z >= finalPath[0].transform.position.z - 0.2f) {
						
				this.transform.LookAt (new Vector3 (finalPath[0].transform.position.x, this.transform.position.y, finalPath[0].transform.position.z));
					this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (finalPath[0].transform.position.x, this.transform.position.y, this.transform.position.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (finalPath[0].transform.position.x, this.transform.position.y, this.transform.position.z)))) * 0.1f);
				}
				GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueReader>().TaskCompletion(this.gameObject);
				
			} else if (DistanceCalculation (this.transform.position, finalPath[0].transform.position) == false) {

				this.animation.Play("Walk");
				
				//Lerp to location of raycasted position.x
				this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (finalPath[0].transform.position.x, this.transform.position.y, this.transform.position.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (finalPath[0].transform.position.x, this.transform.position.y, this.transform.position.z)))) * 0.1f);
					
				if (this.transform.position.x <= finalPath[0].transform.position.x + 0.2f && this.transform.position.x >= finalPath[0].transform.position.x - 0.2f) {
						
					this.transform.LookAt (new Vector3 (finalPath[0].transform.position.x, this.transform.position.y, finalPath[0].transform.position.z));
					this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, finalPath[0].transform.position.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, finalPath[0].transform.position.z)))) * 0.1f);
				}
				GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueReader>().TaskCompletion(this.gameObject);
				}
		}
		
		
       
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
	
	bool DistanceCalculation (Vector3 position1, Vector3 position2)
	{
		float calculateX;
		float calculateZ;
		if (position1.x > position2.x) {
			calculateX = position1.x - position2.x;
		} else if (position1.x < position2.x) {
			calculateX = position2.x - position1.x;
		} else {
			calculateX = position1.x - position2.x;
		}
		
		if (position1.z > position2.z) {
			calculateZ = position1.z - position2.z;
		} else if (position1.z < position2.z) {
			calculateZ = position2.z - position1.z;
		} else {
			calculateZ = position1.z - position2.z;
		}
		
		if (calculateX >= calculateZ) {
			return true;
		}
		if (calculateZ > calculateX) {
			return false;
		} else {
			return false;
		}
	}
} 
