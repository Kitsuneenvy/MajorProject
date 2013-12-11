using UnityEngine;
using System.Collections;

public class Offset : MonoBehaviour {
	float storeX = 0;
	float storeY = 0;
	
	gameManage gameManageObject;
	
	Material cultMat;
	Material knightMat;
	
	// Use this for initialization
	void Start () {
	
		gameManageObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>();
		knightMat = Resources.Load("KnightCommPtMat") as Material;
		cultMat =  Resources.Load("CultCommPtMat") as Material;
	}
	
	//We know this is a bad way to do it but we don't want to accidentally break something by changing it.
	// Update is called once per frame
	void Update () {
		if(name=="ObjectivesPanel"){
			this.transform.localScale = new Vector3(Screen.width/4,Screen.height/5,1);
			this.transform.localPosition=new Vector3(-transform.localScale.x/2,-transform.localScale.y/2,0);
		}
		else if(name=="CharacterDetailsPanel"){
			this.transform.localScale = new Vector3(Screen.width/3,Screen.height/4,1);
			this.transform.localPosition=new Vector3(transform.localScale.x/2,-transform.localScale.y/2,0);
		}
		else if(name=="CommandPointsPanel"){
			this.transform.localScale = new Vector3(Screen.width/3,Screen.height/6,1);
			this.transform.localPosition=new Vector3(-transform.localScale.x/2,transform.localScale.y/2,0);
		}
		else if(name=="StoryLogPanel"){
			this.transform.localScale = new Vector3(Screen.width/3,Screen.height/4,1);
			this.transform.localPosition=new Vector3(transform.localScale.x/2,transform.localScale.y/2,0);
		}
		else if(name=="NarrativePanel"){
			this.transform.localScale = new Vector3(Screen.width/2,Screen.height/2,1);
			this.transform.localPosition = new Vector3(transform.localScale.x/2,transform.localScale.y/2,0);
		}
		else if(name=="Character Attributes"){
			this.transform.localPosition = new Vector3(Screen.width/50,-Screen.height/5,0);
		}
		if(name=="Character HP"){
			this.transform.localPosition = new Vector3(Screen.width/50, -Screen.height/8,0);
		}
		else if(name == "Character Name"){
			this.transform.localPosition = new Vector3(Screen.width/50, -Screen.height/20,0);
		}
		else if(name == "Command Points"){
			this.transform.localPosition = new Vector3(-Screen.width/2, Screen.height/14,0);
		}
		else if(name == "Dialogue"){
			this.transform.localPosition = new Vector3(Screen.width/50f, Screen.height/8,0);
		}
		else if(name == "CharacterDialogue"){
			this.transform.localPosition = new Vector3(Screen.width/20f, 0,0);
		}
		else if(name == "CharacterPortrait"){
			//this.transform.localPosition = new Vector3(-Screen.width/5f, Screen.height/7,0);
			this.GetComponent<UIWidget>().depth= 5;
			this.transform.localPosition = new Vector3(-(Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).x/4),0,0);
			this.transform.localScale = new Vector3((Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).x/2),Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).y,0);
		}
		else if(name == "NextButton"){
			this.transform.localScale = new Vector3(GameObject.Find("NarrativePanel").transform.localScale.y/300,GameObject.Find("NarrativePanel").transform.localScale.y/300,1);
			this.transform.localPosition = new Vector3(Screen.width/6f, -Screen.height/5,0);
		}
		else if(name == "Objectives")
		{
			this.transform.localPosition = new Vector3(-Screen.width/6, -Screen.height/18,0);
		}
		else if(name == "Checkbox")
		{
			this.transform.localPosition = new Vector3(-Screen.width/5, -Screen.height/8,0);
			this.transform.FindChild("ObjectivesLabel").GetComponent<UILabel>().text = GameObject.Find("A*").GetComponent<MissionReader>().objective;
		}
		else if(name == "EndTurn"){
			//this.transform.localScale = new Vector3(Screen.width/140,Screen.height/100,1);
//			this.transform.localScale = new Vector3(GameObject.Find("CommandPointsPanel").transform.localScale.y/150,this.transform.localScale.y,0);
			this.transform.localPosition = new Vector3(-Screen.width/12f, Screen.height/12f,0);	
		}
		else if(name == "CharacterName"){
			this.GetComponent<UIWidget>().depth = 10;
			this.transform.localPosition = new Vector3(-(Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).x/4),-(Vector3.Scale(GameObject.Find("NarrativePanel").GetComponent<UIWidget>().relativeSize,GameObject.Find("NarrativePanel").transform.localScale).y*0.45f),-1);
		}
		
		else if(name == "CommPt1")
		{
			this.GetComponent<UIWidget>().depth = 7;
			this.transform.localScale = new Vector3(Screen.width/11,Screen.height/8,1);
			this.transform.localPosition = new Vector3(-Screen.width/2.4f, Screen.height/12,5);
			
			if(gameManageObject.playerTurn == true && (this.GetComponent<UITexture>().material == null||this.GetComponent<UITexture>().material == cultMat))
			{
				if(gameManageObject.commandPoints<=3&&gameManageObject.commandPoints>0){
					this.GetComponent<UITexture>().material = knightMat;
				}
			}
			else if (gameManageObject.playerTurn == false && (this.GetComponent<UITexture>().material == null||this.GetComponent<UITexture>().material == knightMat))
			{
				if(gameManageObject.commandPoints<=3&&gameManageObject.commandPoints>0){
					this.GetComponent<UITexture>().material = cultMat;
				}
			}
			if (gameManageObject.commandPoints <1){
				this.GetComponent<UITexture>().material = null;
			}
		}
		
		else if(name == "CommPt2")
		{
			this.GetComponent<UIWidget>().depth = 7;
			this.transform.localScale = new Vector3(Screen.width/11,Screen.height/8,1);
			this.transform.localPosition = new Vector3((-Screen.width/2.4f + this.transform.localScale.x), Screen.height/12,5);
			if(gameManageObject.playerTurn == true && (this.GetComponent<UITexture>().material == null||this.GetComponent<UITexture>().material == cultMat))
			{
				if(gameManageObject.commandPoints<=3&&gameManageObject.commandPoints>1){
					this.GetComponent<UITexture>().material = knightMat;
				} 
			}
			else if (gameManageObject.playerTurn == false && (this.GetComponent<UITexture>().material == null||this.GetComponent<UITexture>().material == knightMat))
			{
				if(gameManageObject.commandPoints<=3&&gameManageObject.commandPoints>1){
					this.GetComponent<UITexture>().material = cultMat;
				} 
			}
			if (gameManageObject.commandPoints <2){
				this.GetComponent<UITexture>().material = null;
			}
		}
		
		else if(name == "CommPt3")
		{
			this.GetComponent<UIWidget>().depth = 7;
			this.transform.localScale = new Vector3(Screen.width/11,Screen.height/8,1);
			this.transform.localPosition = new Vector3((-Screen.width/2.4f + this.transform.localScale.x*2), Screen.height/12,5);
			if(gameManageObject.playerTurn == true && (this.GetComponent<UITexture>().material == null||this.GetComponent<UITexture>().material == cultMat))
			{
				if(gameManageObject.commandPoints==3){
					this.GetComponent<UITexture>().material = knightMat;
				}
			}
			else if (gameManageObject.playerTurn == false && (this.GetComponent<UITexture>().material == null||this.GetComponent<UITexture>().material == knightMat))
			{
				if(gameManageObject.commandPoints==3){
					this.GetComponent<UITexture>().material = cultMat;
				} 
			}
			if (gameManageObject.commandPoints <3){
				this.GetComponent<UITexture>().material = null;
			}
		}
	
	}
}
