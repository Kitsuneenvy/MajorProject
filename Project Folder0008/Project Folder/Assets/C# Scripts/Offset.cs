using UnityEngine;
using System.Collections;

public class Offset : MonoBehaviour {
	float storeX = 0;
	float storeY = 0;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(name=="ObjectivesPanel"){
			this.transform.localScale = new Vector3(Screen.width/4,Screen.height/5,1);
			this.transform.localPosition=new Vector3(-transform.localScale.x/2,-transform.localScale.y/2,0);
		}
		if(name=="CharacterDetailsPanel"){
			this.transform.localScale = new Vector3(Screen.width/3,Screen.height/4,1);
			this.transform.localPosition=new Vector3(transform.localScale.x/2,-transform.localScale.y/2,0);
		}
		if(name=="CommandPointsPanel"){
			this.transform.localScale = new Vector3(Screen.width/3,Screen.height/6,1);
			this.transform.localPosition=new Vector3(-transform.localScale.x/2,transform.localScale.y/2,0);
		}
		if(name=="StoryLogPanel"){
			this.transform.localScale = new Vector3(Screen.width/3,Screen.height/4,1);
			this.transform.localPosition=new Vector3(transform.localScale.x/2,transform.localScale.y/2,0);
		}
		if(name=="NarrativePanel"){
			this.transform.localScale = new Vector3(Screen.width/2,Screen.height/2,1);
			this.transform.localPosition = new Vector3(transform.localScale.x/2,transform.localScale.y/2,0);
		}
		if(name=="Character Attributes"){
			this.transform.localPosition = new Vector3(Screen.width/50,-Screen.height/5,0);
		}
		if(name=="Character HP"){
			this.transform.localPosition = new Vector3(Screen.width/50, -Screen.height/8,0);
		}
		if(name == "Character Name"){
			this.transform.localPosition = new Vector3(Screen.width/50, -Screen.height/20,0);
		}
		if(name == "Command Points"){
			this.transform.localPosition = new Vector3(-Screen.width/2.5f, Screen.height/14,0);
		}
		if(name == "Dialogue"){
			this.transform.localPosition = new Vector3(Screen.width/50f, Screen.height/8,0);
		}
		if(name == "CharacterDialogue"){
			this.transform.localPosition = new Vector3(Screen.width/20f, 0,0);
		}
		if(name == "CharacterPortrait"){
			//this.transform.localPosition = new Vector3(-Screen.width/5f, Screen.height/7,0);
			this.GetComponent<UIWidget>().depth= 5;
			this.transform.localPosition = new Vector3(-(Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).x/4),0,0);
			this.transform.localScale = new Vector3((Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).x/2),Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).y,0);
		}
		if(name == "NextButton"){
			this.transform.localScale = new Vector3(GameObject.Find("NarrativePanel").transform.localScale.y/300,GameObject.Find("NarrativePanel").transform.localScale.y/300,1);
			this.transform.localPosition = new Vector3(Screen.width/6f, -Screen.height/5,0);
		}
		if(name == "Objectives")
		{
			this.transform.localPosition = new Vector3(-Screen.width/6, -Screen.height/18,0);
		}
		if(name == "Checkbox")
		{
			this.transform.localPosition = new Vector3(-Screen.width/5, -Screen.height/8,0);
			this.transform.FindChild("ObjectivesLabel").GetComponent<UILabel>().text = GameObject.Find("A*").GetComponent<MissionReader>().objective;
		}
		if(name == "EndTurn"){
//			this.transform.localScale = new Vector3(GameObject.Find("CommandPointsPanel").transform.localScale.y/150,this.transform.localScale.y,0);
			this.transform.localPosition = new Vector3(-Screen.width/12f, Screen.height/12f,0);	
		}
		if(name == "CharacterName"){
			this.GetComponent<UIWidget>().depth = 10;
			this.transform.localPosition = new Vector3(-(Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).x/4),-(Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).y*0.45f),-1);
		}
	
	}
}
