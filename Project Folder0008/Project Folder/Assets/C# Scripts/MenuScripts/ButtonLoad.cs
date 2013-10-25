using UnityEngine;
using System.Collections;

public class ButtonLoad : MonoBehaviour {
	
	UILabel editLabel;
	
	public GameObject backButton;
	public GameObject loadText;
	
	void Start()
	{
		//assign
		editLabel = GameObject.Find("NTKFC").GetComponent<UILabel>();
	}
	
	void OnClick() {
		
		//disable menu buttons
		this.gameObject.SetActive(false);
		GameObject.Find("StartButton").SetActive(false);
		GameObject.Find("ExitButton").SetActive(false);
		GameObject.Find("OptionsButton").SetActive(false);
		
		//change title
		editLabel.text = "Load Menu";
		
		//enable text and button
		backButton.SetActive(true);
		loadText.SetActive(true);
	}
	
}
