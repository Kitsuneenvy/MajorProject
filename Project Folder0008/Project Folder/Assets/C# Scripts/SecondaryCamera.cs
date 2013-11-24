using UnityEngine;
using System.Collections;

public class SecondaryCamera : MonoBehaviour {
	Vector3 lookTarget = Vector3.zero;
	int posCount = 0;
	bool moveDirection = false;
	bool active = false;
	Rect cameraRect = new Rect(0,0,0,0);

	// Use this for initialization
	void Start () {
		cameraRect = this.GetComponent<Camera>().rect;
	}
	
	// Update is called once per frame
	void Update () {
		if(active==false){
			if(this.GetComponent<Camera>().rect.width<=0){
				this.GetComponent<Camera>().depth = -2;
			} else {
				cameraRect = new Rect(cameraRect.x,cameraRect.y,cameraRect.width-0.05f,cameraRect.height);
				this.GetComponent<Camera>().rect = cameraRect;
			}
		} else {
			this.GetComponent<Camera>().depth = 0;
			if(this.GetComponent<Camera>().rect.width>=0.30f){
				this.GetComponent<Camera>().depth = 0;
			} else {
				cameraRect = new Rect(cameraRect.x,cameraRect.y,cameraRect.width+0.05f,cameraRect.height);
				this.GetComponent<Camera>().rect = cameraRect;
			}
		}
		this.transform.LookAt(lookTarget);
		if(moveDirection == false){
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z+0.05f);
			posCount++;
			if(posCount>=300){
				moveDirection = true;
			}
		} else if (moveDirection == true){
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z-0.05f);
			posCount--;
			if(posCount<=0){
				moveDirection = false;
			}
		}
	}
	
	public void setFocus(GameObject target){
		lookTarget = new Vector3(target.transform.position.x,target.transform.position.y+5,target.transform.position.z);
		this.transform.position = new Vector3(lookTarget.x+15,lookTarget.y+15,lookTarget.z);
	}
	public void setFocus(GameObject target, GameObject target2){
		lookTarget = new Vector3((target.transform.position.x+target2.transform.position.x)/2,(target.transform.position.y+target2.transform.position.y)/2+5,(target.transform.position.z+target2.transform.position.z)/2);
		this.transform.position = ((target.transform.position+target2.transform.position)/2);
		this.transform.position = new Vector3(this.transform.position.x+15,this.transform.position.y+15,this.transform.position.z);
	}
	public void setActive(bool newValue){
		active = newValue;
	}
	public bool getActive(){
		return active;
	}
}
