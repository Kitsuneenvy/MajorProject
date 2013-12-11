using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGenerics : MonoBehaviour
{
	public float maxHealth;
	public float health;
	public float attack;
	public float defence;
	public float accuracy;
	public float dodge;
	public float movement;
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
	public LayerMask unitMask;
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
	public int unitType; // 0 = speed, 1 = attack, 2 = defence, 3 = healer, 4 = flower
	gameManage gameManageObject;
	MissionReader missionReaderObject;
	SoundManager soundObject;
	//This stuff is for AI decision rating
	List<GameObject> unitList = new List<GameObject>();
	List<Vector2> ratingsList = new List<Vector2>();
	public bool statsIncreased = true;
	GameObject unit;
	int ratingNum;
	int id;
	GameObject damageText;
	//End of AI

	// Use this for initialization if we need it
	void Start ()
	{
		gameManageObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>();
		soundObject = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundManager>();
		missionReaderObject = GameObject.Find("A*").GetComponent<MissionReader>();
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
			attack = 40;
			defence = 10;
			accuracy = 70;
			dodge = 30;
			movement = 3;
		} else if (type == 2) {
			health = 100;
			attack = 20;
			defence = 15;
			accuracy = 80;
			dodge = 15;
			movement = 3;
		} else if (type == 3) {
			health = 40;
			attack = 30;
			defence = 5;
			accuracy = 100;
			dodge = 25;
			movement = 3;
		} else if (type == 4) {
			health = 1;
			attack = 0;
			defence = 0;
			accuracy = 0;
			dodge = 0;
			movement = 0;
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
				gameManageObject.resetInactive();
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out info, Mathf.Infinity, gridMask.value)) {
					if (info.collider.tag == "Square" && info.collider.GetComponent<Grid> ().returnUnit () != null) {
						if (adjacentSquares.Contains (info.collider.gameObject) && ((info.collider.gameObject.GetComponent<Grid> ().returnUnit ().gameObject.tag != this.tag || (info.collider.gameObject.GetComponent<Grid> ().returnUnit ().gameObject.tag == this.tag && unitType == 3)))) {
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
				gameManageObject.resetInactive();
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out info, Mathf.Infinity, gridMask.value)) {
					if (info.collider.tag == "Square" && info.collider.GetComponent<Grid> ().returnUnit () == null) {
						if (moveableSquares.Contains (info.collider.gameObject)) {
							this.GetComponent<AstarAI>().move (info.collider.gameObject, moveableSquares);
							movementState = false;
							AIThinkSquares.Clear();
						}
					}
				}
			}
				
		}
		
	}

	public void launchAttack (GameObject target)
	{
		gameManageObject.resetInactive();
		int bonusHit = 0;
		int bonusDamage = 0;
		setAttackState (false);
		
		GameObject.Find("Panel").GetComponent<DropDownMenu>().resetSelectedUnit();
		if(GameObject.Find("Game Manager").GetComponent<gameManage>().commandPoints>0){
			
		this.animation.Play("Attack",PlayMode.StopAll);
			if(this.GetComponent<AstarAI>().myTurn == true){
				this.GetComponent<AstarAI>().myTurn = false;
				GameObject.Find("Game Manager").GetComponent<gameManage>().toggleTurn();
			}
			this.transform.LookAt(new Vector3(target.transform.position.x,this.transform.position.y,target.transform.position.z)); // Don't look at the sky randomly.
			GameObject.Find("Game Manager").GetComponent<gameManage>().commandPoints--;
			UnitGenerics targetGenerics;
			targetGenerics = target.GetComponent<UnitGenerics> ();
			if(target.tag!="Flower"){
				target.transform.LookAt(new Vector3(this.transform.position.x,target.transform.position.y,this.transform.position.z));
			}
			GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<SecondaryCamera>().setFocus(this.gameObject,target.gameObject);
			GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<SecondaryCamera>().setActive(true);
			if(unitType==2&&targetGenerics.unitType==0||unitType==1&&targetGenerics.unitType==2||unitType==0&&targetGenerics.unitType==1){
				bonusHit = 20;
				bonusDamage = 20;
			}
			if(unitType==3){
				bonusHit+=100;
			}
			if ((Random.Range (0, 100) <= (accuracy +bonusHit - targetGenerics.dodge))||target.tag == "Flower") {
				if(!target.name.Contains("Flower"))
				{
					TempParticle = target.GetComponent<ParticleSystem>();
					TempParticle.startColor = Color.red;
					TempParticle.startLifetime = 0.5f;
					TempParticle.enableEmission = true;
					TempParticle.loop = false;
					TempParticle.Play();
				}
				if(target.tag!="Flower"){
					if(targetGenerics.health - (attack +bonusDamage - targetGenerics.defence)>0){
						StartCoroutine(animationQ(target.gameObject,"TakenHit"));
					}
				}
				if(unitType!=3){
					damageText = GameObject.Instantiate(Resources.Load("DamageText"),new Vector3((this.transform.position.x+target.transform.position.x)/2,(this.transform.position.y+target.transform.position.y)/2+5,(this.transform.position.z+target.transform.position.z)/2),Quaternion.identity) as GameObject;
					damageText.GetComponent<TextMesh>().color = Color.white;
					damageText.GetComponent<TextMesh>().text = "-"+(attack+bonusDamage - targetGenerics.defence).ToString();
					if((attack - targetGenerics.defence) > 0){
						targetGenerics.setHealth (targetGenerics.health - (attack +bonusDamage - targetGenerics.defence));
					} else {
						targetGenerics.setHealth (targetGenerics.health - 1);
					}
				} else {
					if(target.tag == this.tag){
						damageText = GameObject.Instantiate(Resources.Load("DamageText"),new Vector3((this.transform.position.x+target.transform.position.x)/2,(this.transform.position.y+target.transform.position.y)/2+5,(this.transform.position.z+target.transform.position.z)/2),Quaternion.identity) as GameObject;
						damageText.GetComponent<TextMesh>().color = Color.white;
						damageText.GetComponent<TextMesh>().text = "+"+(attack).ToString();
						if(targetGenerics.health+attack>targetGenerics.maxHealth){
							targetGenerics.health = targetGenerics.maxHealth;
						} else {
							targetGenerics.setHealth(targetGenerics.health + attack);
						}
					} else {
						damageText = GameObject.Instantiate(Resources.Load("DamageText"),new Vector3((this.transform.position.x+target.transform.position.x)/2,(this.transform.position.y+target.transform.position.y)/2+5,(this.transform.position.z+target.transform.position.z)/2),Quaternion.identity) as GameObject;
						damageText.GetComponent<TextMesh>().color = Color.white;
						damageText.GetComponent<TextMesh>().text = "-"+(attack+bonusDamage - targetGenerics.defence).ToString();
						if(attack - targetGenerics.defence > 0){
							targetGenerics.setHealth (targetGenerics.health - (attack +bonusDamage - targetGenerics.defence));
						} else {
							targetGenerics.setHealth (targetGenerics.health - 1);
						}
					}
				}
				if(Random.Range(0,100)<50){
					if(Random.Range(0,100)<25){
						switch(unitType){
							case(0):
							{
								if(tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.mowerAudio[0]);
								}
								if(tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.frierAudio[0]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							case(1):
							{
								if(tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.prunerAudio[0]);
								}
								if(tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.ladlewightAudio[0]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							case(2):
							{
								if(tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.potterAudio[0]);
								}
								if(tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.bowlderAudio[0]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							case(3):
							{
								if(tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.floristAudio[0]);
								}
								if(tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.chefAudio[0]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							default:
							{
								break;
							}
						}
					}
				} else {
					if(Random.Range(0,100)<25){
						switch(targetGenerics.unitType){
							case(0):
							{
								if(target.tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.mowerAudio[4]);
								}
								if(target.tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.frierAudio[4]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							case(1):
							{
								if(target.tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.prunerAudio[4]);
								}
								if(target.tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.ladlewightAudio[4]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							case(2):
							{
								if(target.tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.potterAudio[4]);
								}
								if(target.tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.bowlderAudio[4]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							case(3):
							{
								if(target.tag == "Enemy"){
									soundObject.soundEffects.clip = (soundObject.floristAudio[4]);
								}
								if(target.tag == "PlayerUnit"){
									soundObject.soundEffects.clip = (soundObject.chefAudio[4]);
								}
								soundObject.soundEffects.Play();
								break;
							}
							default:
							{
								break;
							}
						}
					}
				}
				if(targetGenerics.getHealth()<= 0)
				{
					if(Random.Range(0,100)<100){
					switch(targetGenerics.unitType){
						case(0):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.mowerAudio[3]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.frierAudio[3]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						case(1):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.prunerAudio[3]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.ladlewightAudio[3]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						case(2):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.potterAudio[3]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.bowlderAudio[3]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						case(3):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.floristAudio[3]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.chefAudio[3]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						default:
						{
							break;
						}
					}
				}
					//play death animation
					if(target.tag!="Flower"){
						StartCoroutine(animationQ(target.gameObject,"Death2"));
						for(int i = 0; i < 6; i++)
						{
							StartCoroutine(particleEmissionIncrease(targetGenerics.gameObject));
						}
					} else {
						GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<SecondaryCamera>().setActive(false);
					}
					//flower buff removal
					if(target.name.Contains("Flower"))
					{
						List<GameObject> tempList = new List<GameObject>();
						tempList = checkAdjacentGrids(targetGenerics.onGrid.gameObject);
						foreach(GameObject tile in tempList)
						{
							
							if(tile.particleEmitter != null)
							{
								tile.particleEmitter.maxEmission = 0;
								if(tile.GetComponent<Grid>().heldUnit != null &&tile.GetComponent<Grid>().heldUnit.tag == "Enemy"&& tile.GetComponent<Grid>().heldUnit.GetComponent<UnitGenerics>().statsIncreased == true)
								{
									
									tile.GetComponent<Grid>().heldUnit.GetComponent<UnitGenerics>().attack -= 10;
									
									tile.GetComponent<Grid>().heldUnit.GetComponent<UnitGenerics>().statsIncreased = false;
								}
							}
						}
						tempList.Clear();
					}
					//remove from list
					missionReaderObject.allUnits.Remove(targetGenerics.gameObject);
					missionReaderObject.enemyUnits.Remove(targetGenerics.gameObject);
					
					Debug.Log(missionReaderObject.enemyUnits.Count);
					targetGenerics.onGrid.heldUnit = null;
					foreach(GameObject unit in missionReaderObject.allUnits){
						unit.GetComponent<UnitGenerics>().refreshMovement();
					}
					//destroy object
					if(target.tag!="Flower"){
						//Destroy(targetGenerics.gameObject,targetGenerics.gameObject.animation["Death2"].length);
					} else {
						Destroy(targetGenerics.gameObject);
						missionReaderObject.flowerUnits.Remove(targetGenerics.gameObject);
					}
					//check if task is completed
					GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueReader>().TaskCompletion(null);
				}
			} else {
				damageText = GameObject.Instantiate(Resources.Load("DamageText"),new Vector3((this.transform.position.x+target.transform.position.x)/2,(this.transform.position.y+target.transform.position.y)/2+5,(this.transform.position.z+target.transform.position.z)/2),Quaternion.identity) as GameObject;
				damageText.GetComponent<TextMesh>().text = "Miss!";
				StartCoroutine(animationQ(target.gameObject,"Dodge"));
				if(Random.Range(0,100)<25){
					switch(targetGenerics.unitType){
						case(0):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.mowerAudio[2]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.frierAudio[2]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						case(1):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.prunerAudio[2]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.ladlewightAudio[2]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						case(2):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.potterAudio[2]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.bowlderAudio[2]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						case(3):
						{
							if(target.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.floristAudio[2]);
							}
							if(target.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.chefAudio[2]);
							}
							soundObject.soundEffects.Play();
							break;
						}
						default:
						{
							break;
						}
					}
				}
//					info.collider.GetComponent<Grid>().returnUnit().gameObject.GetComponent<Animation>().Play("DodgeMain",PlayMode.StopAll);
			}
		} else {
			//GameObject.Find("Game Manager").GetComponent<gameManage>().nextTurn();
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
		onGrid.heldUnit = this.gameObject;
		currentX = onGrid.returnXY ().x;
		currentY = onGrid.returnXY ().y;
		current = new Vector2 (currentX, currentY);
		foreach (GameObject testGrid in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()) {
			if (checkAdjacentGrids(onGrid.gameObject).Contains(testGrid)) {
				adjacentSquares.Add (testGrid);
			}
		}
		moveableSquares.Add(onGrid.gameObject);
		for(int j = 0; j<movement; j++){
			foreach(GameObject tile in moveableSquares){
				GameObject currentCheckTile = tile;
				int tilesAway = 0;
				for(int i = 0; i<4; i++){
					foreach(GameObject secondTest in checkAdjacentGrids(tile)){
						if(currentCheckTile.name.Contains("48")){
						}
						if(currentCheckTile.GetComponent<Grid>().returnUnit()!=null&&currentCheckTile.GetComponent<Grid>().returnUnit()==this.gameObject){
							if(tilesAway< movement){
								if(!AIThinkSquares.Contains(secondTest)){
									AIThinkSquares.Add(secondTest);
								}
							}	
						} else {
							currentCheckTile = closestAdjacent(checkAdjacentGrids(currentCheckTile));
							tilesAway++;
						}
					}
				}
			}
			foreach(GameObject tile in AIThinkSquares){
				if(tile.GetComponent<Grid>().returnUnit()==null){
					if(!moveableSquares.Contains(tile)){
						moveableSquares.Add(tile);
					}
				}
			}
		}
		moveableSquares.Remove(onGrid.gameObject);
		AIThinkSquares.Remove(onGrid.gameObject);
		if(missionReaderObject!=null){
			if(missionReaderObject.returnLayoutCompleted()==true){
				foreach(GameObject unit in missionReaderObject.allUnits){
					unit.GetComponent<UnitGenerics>().refreshMovement();
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
		//The rating of the best unit
		float highestRating = 0;
		//The target for whichever action the AI will take
		GameObject chosenTarget = null;
		float shortestDistance = Mathf.Infinity;
		foreach(GameObject testUnit in missionReaderObject.allUnits){
			if(testUnit.tag=="PlayerUnit"){
				if(calculateGridDistance(testUnit.GetComponent<UnitGenerics>().onGrid.gameObject,this.GetComponent<UnitGenerics>().onGrid.gameObject)<shortestDistance){
					shortestDistance = calculateGridDistance(testUnit.GetComponent<UnitGenerics>().onGrid.gameObject,this.GetComponent<UnitGenerics>().onGrid.gameObject);
					chosenTarget = testUnit;
				} else if (calculateGridDistance(testUnit.GetComponent<UnitGenerics>().onGrid.gameObject,this.GetComponent<UnitGenerics>().onGrid.gameObject)==shortestDistance){
					if(calculateRating(chosenTarget, this.gameObject)<calculateRating(testUnit,this.gameObject)){
						chosenTarget = testUnit;
					}
				}
			}
		}
		highestRating = calculateRating(chosenTarget, this.gameObject);
		gameManageObject.setActions(chosenTarget,this.gameObject,highestRating);
		
		

		
	}
	
	float calculateRating(GameObject target, GameObject unit){
		float ratingReturn = 0;
		if(target.GetComponent<UnitGenerics>().health/target.GetComponent<UnitGenerics>().maxHealth<health/maxHealth){
			ratingReturn = ratingReturn+2;
		}
		if(target.GetComponent<UnitGenerics>().unitType==0&&unit.GetComponent<UnitGenerics>().unitType==2){
			ratingReturn = ratingReturn+2;
		}
		if(target.GetComponent<UnitGenerics>().unitType==1&&unit.GetComponent<UnitGenerics>().unitType==0){
			ratingReturn = ratingReturn+2;
		}
		if(target.GetComponent<UnitGenerics>().unitType==2&&unit.GetComponent<UnitGenerics>().unitType==1){
			ratingReturn = ratingReturn+2;
		}
		if(target.GetComponent<UnitGenerics>().unitType==3&&unit.GetComponent<UnitGenerics>().unitType!=3){
			ratingReturn = ratingReturn+3;
		}
		return ratingReturn;
	}
	
	//A function to return the given rating for any gameobject. Returns vector2.zero if the object is not in the list
	//Not necessary with new AI.
	Vector2 returnRating(GameObject unitToCheck){
		int count = 0;
		foreach(GameObject check in unitList){
			if(check==unitToCheck){
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
		List<GameObject> gridList = GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders();
		int checkIndex = gridList.IndexOf(checkObj);
		List<GameObject> returnAdjacent = new List<GameObject>();
		RaycastHit checkHit;
		if(checkIndex<=gridList.Count){
			if(checkIndex == GameObject.Find("A*").GetComponent<AstarPath>().astarData.gridGraph.width)
			{
				returnAdjacent.Add(gridList[0]);
			}
			if(checkIndex!=0){
				if(checkIndex%GameObject.Find("A*").GetComponent<AstarPath>().astarData.gridGraph.width!=0){
					returnAdjacent.Add(gridList[checkIndex-1]);
				}
			}
		}
		if(checkIndex+GameObject.Find("A*").GetComponent<AstarPath>().astarData.gridGraph.width<gridList.Count){
			returnAdjacent.Add(gridList[checkIndex+GameObject.Find("A*").GetComponent<AstarPath>().astarData.gridGraph.width]);
		}
		if(checkIndex-GameObject.Find("A*").GetComponent<AstarPath>().astarData.gridGraph.width>0){
			returnAdjacent.Add(gridList[checkIndex-GameObject.Find("A*").GetComponent<AstarPath>().astarData.gridGraph.width]);
		}
		if(checkIndex+1!=gridList.Count){
			if((checkIndex+1)%GameObject.Find("A*").GetComponent<AstarPath>().astarData.gridGraph.width!=0){
				returnAdjacent.Add(gridList[checkIndex+1]);
			}
		}
		return returnAdjacent;
	}
	
	public GameObject closestAdjacent(List<GameObject> adjacentList){
		float shortestDistance = Mathf.Infinity;
		GameObject closest = null;
		foreach(GameObject tile in adjacentList){
			if(Vector3.Distance(tile.transform.position,this.transform.position)<shortestDistance){
				shortestDistance = Vector3.Distance(tile.transform.position,this.transform.position);
				closest = tile;
			}
		} 
		return closest;
	}
	//End of AI Functions
	
	
	//Called to tell all the other units to update for the newly moved unit. Memory hog, but unless you can think of a better way...?
		public void refreshMovement(){
		moveableSquares.Clear();
		adjacentSquares.Clear();
		AIThinkSquares.Clear();
		foreach (GameObject testGrid in GameObject.Find("Game Manager").GetComponent<GridTool>().returnGridColliders()) {
			if (checkAdjacentGrids(onGrid.gameObject).Contains(testGrid)) {
				adjacentSquares.Add (testGrid);
			}
		}
		moveableSquares.Add(onGrid.gameObject);
		for(int j = 0; j<movement; j++){
			foreach(GameObject tile in moveableSquares){
				GameObject currentCheckTile = tile;
				int tilesAway = 0;
				for(int i = 0; i<4; i++){
					foreach(GameObject secondTest in checkAdjacentGrids(tile)){
						if(currentCheckTile.name.Contains("48")){
						}
						if(currentCheckTile.GetComponent<Grid>().returnUnit()!=null&&currentCheckTile.GetComponent<Grid>().returnUnit()==this.gameObject){
							if(tilesAway< movement){
								if(!AIThinkSquares.Contains(secondTest)){
									AIThinkSquares.Add(secondTest);
								}
							}	
						} else {
							currentCheckTile = closestAdjacent(checkAdjacentGrids(currentCheckTile));
							tilesAway++;
						}
					}
				}
			}
			foreach(GameObject tile in AIThinkSquares){
				if(tile.GetComponent<Grid>().returnUnit()==null){
					if(!moveableSquares.Contains(tile)){
						moveableSquares.Add(tile);
					}
				}
			}
		}
		moveableSquares.Remove(onGrid.gameObject);
		AIThinkSquares.Remove(onGrid.gameObject);
	}
	
	IEnumerator animationQ(GameObject target, string animationToPlay){
		yield return new WaitForSeconds(this.animation.GetClip("Attack").length);
		if(target!=null){
		target.animation.Play(animationToPlay);
		yield return new WaitForSeconds(target.animation.GetClip(animationToPlay).length);
		}
		if(animationToPlay == "Death2"){
			DestroyImmediate(target.gameObject);
		}
		GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<SecondaryCamera>().setActive(false);
	}
	
	IEnumerator particleEmissionIncrease(GameObject targetChar)
	{
		Color[] animatorColours = targetChar.GetComponent<ParticleAnimator>().colorAnimation;
		animatorColours[0] = new Color(0f,0f,0f,1f);
		animatorColours[1] = new Color(0f,0f,0f,1f);
		animatorColours[2] = new Color(0f,0f,0.0f,1f);
		animatorColours[3] = new Color(0f,0f,0f,1f);
		animatorColours[4] = new Color(0f,0f,0f,1f);
		targetChar.GetComponent<ParticleAnimator>().colorAnimation = animatorColours;
		targetChar.GetComponent("EllipsoidParticleEmitter").particleEmitter.maxSize = 50;
		targetChar.GetComponent("EllipsoidParticleEmitter").particleEmitter.minSize = 50;
		targetChar.GetComponent("EllipsoidParticleEmitter").particleEmitter.maxEmission += 50;
		yield return new WaitForSeconds(0.2f);
	}
}
