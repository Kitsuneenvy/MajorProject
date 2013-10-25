using UnityEngine;
using System.Collections;

public class AdjustManualHeight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<UIRoot>().manualHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
