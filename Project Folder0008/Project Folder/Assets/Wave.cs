using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {
	
	float number = 0;
	bool direction = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(direction==false){
			number+=Time.deltaTime;
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+0.02f,this.transform.position.z);
			if(number>=2){
				direction=true;
			}
		} else if(direction==true){
			number-=Time.deltaTime;
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-0.02f,this.transform.position.z);
			if(number<=0){
				direction=false;
			}
		}
	}
}
