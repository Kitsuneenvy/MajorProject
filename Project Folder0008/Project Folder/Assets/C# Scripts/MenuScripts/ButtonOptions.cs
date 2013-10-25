using UnityEngine;
using System.Collections;

public class ButtonOptions : MonoBehaviour {
	
	UILabel editLabel;
	
	
	public GameObject backButton;
	public GameObject checkBox;
	public GameObject slider;
	public GameObject musicVolume;
	
	void Start()
	{
		//assign
		editLabel = GameObject.Find("NTKFC").GetComponent<UILabel>();
	}

	// Use this for initialization
	void OnClick () {
		
		//disable menu buttons
		this.gameObject.SetActive(false);
		GameObject.Find("StartButton").SetActive(false);
		GameObject.Find("ExitButton").SetActive(false);
		GameObject.Find("LoadButton").SetActive(false);
		
		//change title
		editLabel.text = "Options";
		
		//enable options features
		backButton.SetActive(true);
		checkBox.SetActive(true);
		slider.SetActive(true);
		musicVolume.SetActive(true);
		
	}
	
}
