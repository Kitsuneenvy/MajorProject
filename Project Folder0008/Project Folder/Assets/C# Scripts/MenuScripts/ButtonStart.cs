using UnityEngine;
using System.Collections;

public class ButtonStart : MonoBehaviour {
	
	public GameObject commName;
	public GameObject tutCheckbox;
	public GameObject backButton;
	public GameObject commTextField;
	public GameObject beginButton;
	
	// Use this for initialization
	void OnClick() {
	
		//disable menu buttons
		this.gameObject.SetActive(false);
		GameObject.Find("LoadButton").SetActive(false);
		GameObject.Find("ExitButton").SetActive(false);
		GameObject.Find("OptionsButton").SetActive(false);
		
		
		commName.SetActive(true);
		tutCheckbox.SetActive(true);
		backButton.SetActive(true);
		commTextField.SetActive(true);
		beginButton.SetActive(true);
	}
	

}
