using UnityEngine;
using System.Collections;

public class damageText : MonoBehaviour {
	float delay = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+0.05f,this.transform.position.z);
		delay = delay-Time.deltaTime;
		if(delay<=0){
			DestroyImmediate(this.gameObject);
		}
	}
}
