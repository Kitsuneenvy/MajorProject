using UnityEngine;
using System.Collections;

public class HUDAdjustments : MonoBehaviour {
	public GameObject leftPanel;
	public GameObject topRightPanel;
	public GameObject bottomRightPanel;
	public GameObject commandPointsLabel;
	public GameObject objectivesLabel;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(leftPanel.transform.position.ToString());
		//Debug.Log((Screen.width/3).ToString());
		//leftPanel.transform.localScale = new Vector3(Screen.width/5,Screen.height/5,1);
		//topRightPanel.transform.localScale = new Vector3(Screen.width/4,Screen.height/4,1);
		//bottomRightPanel.transform.localScale = new Vector3(Screen.width/3,Screen.height/4,1);
		
		/*leftPanel.transform.position = new Vector3(0,0,1);
		topRightPanel.transform.position = new Vector3(82,0,1);
		bottomRightPanel.transform.position = new Vector3(Screen.width/5, Screen.height/5,1);*/
	}
}
