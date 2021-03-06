using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	
	public bool noMove = false;
	DropDownMenu dropDownMenuObject;
	int xLeft = 0; //left 
	int xRight = 0;	//right
	int zFront = 0;	//forward
	int zBack = 0;	//back
	int RotationY = 0;
	MissionReader missionReader;
	AstarPath astarPath;
	Vector3 previousPos = new Vector3(0,0,0);
	Vector3 changedPos = new Vector3(0,0,0);
	//tell camera to move
	public bool moveCamera = false;
	
	// Use this for initialization
	void Start () {
		dropDownMenuObject = GameObject.Find("Panel").GetComponent<DropDownMenu>();
		missionReader = GameObject.FindGameObjectWithTag("Grid").GetComponent<MissionReader>();
		astarPath = GameObject.FindGameObjectWithTag("Grid").GetComponent<AstarPath>();
		xLeft = -27;
		xRight = 27;
		zFront = -110;
		zBack = -140;
	}
	
	// Update is called once per frame
	void Update () {
		//if a new mission is created
		if(!noMove){
			if(missionReader.returnLayoutCompleted() && moveCamera == true)
			{
				if(missionReader.flipped == true)
				{
					RotationY = 180;
				}
				previousPos = this.transform.position;
				changedPos = new Vector3(astarPath.astarData.gridGraph.center.x,this.transform.position.y,astarPath.astarData.gridGraph.center.z);
				//set camera position to the centre of grid
				this.transform.position = new Vector3(astarPath.astarData.gridGraph.center.x,this.transform.position.y,astarPath.astarData.gridGraph.center.z);
				
				//set camera rotation to that of the grid
				this.transform.rotation = Quaternion.Euler(new Vector3(50, astarPath.astarData.gridGraph.rotation.y + RotationY, astarPath.astarData.gridGraph.rotation.z));
				
				//set xleft,xright,zback and zfront
				//if(missionReader.flipped)
			//	{
			//		xLeft = (int)astarPath.astarData.gridGraph.center.x + 27;
			//		xRight = (int)astarPath.astarData.gridGraph.center.x - 27;
			//		zFront = (int)astarPath.astarData.gridGraph.center.z - (((int)astarPath.astarData.gridGraph.depth * 5)/2) - 9;
			//		zBack = (int)astarPath.astarData.gridGraph.center.z + (((int)astarPath.astarData.gridGraph.depth * 5)/2) -3;
			//	}
			//	else
			//	{
					xLeft = (int)astarPath.astarData.gridGraph.center.x - 27;
					xRight = (int)astarPath.astarData.gridGraph.center.x + 27;
					zFront = (int)astarPath.astarData.gridGraph.center.z + (((int)astarPath.astarData.gridGraph.depth * 5)/2) - 10;
					zBack = (int)astarPath.astarData.gridGraph.center.z - (((int)astarPath.astarData.gridGraph.depth * 5)/2) - 10;
			//	}
				
				if(changedPos != previousPos)
				{
					moveCamera = false;
				}
			}
		}
		
		//Move camera as long as no drop down menu is created
		if(!dropDownMenuObject.cameraLock&&noMove==false){
			if(!missionReader.flipped){
				//moves camera left
				if(Input.mousePosition.x<=Screen.width/50 && this.transform.position.x > xLeft){
		
					this.transform.Translate(Vector3.left/2);		
				}
					//moves camera right
				if(Input.mousePosition.x>=Screen.width-Screen.width/50 && this.transform.position.x < xRight){
					
					this.transform.Translate(Vector3.right/2);
				}
					//moves camera down
				if(Input.mousePosition.y<=Screen.height/50 && this.transform.position.z > zBack){
						
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,-0.5f)));
				}
					//moves camera up
				if(Input.mousePosition.y>=Screen.height-Screen.height/50 && this.transform.position.z < zFront){
						
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,0.5f)));
				}
		
			}
			else{
				//moves camera left
				if(Input.mousePosition.x<=Screen.width/50 && this.transform.position.x < (xRight + 5 )){
		
					this.transform.Translate(Vector3.left/2);		
				}
					//moves camera right
				if(Input.mousePosition.x>=Screen.width-Screen.width/50 && this.transform.position.x > (xLeft - 2)){
					
					this.transform.Translate(Vector3.right/2);
				}
					//moves camera down
				if(Input.mousePosition.y<=Screen.height/50 && this.transform.position.z < (zFront + 10)){
						
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,0.5f)));
		
				}
					//moves camera up
				if(Input.mousePosition.y>=Screen.height-Screen.height/50 && this.transform.position.z > zBack){
							
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,-0.5f)));
				}
			}
			//zooms in camera
		if(Input.GetAxis("Mouse ScrollWheel")>0&&this.transform.position.y>75){
			this.transform.Translate(this.transform.InverseTransformDirection(Vector3.down*2));
		}
			//zooms out camera
		if(Input.GetAxis("Mouse ScrollWheel")<0&&this.transform.position.y<100){
			this.transform.Translate(this.transform.InverseTransformDirection(Vector3.up*2));
		}
		}
		//if a drop down menu is created
		else if(dropDownMenuObject.cameraLock)
		{
			//this.transform.position = new Vector3(dropDownMenuObject.popList.transform.position.x,this.transform.position.y,dropDownMenuObject.popList.transform.z);
		}
	
	}
}
