﻿using UnityEngine;
using System.Collections;

public class EndTurn : MonoBehaviour {
	
	public bool pressed = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
		if(GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().playerTurn == true){
			if(Application.loadedLevelName == "Tutorial")
			{
				pressed = true;
				GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueReader>().TaskCompletion(null);
			}
			GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().nextTurn();
		}
	}
}
