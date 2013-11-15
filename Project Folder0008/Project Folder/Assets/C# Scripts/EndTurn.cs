using UnityEngine;
using System.Collections;

public class EndTurn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
		if(GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().playerTurn == true){
			GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().nextTurn();
		}
	}
}
