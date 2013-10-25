using UnityEngine;
using System.Collections;

public class NextDialogue : MonoBehaviour {
	DialogueReader readerObject;

	// Use this for initialization
	void Start () {
		readerObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueReader>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
		readerObject.readLine(readerObject.dialogueLine+1);
	}
}
