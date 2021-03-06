﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;

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
	public bool initText = false;
	public bool cinematicComplete = false;
	GameObject mainCameraObj;
	// Use this for initialization
	void Start () {
		mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
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
		
	}
	
	// Update is called once per frame
	void Update () {
		if(mReaderObject.returnLayoutCompleted() == true&&initText == false){
			if(Application.loadedLevelName == "Main")
			{
				readSection(0);
			}
			initText = true;
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
	void readDialogueFile(int missionCount){
		DialogueLines.Clear();
		section = 0;
		dialogueLine = 0;
		sectionLine = 0;
		switch(missionCount){
		case(1):
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/Mission1Dialogue.txt")){
				DialogueLines.Add(Line);
			}
			break;
		case(2):
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/Mission2Dialogue.txt")){
				DialogueLines.Add(Line);
			}
			break;
		case(3):
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/Mission3Dialogue.txt")){
				DialogueLines.Add(Line);
			}
			break;
		case(4):
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/Mission4Dialogue.txt")){
				DialogueLines.Add(Line);
			}
			break;
		case(5):
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/Mission5Dialogue.txt")){
				DialogueLines.Add(Line);
			}
			break;
		default:
			break;
		}
	}
	public void readSection(int sectionNumber){
		for(int i = 0; i<DialogueLines.Count; i++){
			if(DialogueLines[i].StartsWith("+"+sectionNumber.ToString())){
//				Debug.Log(DialogueLines[i]);
				//Debug.Log(section.ToString());
				section++;
				dialogueLine = 0;
				sectionLine = i;
			}
			if(section == sectionNumber){
				//Debug.Log(section.ToString());
				if(DialogueLines[i].StartsWith("<")){
					dialogueLine=i;
					readLine(dialogueLine);
					return;
				}
				if(DialogueLines[i].StartsWith("-")){
					dialogueLine = i;
					readLine(dialogueLine);
					return;
				}
			}
			if(DialogueLines[i].StartsWith("$")){
				return;
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
			string numberLine = Regex.Replace(DialogueLines[lineNumber],"[^0-9]","");
			ScriptedEvents(int.Parse(numberLine));
			if(lineNumber+1<=DialogueLines.Count){
				if(!DialogueLines[lineNumber+1].StartsWith("$")){
					readSection(section+1);
				}
			}
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
		if(mReaderObject.recheck==false){
			if(lineNumber+1<=DialogueLines.Count){
				if(DialogueLines[lineNumber+1].StartsWith("-")){
					buttonText.text = "End";
				} else {
					buttonText.text = "Next";
				}
			}
		}
		narrativeDialogue.text = dialogue;
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
			if(mReaderObject.currentMission == 1){
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
					cinematicComplete = false;
					initialPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
					initialRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
					mainCameraObj.GetComponent<CameraMovement>().noMove = true;
					mainCameraObj.transform.position = new Vector3(pivotFocus.transform.position.x+4,pivotFocus.transform.position.y+4,pivotFocus.transform.position.z+4);
					mainCameraObj.transform.LookAt(pivotFocus.transform);
					mainCameraObj.GetComponent<Camera>().cullingMask = cinematicMask;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 1:
					cinematicComplete = true;
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().moveCamera = true;
					DestroyImmediate(tempObject1);
					DestroyImmediate(tempObject2);
					DestroyImmediate(tempObject3);
					DestroyImmediate(pivotFocus);
					mainCameraObj.transform.rotation = initialRotation;
					mainCameraObj.transform.position = initialPosition;
					mainCameraObj.GetComponent<Camera>().cullingMask = everythingMask;
					mainCameraObj.GetComponent<CameraMovement>().noMove = false;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 2:
					break;
				case 3:
					mReaderObject.currentMission = 2;
					mReaderObject.recheck = false;
					mReaderObject.newMission = true;
					mReaderObject.mission2= true;
					mReaderObject.layoutCompleted = false;
					mReaderObject.checkmark.alpha = 0;
					readDialogueFile(2);
					readSection(0);
					gameManageObject.narrativePanelOpen = true;
					break;
				case 4:
					break;
				default:
					break;
				}
			} else if (mReaderObject.currentMission == 2){
				GameObject tempObject1 = null;
				GameObject tempObject2 = null;
				GameObject tempObject3 = null;
				GameObject pivotFocus = null;
				foreach(GameObject cineObject in GameObject.FindGameObjectsWithTag("Player")){
						if(cineObject.name.Contains("Chef2")){
							tempObject1 = cineObject;
						}
						if(cineObject.name.Contains("Frier2")){
							tempObject2 = cineObject;
						}
						if(cineObject.name.Contains("Ladlewight2")){
							tempObject3 = cineObject;
						}
						if(cineObject.name.Contains("Mission2")){
							pivotFocus = cineObject;
						}
					}
				switch(EventToPlay){
				case 0:
					cinematicComplete = false;
					initialPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
					initialRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
					mainCameraObj.GetComponent<CameraMovement>().noMove = true;
					mainCameraObj.transform.position = new Vector3(pivotFocus.transform.position.x+4,pivotFocus.transform.position.y+4,pivotFocus.transform.position.z+4);
					mainCameraObj.transform.LookAt(pivotFocus.transform);
					mainCameraObj.GetComponent<Camera>().cullingMask = cinematicMask;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 1:
					cinematicComplete = true;
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().moveCamera = true;
					DestroyImmediate(tempObject1);
					DestroyImmediate(tempObject2);
					DestroyImmediate(tempObject3);
					DestroyImmediate(pivotFocus);
					mainCameraObj.transform.rotation = initialRotation;
					mainCameraObj.transform.position = initialPosition;
					mainCameraObj.GetComponent<Camera>().cullingMask = everythingMask;
					mainCameraObj.GetComponent<CameraMovement>().noMove = false;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 2:
					break;
				case 3:
					mReaderObject.currentMission = 3;
					mReaderObject.recheck = false;
					mReaderObject.newMission = true;
					mReaderObject.mission3 = true;
					mReaderObject.layoutCompleted = false;
					mReaderObject.checkmark.alpha = 0;
					readDialogueFile(3);
					readSection(0);
					gameManageObject.narrativePanelOpen = true;
					break;
				case 4:
					break;
				default:
					break;
				}
			} else if (mReaderObject.currentMission == 3){
				GameObject tempObject1 = null;
				GameObject tempObject2 = null;
				GameObject tempObject3 = null;
				GameObject pivotFocus = null;
				foreach(GameObject cineObject in GameObject.FindGameObjectsWithTag("Player")){
						if(cineObject.name.Contains("Chef3")){
							tempObject1 = cineObject;
						}
						if(cineObject.name.Contains("Frier3")){
							tempObject2 = cineObject;
						}
						if(cineObject.name.Contains("Bowlder3")){
							tempObject3 = cineObject;
						}
						if(cineObject.name.Contains("Mission3")){
							pivotFocus = cineObject;
						}
					}
				switch(EventToPlay){
				case 0:
					cinematicComplete = false;
					initialPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
					initialRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
					mainCameraObj.GetComponent<CameraMovement>().noMove = true;
					mainCameraObj.transform.position = new Vector3(pivotFocus.transform.position.x+4,pivotFocus.transform.position.y+4,pivotFocus.transform.position.z+4);
					mainCameraObj.transform.LookAt(pivotFocus.transform);
					mainCameraObj.GetComponent<Camera>().cullingMask = cinematicMask;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 1:
					cinematicComplete = true;
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().moveCamera = true;
					DestroyImmediate(tempObject1);
					DestroyImmediate(tempObject2);
					DestroyImmediate(tempObject3);
					DestroyImmediate(pivotFocus);
					mainCameraObj.transform.rotation = initialRotation;
					mainCameraObj.transform.position = initialPosition;
					mainCameraObj.GetComponent<Camera>().cullingMask = everythingMask;
					mainCameraObj.GetComponent<CameraMovement>().noMove = false;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 2:
					break;
				case 3:
					mReaderObject.currentMission = 4;
					mReaderObject.recheck = false;
					mReaderObject.newMission = true;
					mReaderObject.mission4= true;
					mReaderObject.layoutCompleted = false;
					mReaderObject.checkmark.alpha = 0;
					readDialogueFile(4);
					readSection(0);
					gameManageObject.narrativePanelOpen = true;
					break;
				case 4:
					break;
				default:
					break;
				}
			} else if (mReaderObject.currentMission == 4){
				GameObject tempObject1 = null;
				GameObject tempObject2 = null;
				GameObject tempObject3 = null;
				GameObject pivotFocus = null;
				foreach(GameObject cineObject in GameObject.FindGameObjectsWithTag("Player")){
						if(cineObject.name.Contains("Chef4")){
							tempObject1 = cineObject;
						}
						if(cineObject.name.Contains("Frier4")){
							tempObject2 = cineObject;
						}
						if(cineObject.name.Contains("Bowlder4")){
							tempObject3 = cineObject;
						}
						if(cineObject.name.Contains("Mission4")){
							pivotFocus = cineObject;
						}
					}
				switch(EventToPlay){
				case 0:
					cinematicComplete = false;
					initialPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
					initialRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
					mainCameraObj.GetComponent<CameraMovement>().noMove = true;
					mainCameraObj.transform.position = new Vector3(pivotFocus.transform.position.x+4,pivotFocus.transform.position.y+4,pivotFocus.transform.position.z+4);
					mainCameraObj.transform.LookAt(pivotFocus.transform);
					mainCameraObj.GetComponent<Camera>().cullingMask = cinematicMask;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 1:
					cinematicComplete = true;
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().moveCamera = true;
					DestroyImmediate(tempObject1);
					DestroyImmediate(tempObject2);
					DestroyImmediate(tempObject3);
					DestroyImmediate(pivotFocus);
					mainCameraObj.transform.rotation = initialRotation;
					mainCameraObj.transform.position = initialPosition;
					mainCameraObj.GetComponent<Camera>().cullingMask = everythingMask;
					mainCameraObj.GetComponent<CameraMovement>().noMove = false;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 2:
					break;
				case 3:
					mReaderObject.currentMission = 5;
					mReaderObject.recheck = false;
					mReaderObject.newMission = true;
					mReaderObject.mission5= true;
					mReaderObject.layoutCompleted = false;
					readDialogueFile(5);
					gameManageObject.narrativePanelOpen = true;
					readSection(0);
					mReaderObject.checkmark.alpha = 0;
					break;
				case 4:
					break;
				default:
					break;
				}
			} else if (mReaderObject.currentMission == 5){
				GameObject tempObject1 = null;
				GameObject tempObject2 = null;
				GameObject tempObject3 = null;
				GameObject pivotFocus = null;
				foreach(GameObject cineObject in GameObject.FindGameObjectsWithTag("Player")){
						if(cineObject.name.Contains("Chef5")){
							tempObject1 = cineObject;
						}
						if(cineObject.name.Contains("Frier5")){
							tempObject2 = cineObject;
						}
						if(cineObject.name.Contains("Bowlder5")){
							tempObject3 = cineObject;
						}
						if(cineObject.name.Contains("Mission5")){
							pivotFocus = cineObject;
						}
					}
				switch(EventToPlay){
				case 0:
					cinematicComplete = false;
					initialPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
					initialRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
					mainCameraObj.GetComponent<CameraMovement>().noMove = true;
					mainCameraObj.transform.position = new Vector3(pivotFocus.transform.position.x+4,pivotFocus.transform.position.y+4,pivotFocus.transform.position.z+4);
					mainCameraObj.transform.LookAt(pivotFocus.transform);
					mainCameraObj.GetComponent<Camera>().cullingMask = cinematicMask;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 1:
					cinematicComplete = true;
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().moveCamera = true;
					DestroyImmediate(tempObject1);
					DestroyImmediate(tempObject2);
					DestroyImmediate(tempObject3);
					DestroyImmediate(pivotFocus);
					mainCameraObj.transform.rotation = initialRotation;
					mainCameraObj.transform.position = initialPosition;
					mainCameraObj.GetComponent<Camera>().cullingMask = everythingMask;
					mainCameraObj.GetComponent<CameraMovement>().noMove = false;
					gameManageObject.narrativePanelOpen = true;
					break;
				case 2:
					break;
				case 3:
					//Do endgame stuff, maybe a nice fade then roll the credits.
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
			if(mReaderObject.currentMission == 1 || mReaderObject.objective.Contains("Defend"))
			{
				if(mReaderObject.enemyUnits.Count == 0)
				{
					mReaderObject.checkmark.alpha = 255;
					gameManageObject.narrativePanelOpen = true;
				}
				
			}
			else if(mReaderObject.objective.Contains("tile"))
			{
				if(((mReaderObject.optionalTiles[0].GetComponent<Grid>().heldUnit!=null)&&(mReaderObject.optionalTiles[0].GetComponent<Grid>().heldUnit == character)) || ((mReaderObject.optionalTiles[1].GetComponent<Grid>().heldUnit!=null)&&(mReaderObject.optionalTiles[1].GetComponent<Grid>().heldUnit == character)) || ((mReaderObject.optionalTiles[2].GetComponent<Grid>().heldUnit!=null)&&(mReaderObject.optionalTiles[2].GetComponent<Grid>().heldUnit == character)))
				{
					mReaderObject.checkmark.alpha = 255;
					gameManageObject.narrativePanelOpen = true;
				}
			}
			else if(mReaderObject.objective.Contains("Rout"))
			{
				if(mReaderObject.enemyUnits.Count < 3)
				{
					mReaderObject.checkmark.alpha = 255;
					gameManageObject.narrativePanelOpen = true;
				}
			}
			else if(mReaderObject.objective.Contains("escaping"))
			{
				//need to change this
				if(mReaderObject.enemyUnits.Count == 0)
				{
					mReaderObject.checkmark.alpha = 255;
					gameManageObject.narrativePanelOpen = true;
				}
			}
		}
	}
}
