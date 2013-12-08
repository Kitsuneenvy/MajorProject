using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class DialogueReader : MonoBehaviour {
	List<string> DialogueLines = new List<string>();
	public int section = 0;
	int sectionLine = 0;
	public int dialogueLine = 0;
	public LayerMask everythingMask;
	public LayerMask cinematicMask;
	string charName = "";
	string dialogue = "";
	float delay = 0;
	Quaternion initialRotation = Quaternion.identity;
	Vector3 initialPosition = Vector3.zero;
	UILabel narrativeDialogue;
	UILabel buttonText;
	GameObject narrativeAnchor;
	MissionReader mReaderObject;
	public GameObject endButton;
	gameManage gameManageObject;
	StoreData storeDataObject;
	// Use this for initialization
	void Start () {
		buttonText = GameObject.Find("DialogueLabel").GetComponent<UILabel>();
		narrativeAnchor = GameObject.FindGameObjectWithTag("NarrativeAnchor");
		narrativeDialogue = GameObject.FindGameObjectWithTag("NarrativeDialogue").GetComponent<UILabel>();
		mReaderObject = GameObject.Find("A*").GetComponent<MissionReader>();
		gameManageObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>();
		storeDataObject = GameObject.Find("SaveData").GetComponent<StoreData>();
		readDialogueFile();
		gameManageObject.narrativePanelOpen = true;
		if(Application.loadedLevelName == "Tutorial")
		{
			readSection(1);
		}
		if(Application.loadedLevelName == "Main")
		{
			readSection(0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.O)){
			readSection(1);
		}
	}
	
	void readDialogueFile(){
		//If tutorial
		if(Application.loadedLevelName == "Tutorial"){ 
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/TutorialDialogue.txt")){
				DialogueLines.Add(Line);
			}
		}
		if(Application.loadedLevelName == "Main"){
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/Mission1Dialogue.txt")){
				DialogueLines.Add(Line);
			}
		}
	}
	public void readSection(int sectionNumber){
		for(int i = 0; i<DialogueLines.Count; i++){
			//Debug.Log(DialogueLines[i]);
			if(DialogueLines[i].StartsWith("+"+sectionNumber.ToString())){
				section++;
				dialogueLine = 0;
				sectionLine = i;
			}
			if(section == sectionNumber){
				if(DialogueLines[i].StartsWith("<")){
					dialogueLine=i;
					readLine(dialogueLine);
					break;
				}
				if(DialogueLines[i].StartsWith("-")){
					dialogueLine = i;
					readLine(dialogueLine);
					break;
				}
			}
			if(DialogueLines[i].StartsWith("$")){
				Debug.Log("End!");
				break;
			}
		}
	}
	public void readLine(int lineNumber){
		if(!DialogueLines[lineNumber].StartsWith("-")){
			dialogueLine = lineNumber;
			charName = DialogueLines[lineNumber].Substring(DialogueLines[lineNumber].IndexOf("<".ToCharArray()[0])+1,DialogueLines[lineNumber].IndexOf(">".ToCharArray()[0])-1);
			dialogue = DialogueLines[lineNumber].Substring(DialogueLines[lineNumber].IndexOf(":".ToCharArray()[0])+2, DialogueLines[lineNumber].IndexOf(";".ToCharArray()[0])-(DialogueLines[lineNumber].IndexOf(":".ToCharArray()[0])+2));
		} else {
			GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().setNarrativePanelOpen(false);
			ScriptedEvents(section);
			readSection(section+1);
			//If you reach the end. Might handle it another way.
		}
		if(charName == "Chef"){
			
			GameObject.Find("CharacterPortrait").GetComponent<UITexture>().material	 = Resources.Load("ChefPortrait") as Material;
			//Do chef portrait
			if(storeDataObject.returnCommName() != "" && storeDataObject.returnCommName() != null)
			{
				charName = storeDataObject.returnCommName();
			}
			else
			{
				using(StreamReader sr = new StreamReader(Application.persistentDataPath+"/AutoSaves/"+storeDataObject.returnAutoSaveName()+".sav"))
				{
					charName = sr.ReadLine();
				}
			}
			GameObject.Find("CharacterName").GetComponent<UILabel>().text = charName;
		} else if(charName=="Pierre"){
			GameObject.Find("CharacterName").GetComponent<UILabel>().text = charName;
			GameObject.Find("CharacterPortrait").GetComponent<UITexture>().material = Resources.Load("FrierPortrait") as Material;
		} else if(charName=="Ladlewight"){
			GameObject.Find("CharacterName").GetComponent<UILabel>().text = charName;
			GameObject.Find("CharacterPortrait").GetComponent<UITexture>().material = Resources.Load("LadlePortrait") as Material;
		} else if(charName=="Bowlder"){
			GameObject.Find("CharacterName").GetComponent<UILabel>().text = charName;
			GameObject.Find("CharacterPortrait").GetComponent<UITexture>().material = Resources.Load("BowlderPortrait") as Material;
		} else {
			GameObject.Find("CharacterName").GetComponent<UILabel>().text = charName;
			GameObject.Find("CharacterPortrait").GetComponent<UITexture>().material = null;
		}
		if(DialogueLines[lineNumber+1].StartsWith("-")){
			buttonText.text = "End";
		} else {
			buttonText.text = "Next";
		}
		narrativeDialogue.text = dialogue;
		//etc
	}
	
	public void ScriptedEvents(int EventToPlay){
		//WARNING, ENTERING AN INVALID EVENT NUMBER WILL BREAAAAK IT ALLL.
		if(Application.loadedLevelName== "Tutorial"){
			switch(EventToPlay){
			case 1:
				mReaderObject.objective = "Move next to \n enemy";
				break;
			case 2:
				mReaderObject.checkmark.alpha = 0;
				mReaderObject.objective = "End your turn";
				break;
			case 3:
				mReaderObject.checkmark.alpha = 0;
				mReaderObject.objective = "Defeat the robot";
				break;
			case 4:
				Application.LoadLevel("Main");
				break;
			default:
				break;
			}
		}
		if(Application.loadedLevelName == "Main"){
			if(GameObject.Find("A*").GetComponent<MissionReader>().currentMission == 1||EventToPlay == 0){
				GameObject tempObject1 = null;
				GameObject tempObject2 = null;
				GameObject tempObject3 = null;
				GameObject pivotFocus = null;
				foreach(GameObject cineObject in GameObject.FindGameObjectsWithTag("Player")){
						if(cineObject.name.Contains("Chef")){
							tempObject1 = cineObject;
						}
						if(cineObject.name.Contains("Frier")){
							tempObject2 = cineObject;
						}
						if(cineObject.name.Contains("Bowlder")){
							tempObject3 = cineObject;
						}
						if(cineObject.name.Contains("Mission1")){
							pivotFocus = cineObject;
						}
					}
				switch(EventToPlay){
				case 0:
					Debug.Log("noMove is true");
					initialPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
					initialRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
					
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().noMove = true;
					GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(pivotFocus.transform.position.x+4,pivotFocus.transform.position.y+4,pivotFocus.transform.position.z+4);
					GameObject.FindGameObjectWithTag("MainCamera").transform.LookAt(pivotFocus.transform);
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().cullingMask = cinematicMask;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 1:
					Debug.Log("noMove is false");
					DestroyImmediate(tempObject1);
					DestroyImmediate(tempObject2);
					DestroyImmediate(tempObject3);
					DestroyImmediate(pivotFocus);
					GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = initialRotation;
					GameObject.FindGameObjectWithTag("MainCamera").transform.position = initialPosition;
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().cullingMask = everythingMask;
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().noMove = false;
					break;
				case 2:
					break;
				case 3:
					break;
				case 4:
					break;
				default:
					break;
				}
			}
		}
	}
	
	//completing an objective
	public void TaskCompletion(GameObject character)
	{
		if(Application.loadedLevelName == "Tutorial")
		{
			if(mReaderObject.objective.Contains("Move"))
			{
				if(mReaderObject.optionalTiles[0].GetComponent<Grid>().heldUnit == character || mReaderObject.optionalTiles[1].GetComponent<Grid>().heldUnit == character || mReaderObject.optionalTiles[2].GetComponent<Grid>().heldUnit == character)
				{
					mReaderObject.checkmark.alpha = 255;
					gameManageObject.commandPoints = 0;
					gameManageObject.narrativePanelOpen = true;
				}
				
			}
			else if(mReaderObject.objective.Contains("End"))
			{
				
				if(endButton.GetComponent<EndTurn>().pressed == true)
				{
					mReaderObject.checkmark.alpha = 255;
				}
			}
			else
			{
				if(mReaderObject.enemyUnits.Count == 0)
				{
					mReaderObject.checkmark.alpha = 255;
					gameManageObject.narrativePanelOpen = true;
				}
				
			}
		}
		else 
		{
			if(mReaderObject.objective.Contains("Kill all") || mReaderObject.objective.Contains("Defend"))
			{
				if(mReaderObject.enemyUnits.Count == 0)
				{
					mReaderObject.checkmark.alpha = 255;
					//gameManageObject.narrativePanelOpen = true;
				}
				
			}
			else if(mReaderObject.objective.Contains("tile"))
			{
				if(((mReaderObject.optionalTiles[0].GetComponent<Grid>().heldUnit!=null)&&(mReaderObject.optionalTiles[0].GetComponent<Grid>().heldUnit == character)) || ((mReaderObject.optionalTiles[1].GetComponent<Grid>().heldUnit!=null)&&(mReaderObject.optionalTiles[1].GetComponent<Grid>().heldUnit == character)) || ((mReaderObject.optionalTiles[2].GetComponent<Grid>().heldUnit!=null)&&(mReaderObject.optionalTiles[2].GetComponent<Grid>().heldUnit == character)))
				{
					mReaderObject.checkmark.alpha = 255;
					//gameManageObject.narrativePanelOpen = true;
				}
			}
			else if(mReaderObject.objective.Contains("Rout"))
			{
				if(mReaderObject.enemyUnits.Count < 3)
				{
					mReaderObject.checkmark.alpha = 255;
					//gameManageObject.narrativePanelOpen = true;
				}
			}
			else if(mReaderObject.objective.Contains("escaping"))
			{
				//need to change this
				if(mReaderObject.enemyUnits.Count == 0)
				{
					mReaderObject.checkmark.alpha = 255;
					//gameManageObject.narrativePanelOpen = true;
				}
			}
		}
	}
}
