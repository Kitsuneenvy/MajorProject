using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGenerics : MonoBehaviour
{
	float maxHealth;
	public float health;
	public float attack;
	public float defence;
	public float accuracy;
	public float dodge;
	float movement;
	RaycastHit info;
	bool attackState = false;
	bool movementState = false;
	float testX;
	float testY;
	float currentX;
	float currentY;
	float distanceX;
	float distanceY;
	float timer;
	Vector2 current;
	Vector2 test;
	public LayerMask gridMask;
	public Grid onGrid;
	ParticleSystem TempParticle;
	List<GameObject> adjacentSquares = new List<GameObject> ();
	public List<GameObject> moveableSquares = new List<GameObject> ();
	List<GameObject> AIThinkSquares = new List<GameObject>();
	List<GameObject> AITargetAdjacent = new List<GameObject>();
	List<GameObject> AIActionSquares = new List<GameObject>();
	List<GameObject> maxMove = new List<GameObject>();
	int AINumChoices;
	int ID; // Passed to this script by game manager upon initialisation
	public int unitType; // 1 = speed, 2 = attack, 3 = defence, 4 = healer
	SoundManager soundManagerObject;
	//This stuff is for AI decision rating
	List<GameObject> unitList = new List<GameObject>();
	List<Vector2> ratingsList = new List<Vector2>();
	GameObject unit;
	int ratingNum;
	int id;
	//End of AI
	

	// Use this for initialization if we need it
	void Start ()
	{
		soundManagerObject = GameObject.Find("Main Camera").GetComponent<SoundManager>();
	}
	//This function creates the unit statistics
	public void Initialise (int type) 
	{
		if (type == 0) {
			health = 60;
			attack = 20;
			defence = 10;
			accuracy = 100;
			dodge = 40;
			movement = 4;
		} else if (type == 1) {
			health = 80;
			attack = 30;
			defence = 10;
			accuracy = 70;
			dodge = 30;
			movement = 3;
			
		} else if (type == 2) {
			health = 100;
			attack = 25;
			defence = 15;
			accuracy = 80;
			dodge = 15;
			movement = 3;
			
		} else if (type == 3) {
			health = 40;
			attack = -30;
			defence = 5;
			accuracy = 100;
			dodge = 25;
			movement = 3;
		}
		maxHealth = health;
		unitType = type;
		
	}
	// Update is called once per frame
	void Update ()
	{
		if(health<0){
			health = 0;
			//destroy stuff
		}
		if (attackState == true) {
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out info, Mathf.Infinity, gridMask.value)) {
					if (info.collider.tag == "Square" && info.collider.GetComponent<Grid> ().returnUnit () != null) {
						if (adjacentSquares.Contains (info.collider.gameObject) && (info.collider.gameObject.GetComponent<Grid> ().returnUnit ().gameObject.tag != this.tag)) {
							launchAttack (info.collider.GetComponent<Grid> ().returnUnit ());
							attackState = false;
							AIThinkSquares.Clear();
						}
					}
				}
			}
				
		}
		if (movementState == true) {
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out info, Mathf.Infinity, gridMask.value)) {
					if (info.collider.tag == "Square" && info.collider.GetComponent<Grid> ().returnUnit () == null) {
						if (moveableSquares.Contains (info.collider.gameObject)) {
							this.GetComponent<AstarAI>().move (info.collider.gameObject);
							setMovementState(false);
							AIThinkSquares.Clear();
						}
					}
				}
			}
				
		}
	}

	public void launchAttack (GameObject target)
	{
		Debug.Log("attack called");
		setAttackState (false);
		GameObject.Find("Panel").GetComponent<DropDownMenu>().resetSelectedUnit();
		if(GameObject.Find("Game Manager").GetComponent<gameManage>().commandPoints>0){
			this.transform.LookAt(target.transform.position);
			GameObject.Find("Game Manager").GetComponent<gameManage>().commandPoints--;
			UnitGenerics targetGenerics;
			targetGenerics = target.GetComponent<UnitGenerics> ();
			if (Random.Range (0, 100) <= (accuracy - targetGenerics.dodge)) {
				TempParticle = target.GetComponent<ParticleSystem>();
				TempParticle.startColor = Color.red;
				TempParticle.startLifetime = 0.5f;
				TempParticle.enableEmission = true;
				TempParticle.loop = false;
				TempParticle.Play();
				if(unitType == 0)
				{
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip = soundManagerObject.returnTing();
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip);
				}
				else if(unitType == 1)
				{
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip = soundManagerObject.returnClang();
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip);
				}
				else if(unitType == 2)
				{
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip = soundManagerObject.returnClash();
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip);
				}
				else if(unitType == 3)
				{
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip = soundManagerObject.returnDetergent();
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().clip);
				}
				targetGenerics.setHealth (targetGenerics.health + (targetGenerics.defence - attack));
				
		
			}
			else
			{
//					info.collider.GetComponent<Grid>().returnUnit().gameObject.GetComponent<Animation>().Play("DodgeMain",PlayMode.StopAll);
			}
		} else {
			GameObject.Find("Game Manager").GetComponent<gameManage>().nextTurn();
		}
	}

	void setHealth (float newHealth)
	{
		health = newHealth;
	}
	public float getHealth(){
		return health;
	}
	public float getMax(){
		return maxHealth;
	}

	public void setGrid (Grid newGrid)
	{
		foreach (GameObject resetGrid in adjacentSquares) {
			resetGrid.renderer.material = Resources.Load ("Transparent") as Material;
		}
		foreach (GameObject resetGrid in moveableSquares) {
			resetGrid.renderer.material = Resources.Load ("Transparent") as Material;
		}
		AIThinkSquares.Clear();
		moveableSquares.Clear ();
		adjacentSquares.Clear ();
		onGrid = newGrid;
		currentX = onGrid.returnXY ().x;
		currentY = onGrid.returnXY ().y;
		current = new Vector2 (currentX, currentY);
		
		foreach (GameObject testGrid in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()) {
			testX = testGrid.GetComponent<Grid> ().returnXY ().x;
			testY = testGrid.GetComponent<Grid> ().returnXY ().y;
			test = new Vector2 (testX, testY);
			if ((onGrid.returnXY ().x + 1 == testGrid.GetComponent<Grid> ().returnXY ().x
				&& (onGrid.returnXY ().y == testGrid.GetComponent<Grid> ().returnXY ().y))
				|| (onGrid.returnXY ().x == testGrid.GetComponent<Grid> ().returnXY ().x)
				&& (onGrid.returnXY ().y + 1 == testGrid.GetComponent<Grid> ().returnXY ().y
				|| onGrid.returnXY ().y - 1 == testGrid.GetComponent<Grid> ().returnXY ().y)
				|| (onGrid.returnXY ().x - 1 == testGrid.GetComponent<Grid> ().returnXY ().x
				&& (onGrid.returnXY ().y == testGrid.GetComponent<Grid> ().returnXY ().y))) {
				adjacentSquares.Add (testGrid);
			}
			if (testX > currentX) {
				distanceX = testX - currentX;
			} else if (testX < currentX) {
				distanceX = currentX - testX;
			}
			if (testY > currentY) {
				distanceY = testY - currentY;
			} else if (testY < currentY) {
				distanceY = currentY - testY;
			}
			if (testX == currentX) {
				distanceX = 0;
			}
			if (currentY == testY) {
				distanceY = 0;
			}
			if (distanceX + distanceY <= movement) {
				if (test != current) {
					if (testGrid.GetComponent<Grid> ().returnUnit () == null) {
						if(!(moveableSquares.Contains(testGrid))){
							moveableSquares.Add (testGrid);
						}
					}
					if(!(AIThinkSquares.Contains(testGrid))){
						AIThinkSquares.Add(testGrid);
					}
				}
			}
		}
	}

	public void setAttackState (bool newValue)
	{
		attackState = newValue;
		if (attackState == true) {
			foreach (GameObject adjacentGrid in adjacentSquares) {
				adjacentGrid.renderer.material = Resources.Load ("HighlightSquare") as Material;
				adjacentGrid.renderer.material.color = Color.red;
			}
		} else if (attackState == false) {
			foreach (GameObject adjacentGrid in adjacentSquares) {
				adjacentGrid.renderer.material = Resources.Load ("Transparent") as Material;
				adjacentGrid.renderer.material.color = Color.black;
			}
		}
	}

	public void setMovementState (bool newValue)
	{
		movementState = newValue;
		if (movementState == true) {
			foreach (GameObject reachableGrid in moveableSquares) {
				reachableGrid.renderer.material = Resources.Load ("HighlightSquare") as Material;
				reachableGrid.renderer.material.color = Color.blue;
			}
		} else if (movementState == false) {
			foreach (GameObject reachableGrid in moveableSquares) {
				reachableGrid.renderer.material = Resources.Load ("Transparent") as Material;
				reachableGrid.renderer.material.color = Color.black;
			}
		}
	}
	//Beginning of AI functions
	public void AIThink(){
		ratingsList.Clear();
		AITargetAdjacent.Clear();
		unitList.Clear();
		//The ratings used for the various difficulty levels
		Vector2 highestRating = new Vector2(0,0);
		Vector2 middleRating = new Vector2(0,0);
		Vector2 lowestRating = new Vector2(0,0);
		//The target for whichever action the AI will take
		GameObject chosenTarget;
		//A loop to discern which grid squares the unit can reach
		foreach(GameObject testGrid in moveableSquares){
			foreach(GameObject secondTest in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()){
				if(secondTest==testGrid){
					if(!(AIThinkSquares.Contains(secondTest))){
						AIThinkSquares.Add(secondTest);
					}
				}
			}
		}
		//Refine the squares to only the squares that have units in them, to speed it up.
		foreach(GameObject thinkSquare in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()){
			if(thinkSquare.GetComponent<Grid>().heldUnit==null||thinkSquare.GetComponent<Grid>().heldUnit.tag=="Enemy"){
				if(AIThinkSquares.Contains(thinkSquare)){
					AIThinkSquares.Remove(thinkSquare);
				}
			}
		}
		if(AIThinkSquares.Count!=0){
		//Loop through the remaining squares and find out which ones have the best actions
		foreach(GameObject thinkSquare in AIThinkSquares){
			ratingNum = 0;
			UnitGenerics thinkUnit = thinkSquare.GetComponent<Grid>().heldUnit.GetComponent<UnitGenerics>();
			Grid thinkGrid = thinkSquare.GetComponent<Grid>();
			if(thinkGrid.heldUnit.tag==this.tag){
				if(health<maxHealth/2&&thinkUnit.unitType==4){
					unitList.Add(thinkSquare);
					ratingsList.Add(new Vector2(ratingsList.Count+1,8));
				}
				if(thinkUnit.getHealth()<thinkUnit.getMax()/2&&thinkUnit.unitType==4&&unitType==3){
					unitList.Add(thinkSquare);
					ratingsList.Add(new Vector2(ratingsList.Count+1,6));
				}
			} else if (thinkUnit.tag=="PlayerUnit"){
				if(thinkUnit.health<health){
					ratingNum = ratingNum+2;
				}
				if(thinkUnit.unitType==0&&unitType==2){
					ratingNum = ratingNum+2;
				}
				if(thinkUnit.unitType==1&&unitType==0){
					ratingNum = ratingNum+2;
				}
				if(thinkUnit.unitType==2&&unitType==1){
					ratingNum = ratingNum+2;
				}
				if(thinkUnit.unitType==3&&unitType!=3){
					ratingNum = ratingNum+3;
				}
				unitList.Add(thinkSquare);
				ratingsList.Add(new Vector2(ratingsList.Count+1,ratingNum));
			}
		}
		//find the various ratings for each difficulty
			foreach(GameObject square in unitList){
				if(returnRating(square).y>highestRating.y){
					highestRating.y = returnRating(square).y;
				}
				if(returnRating(square).y>middleRating.y&&returnRating(square).y<highestRating.y){
					middleRating.y = returnRating(square).y;
				}
				if(returnRating(square).y>lowestRating.y&&returnRating(square).y<middleRating.y){
					lowestRating.y = returnRating(square).y;
				}
			}
		//Decide on the target, next we do things for it
		chosenTarget = unitList[int.Parse(highestRating.x.ToString())];
		GameObject.Find("Game Manager").GetComponent<gameManage>().setActions(chosenTarget.GetComponent<Grid>().heldUnit.gameObject,this.gameObject,highestRating.y);
		//for simplicity
		Grid chosenGrid = chosenTarget.GetComponent<Grid>();
			Debug.Log(chosenGrid.returnXY().ToString());
		foreach(GameObject testGrid in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()){
		if ((chosenGrid.returnXY ().x + 1 == testGrid.GetComponent<Grid> ().returnXY ().x
				&& (chosenGrid.returnXY ().y == testGrid.GetComponent<Grid> ().returnXY ().y))
				|| (chosenGrid.returnXY ().x == testGrid.GetComponent<Grid> ().returnXY ().x)
				&& (chosenGrid.returnXY ().y + 1 == testGrid.GetComponent<Grid> ().returnXY ().y
				|| chosenGrid.returnXY ().y - 1 == testGrid.GetComponent<Grid> ().returnXY ().y)
				|| (chosenGrid.returnXY ().x - 1 == testGrid.GetComponent<Grid> ().returnXY ().x
				&& (chosenGrid.returnXY ().y == testGrid.GetComponent<Grid> ().returnXY ().y))) {
				AITargetAdjacent.Add (testGrid);
			}
		}
		} else if(AIThinkSquares.Count==0){
			GameObject temporary = null;
			float shortestDis = 999;
			foreach(GameObject square in GameObject.Find(("Game Manager")).GetComponent<GridTool>().returnGridColliders()){
					maxMove.Add(square);
			}
			foreach(GameObject square in GameObject.Find(("Game Manager")).GetComponent<GridTool>().returnGridColliders()){
				if(calculateGridDistance(onGrid.gameObject,square)>movement||square.GetComponent<Grid>().heldUnit!=null){
					maxMove.Remove(square);
				}
			}
			foreach(GameObject square in GameObject.Find(("Game Manager")).GetComponent<GridTool>().returnGridColliders()){
				if(square.GetComponent<Grid>().heldUnit!=null&&square.GetComponent<Grid>().heldUnit.tag=="PlayerUnit"){
					AIThinkSquares.Add(square);
				}
			}
			foreach(GameObject square in maxMove){
				foreach(GameObject checkSquare in AIThinkSquares){
					if(calculateGridDistance(square,checkSquare)<shortestDis){
						shortestDis = calculateGridDistance(square,checkSquare);
						temporary = square;
					}
				}
			}
			//Debug.Log(temporary.GetComponent<Grid>().returnXY().ToString());
			GameObject.Find("Game Manager").GetComponent<gameManage>().setActions(temporary.gameObject,this.gameObject,4);
		
			
		}
		
	}
	//A function to return the given rating for any gameobject. Returns vector2.zero if the object is not in the list
	Vector2 returnRating(GameObject unitToCheck){
		int count = 0;
		foreach(GameObject check in unitList){
			if(check==unitToCheck){
				//count++;
				return ratingsList[count];
			} else {
				count++;
			}
		}
		return new Vector2(0,0);
	}
	//Calculate the distance in grid units between two places on the grid.
	public float calculateGridDistance(GameObject targetSquare1, GameObject targetSquare2){
		Grid targetGrid1 = targetSquare1.GetComponent<Grid>();
		Grid targetGrid2 = targetSquare2.GetComponent<Grid>();
		float x1 = targetGrid1.returnXY().x;
		float y1 = targetGrid1.returnXY().y;
		float x2 = targetGrid2.returnXY().x;
		float y2 = targetGrid2.returnXY().y;
		float disX = 0;
		float disY = 0;
		float result = 0;
		if (x1 > x2) {
				disX = x1 - x2;
			} else if (x2 > x1) {
				disX = x2 - x1;
			}
			if (y2 > y1) {
				disY = y2 - y1;
			} else if (y1 > y2) {
				disY = y1 - y2;
			}
			if (x2 == x1) {
				disX = 0;
			}
			if (y1 == y2) {
				disY = 0;
			}
		result = disX+disY;
		return result;
	}
	
	public List<GameObject> checkAdjacentGrids(GameObject checkObj){
		List<GameObject> returnAdjacent = new List<GameObject>();
		Grid checkGrid = checkObj.GetComponent<Grid>();
		foreach(GameObject testGrid in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()){
		if ((checkGrid.returnXY ().x + 1 == testGrid.GetComponent<Grid> ().returnXY ().x
				&& (checkGrid.returnXY ().y == testGrid.GetComponent<Grid> ().returnXY ().y))
				|| (checkGrid.returnXY ().x == testGrid.GetComponent<Grid> ().returnXY ().x)
				&& (checkGrid.returnXY ().y + 1 == testGrid.GetComponent<Grid> ().returnXY ().y
				|| checkGrid.returnXY ().y - 1 == testGrid.GetComponent<Grid> ().returnXY ().y)
				|| (checkGrid.returnXY ().x - 1 == testGrid.GetComponent<Grid> ().returnXY ().x
				&& (checkGrid.returnXY ().y == testGrid.GetComponent<Grid> ().returnXY ().y))) {
				returnAdjacent.Add (testGrid);
			}
		
	}
		return returnAdjacent;
	}
	//End of AI Functions
}
