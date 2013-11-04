using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	DropDownMenu dropDownMenuObject;
	int xLeft = 0; //left 
	int xRight = 0;	//right
	int zFront = 0;	//forward
	int zBack = 0;	//back
	
	MissionReader missionReader;
	AstarPath astarPath;
	
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
		if(missionReader.returnLayoutCompleted() && moveCamera == true)
		{
			//set camera position to the centre of grid
			this.transform.position = new Vector3(astarPath.astarData.gridGraph.center.x,this.transform.position.y,astarPath.astarData.gridGraph.center.z);
			
			//set camera rotation to that of the grid
			this.transform.rotation = Quaternion.Euler(new Vector3(50, astarPath.astarData.gridGraph.rotation.y, astarPath.astarData.gridGraph.rotation.z));
			
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
				zFront = (int)astarPath.astarData.gridGraph.center.z + (((int)astarPath.astarData.gridGraph.depth * 5)/2) -5;
				zBack = (int)astarPath.astarData.gridGraph.center.z - (((int)astarPath.astarData.gridGraph.depth * 5)/2) - 5;
		//	}
			
			
			moveCamera = false;
		}
		
		//Move camera as long as no drop down menu is created
		if(!dropDownMenuObject.cameraLock){
			if(!missionReader.flipped){
				//moves camera left
				if(Input.mousePosition.x<=Screen.width/50 && this.transform.position.x > xLeft ){
		
					this.transform.Translate(Vector3.left);		
				}
					//moves camera right
				if(Input.mousePosition.x>=Screen.width-Screen.width/50 && this.transform.position.x < xRight){
					
					this.transform.Translate(Vector3.right);
				}
					//moves camera down
				if(Input.mousePosition.y<=Screen.height/50 && this.transform.position.z > zBack){
						
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,-1)));
				}
					//moves camera up
				if(Input.mousePosition.y>=Screen.height-Screen.height/50 && this.transform.position.z < zFront ){
						
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,1)));
				}
		
			}
			else{
				//moves camera left
				if(Input.mousePosition.x<=Screen.width/50 && this.transform.position.x < (xRight + 5 )){
		
					this.transform.Translate(Vector3.left);		
				}
					//moves camera right
				if(Input.mousePosition.x>=Screen.width-Screen.width/50 && this.transform.position.x > (xLeft - 2)){
					
					this.transform.Translate(Vector3.right);
				}
					//moves camera down
				if(Input.mousePosition.y<=Screen.height/50 && this.transform.position.z < (zFront + 10)){
						
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,1)));
		
				}
					//moves camera up
				if(Input.mousePosition.y>=Screen.height-Screen.height/50 && this.transform.position.z > zBack ){
							
					this.transform.Translate(this.transform.InverseTransformDirection(new Vector3(0,0,-1)));
				}
			}
			//zooms in camera
		if(Input.GetAxis("Mouse ScrollWheel")>0&&this.transform.position.y>75){
			this.transform.Translate(this.transform.InverseTransformDirection(Vector3.down));
		}
			//zooms out camera
		if(Input.GetAxis("Mouse ScrollWheel")<0&&this.transform.position.y<100){
			this.transform.Translate(this.transform.InverseTransformDirection(Vector3.up));
		}
		}
		//if a drop down menu is created
		else if(dropDownMenuObject.cameraLock)
		{
			//this.transform.position = new Vector3(dropDownMenuObject.popList.transform.position.x,this.transform.position.y,dropDownMenuObject.popList.transform.z);
		}
	
	}
}
