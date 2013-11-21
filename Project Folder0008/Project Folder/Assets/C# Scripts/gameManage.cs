using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameManage : MonoBehaviour {
	Grid hoverGrid; //The grid you are hovering over.
	MissionReader mReaderObject;
	SoundManager soundObject;
	
	bool missionOne = true;
	bool turnEnded = false;
	
	GameObject temp;
	GameObject NarrativeAnchorObject;
	
	float gridPosx;
	float gridPosy;
	float gridPosz;
	float gridWidth;
	float gridLength;
	float nodeWidthCount;
	float nodeLengthCount;
	
	public float commandPoints = 5;
	
	public bool playerTurn;
	public bool narrativePanelOpen = false;
	
	public LayerMask gridMask;
	public LayerMask astarMask;
	public GameObject endTurnButton;
	
	RaycastHit info;
	
	float timer;
	float dialogueTimer = 15;
	List<GameObject> chosenTargets = new List<GameObject>();
	List<GameObject> sendUnits = new List<GameObject>();
	List<Vector2> chosenRatings = new List<Vector2>();
	
	int turnCounter = 0;
	
	// Use this for initialization
	void Start () {
		NarrativeAnchorObject = GameObject.FindGameObjectWithTag("NarrativeAnchor");
		//gridMask = LayerMask.NameToLayer("Grid");
		//astarMask = LayerMask.NameToLayer("AStar");
		playerTurn = true;	
		mReaderObject = GameObject.FindGameObjectWithTag("Grid").GetComponent<MissionReader>();
		soundObject = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		if(Input.GetKeyDown(KeyCode.H)){
			if(narrativePanelOpen == false){
				narrativePanelOpen = true;
			} else if (narrativePanelOpen == true){
				narrativePanelOpen = false;
			}
		}
		if(Input.GetKeyDown(KeyCode.S)){
			nextTurn();
		}
		if(Input.GetKey(KeyCode.Space)){
			foreach(GameObject gridObject in this.GetComponent<GridTool>().returnGridColliders()){
				gridObject.renderer.material.color = Color.black;
			}
		}
		if(narrativePanelOpen==false){
			if(dialogueTimer<=0){
				UnitGenerics randomUnit = mReaderObject.allUnits[Random.Range(0,mReaderObject.allUnits.Count)].GetComponent<UnitGenerics>();
				Debug.Log(randomUnit.tag.ToString());
				switch(randomUnit.unitType){
					case(0):
						{
							if(randomUnit.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.mowerAudio[1]);
							}
							if(randomUnit.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.frierAudio[1]);
							}
							soundObject.soundEffects.Play();
							dialogueTimer = 15;
							break;
						}
						case(1):
						{
							if(randomUnit.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.prunerAudio[1]);
							}
							if(randomUnit.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.ladlewightAudio[1]);
							}
							soundObject.soundEffects.Play();
							dialogueTimer = 15;
							break;
						}
						case(2):
						{
							if(randomUnit.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.potterAudio[1]);
							}
							if(randomUnit.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.bowlderAudio[1]);
							}
							soundObject.soundEffects.Play();
							dialogueTimer = 15;
							break;
						}
						case(3):
						{
							if(randomUnit.tag == "Enemy"){
								soundObject.soundEffects.clip = (soundObject.floristAudio[1]);
							}
							if(randomUnit.tag == "PlayerUnit"){
								soundObject.soundEffects.clip = (soundObject.chefAudio[1]);
							}
							soundObject.soundEffects.Play();
							dialogueTimer = 15;
							break;
						}
						default:
						{
							dialogueTimer = 15;
							break;
						}
				}
			} else {
				dialogueTimer -= Time.deltaTime;
			}
			NarrativeAnchorObject.SetActive(false);
			if(mReaderObject.returnLayoutCompleted() == true && this.GetComponent<GridTool>().returnGridColliders().Count > 0)
			{
			foreach(GameObject gridObject in this.GetComponent<GridTool>().returnGridColliders()){
				if(gridObject.renderer.material.ToString() == "Transparent (Instance) (UnityEngine.Material)"){
					gridObject.renderer.material.color = Color.black;
				} else if (gridObject.renderer.material.ToString() == "HighlightHoverAttack (Instance) (UnityEngine.Material)"){
					gridObject.renderer.material = Resources.Load("HighlightSquare") as Material;
					gridObject.renderer.material.color = Color.red;
				} else if (gridObject.renderer.material.ToString() == "HighlightHoverMove (Instance) (UnityEngine.Material)"){
					gridObject.renderer.material = Resources.Load("HighlightSquare") as Material;
					gridObject.renderer.material.color = Color.blue;
				}
					
			}
			}
			if(playerTurn==false){
				if(turnEnded==true){
					if(timer>0){
						timer-=Time.deltaTime;
					} else {
						turnEnded = false;
						timer = 1;
						if(commandPoints!=0){
							chosenTargets.Clear();
							chosenRatings.Clear();
							sendUnits.Clear();
							foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
								enemy.GetComponent<UnitGenerics>().AIThink();
							}
							decideAction();
						} else {
							nextTurn();
						}
					} 
				}
			}
		
		
		hoverOverGrid();
		GameObject.Find("Command Points").GetComponent<UILabel>().text = "Command Points: " + commandPoints.ToString();
		} else {
			NarrativeAnchorObject.SetActive(true);
		} 
	}
	
	//returns x,y,z
	public Vector3 gridPositions()
	{
		return new Vector3(gridPosx,gridPosy,gridPosz);
	}
	
	//returns length and width
	public Vector2 gridArea()
	{
		return new Vector2(gridWidth,gridLength);
	}
	
	//returns node amount for width and length
	public Vector2 gridNodeCount()
	{
		return new Vector2(nodeWidthCount, nodeLengthCount);
	}
	//This is called every frame, and checks if you are hovering over a square. If you are, show the highlight effect.
	void hoverOverGrid(){
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out info,Mathf.Infinity,gridMask.value)){
			if(info.collider.tag=="Square"){
				if(info.collider.renderer.material.ToString() == "Transparent (Instance) (UnityEngine.Material)"){
				info.collider.gameObject.renderer.material.color = Color.yellow;
				hoverGrid = info.collider.gameObject.GetComponent<Grid>();
				} else if (info.collider.renderer.material.ToString() == "HighlightSquare (Instance) (UnityEngine.Material)"){
					if(info.collider.renderer.material.color == Color.red){
					info.collider.renderer.material = Resources.Load("HighlightHoverAttack") as Material;
					} else if (info.collider.renderer.material.color == Color.blue){
						info.collider.renderer.material = Resources.Load("HighlightHoverMove") as Material;
					}
				}
				if(info.collider.GetComponent<Grid>().heldUnit!=null){
					UnitGenerics hoverUnit = info.collider.GetComponent<Grid>().heldUnit.GetComponent<UnitGenerics>();
					GameObject.Find("Character Name").GetComponent<UILabel>().text = "Name: " + hoverUnit.name;
					GameObject.Find("Character Attributes").GetComponent<UILabel>().text = 
						"Att: " +hoverUnit.attack.ToString() +
						"  Def: " + hoverUnit.defence.ToString()+
						"  Acc: " + hoverUnit.accuracy.ToString() +
						"  Ddg: " + hoverUnit.dodge;
					GameObject.Find("Character HP").GetComponent<UILabel>().text = "HP:  " +hoverUnit.health.ToString();
				} else {
					GameObject.Find("Character Name").GetComponent<UILabel>().text = "Name:";
					GameObject.Find("Character Attributes").GetComponent<UILabel>().text = 
						"Att:" +
						"    Def:" +
						"    Acc:" +
						"    Ddg:";
					GameObject.Find("Character HP").GetComponent<UILabel>().text = "HP:";
				}
			}
		}
	}
	//This is called when the player ends their turn or the AI runs out of command points. It resets the command points and AI stuff.
	public void nextTurn(){
		timer = 3;
		commandPoints = 5;
		turnCounter++;
		
		if(playerTurn){
			endTurnButton.SetActive(false);
			chosenTargets.Clear();
			chosenRatings.Clear();
			sendUnits.Clear();
			playerTurn=false;
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
				enemy.GetComponent<UnitGenerics>().AIThink();
			}
			decideAction();
		} else {
			endTurnButton.SetActive(true);
			playerTurn = true;
		}
		if(turnCounter == 2 && Application.loadedLevelName == "Tutorial")
		{
			narrativePanelOpen = true;
		}
	}
	//The AI uses this to determine the best action to take currently
	public void decideAction(){
		bool adjacent = false;
		float count = 0;
		int listCount = 0;
		float max = 0;
		float shortestDistance = Mathf.Infinity;
		GameObject moveSquare = null;
		foreach(Vector2 rating in chosenRatings){
			if(rating.y > max){
				max = rating.y;
				count = rating.x;
			}
			listCount = (int)count;
		}
		if(sendUnits[listCount].GetComponent<UnitGenerics>().checkAdjacentGrids(chosenTargets[listCount].GetComponent<UnitGenerics>().onGrid.gameObject).Contains(sendUnits[listCount].GetComponent<UnitGenerics>().onGrid.gameObject)){
			sendUnits[listCount].GetComponent<AstarAI>().myTurn = true;
			sendUnits[listCount].GetComponent<UnitGenerics>().launchAttack(chosenTargets[listCount]);
			chosenTargets.Clear();
			sendUnits.Clear();
			chosenRatings.Clear();
		} else {
			if(chosenTargets[listCount].GetComponent<Grid>()==null){
			foreach(GameObject square in sendUnits[listCount].GetComponent<UnitGenerics>().checkAdjacentGrids(chosenTargets[listCount].GetComponent<UnitGenerics>().onGrid.gameObject)){
					if(square.GetComponent<Grid>().returnUnit()==null){
						if(sendUnits[listCount].GetComponent<UnitGenerics>().calculateGridDistance(sendUnits[listCount].GetComponent<UnitGenerics>().onGrid.gameObject,square)<shortestDistance){
							shortestDistance = sendUnits[listCount].GetComponent<UnitGenerics>().calculateGridDistance(sendUnits[listCount].GetComponent<UnitGenerics>().onGrid.gameObject,square);
							moveSquare = square;
						}
					}
				}
				
			}
			if(sendUnits[listCount].GetComponent<UnitGenerics>().calculateGridDistance(sendUnits[listCount].GetComponent<UnitGenerics>().onGrid.gameObject,moveSquare)>sendUnits[listCount].GetComponent<UnitGenerics>().movement){
				float distanceSquare = Mathf.Infinity;
				GameObject tempTile = null;
				foreach(GameObject tile in sendUnits[listCount].GetComponent<UnitGenerics>().moveableSquares){
					if(sendUnits[listCount].GetComponent<UnitGenerics>().calculateGridDistance(tile,moveSquare)<distanceSquare){
						distanceSquare = sendUnits[listCount].GetComponent<UnitGenerics>().calculateGridDistance(tile,moveSquare);
						tempTile = tile;
					}
				}
				moveSquare = tempTile;
			}
			sendUnits[listCount].GetComponent<AstarAI>().myTurn = true;
			sendUnits[listCount].GetComponent<AstarAI>().move(moveSquare);
			chosenTargets.Clear();
			sendUnits.Clear();
			chosenRatings.Clear();
		}
	}
	//Set the actions that each unit has decided is best for it in a list.
	public void setActions(GameObject unitTarget,GameObject sendingUnit, float ratings){
		chosenTargets.Add(unitTarget);
		sendUnits.Add(sendingUnit);
		chosenRatings.Add(new Vector2(chosenRatings.Count,ratings));
	}
	
	public void setNarrativePanelOpen(bool newValue){
		narrativePanelOpen = newValue;
	}
	
	public void toggleTurn(){
			turnEnded = true;
	}
	public bool returnTurn(){
		return turnEnded;
	}
}
