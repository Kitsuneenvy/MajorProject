﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class DialogueReader : MonoBehaviour {
	List<string> DialogueLines = new List<string>();
	public int section = 0;
	int sectionLine = 0;
	public int dialogueLine = 0;
	string charName = "";
	string dialogue = "";
	
	UILabel narrativeDialogue;
	UILabel buttonText;
	GameObject narrativeAnchor;
	

	// Use this for initialization
	void Start () {
		buttonText = GameObject.Find("DialogueLabel").GetComponent<UILabel>();
		narrativeAnchor = GameObject.FindGameObjectWithTag("NarrativeAnchor");
		narrativeDialogue = GameObject.FindGameObjectWithTag("NarrativeDialogue").GetComponent<UILabel>();
		readDialogueFile();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.O)){
			readSection(1);
		}
	}
	
	void readDialogueFile(){
		//If tutorial
		if(1==1){ 
			foreach(string Line in File.ReadAllLines("Assets/MissionFiles/TutorialDialogue.txt")){
				DialogueLines.Add(Line);
			}
		}
	}
	public void readSection(int sectionNumber){
		Debug.Log(sectionNumber.ToString());
		for(int i = 0; i<DialogueLines.Count; i++){
			Debug.Log(DialogueLines[i]);
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
			//Do chef portrait
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
		switch(EventToPlay){
		case 1:
			Debug.Log("HALLAEO");
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
