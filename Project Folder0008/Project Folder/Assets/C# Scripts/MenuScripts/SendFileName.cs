using UnityEngine;
using System.Collections;

public class SendFileName : MonoBehaviour {
	
	public GameObject loadButton2;
	public GameObject deleteButton;
	// Use this for initialization
	void Start () {
		loadButton2 = GameObject.Find("LoadButton2");
		deleteButton = GameObject.Find("DeleteButton");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		deleteButton.GetComponent<ButtonDelete>().holdFile(this.GetComponentInChildren<UILabel>().text + ".sav", this.gameObject);
		loadButton2.GetComponent<ButtonLoad2>().holdFile(this.GetComponentInChildren<UILabel>().text);
	}
}
