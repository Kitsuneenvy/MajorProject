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

	public void move (GameObject newGrid)
	{
		GameObject.Find ("Panel").GetComponent<DropDownMenu> ().resetSelectedUnit ();
		if (GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints > 0) {
			GameObject.Find ("Game Manager").GetComponent<gameManage> ().commandPoints--;
			//set target position to location of collided raycast
			targetPosition = newGrid.transform.position;
			//set y above terrain so as to not have the character Lerp into the ground
			//targetPosition.y = targetPosition.y + 0.5f*this.GetComponentInChildren<Renderer>().renderer.bounds.size.y;
			//Start a new path to the targetPosition, return the result to the OnPathComplete function
			seeker.StartPath (transform.position, targetPosition, OnPathComplete);
			moveUnit = true;
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
		
		
		if(Vector3.Distance(this.transform.position,targetPosition)<=1f){
			moveUnit=false;
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
					{Debug.Log("Reset");
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
								{Debug.Log("attack increased");
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
										this.GetComponent<UnitGenerics>().attack = -40;
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
				if(this.animation.isPlaying == false)
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
			transform.position = Vector3.Lerp (this.transform.position, targetPosition, (1 / (Vector3.Distance (this.transform.position, targetPosition))) * 0.1f);
			return;
		}
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;//DONT THINK IS REQUIRED ANYMORE CHECK LATER
		
		//set timevalue
		timeValue = (1 / (targetPosition - this.transform.position).magnitude);
		 if(moveUnit== true){

			if (DistanceCalculation (this.transform.position, targetPosition) == true) { 
				
				this.animation.Play("Walk");
				
				//Lerp to location of raycasted position.z
				this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, targetPosition.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, targetPosition.z)))) * 0.1f);
					
				if (this.transform.position.z <= targetPosition.z + 0.2f && this.transform.position.z >= targetPosition.z - 0.2f) {
						
				this.transform.LookAt (new Vector3 (targetPosition.x, this.transform.position.y, targetPosition.z));
					this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (targetPosition.x, this.transform.position.y, this.transform.position.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (targetPosition.x, this.transform.position.y, this.transform.position.z)))) * 0.1f);
				}
				GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueReader>().TaskCompletion(this.gameObject);
				
			} else if (DistanceCalculation (this.transform.position, targetPosition) == false) {

				this.animation.Play("Walk");
				
				//Lerp to location of raycasted position.x
				this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (targetPosition.x, this.transform.position.y, this.transform.position.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (targetPosition.x, this.transform.position.y, this.transform.position.z)))) * 0.1f);
					
				if (this.transform.position.x <= targetPosition.x + 0.2f && this.transform.position.x >= targetPosition.x - 0.2f) {
						
					this.transform.LookAt (new Vector3 (targetPosition.x, this.transform.position.y, targetPosition.z));
					this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, targetPosition.z), (1 / (Vector3.Distance (this.transform.position, new Vector3 (this.transform.position.x, this.transform.position.y, targetPosition.z)))) * 0.1f);
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
