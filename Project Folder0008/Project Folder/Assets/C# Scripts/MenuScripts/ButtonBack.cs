using UnityEngine;
using System.Collections;

public class ButtonBack : MonoBehaviour {
	
	UILabel editLabel;
	
	//menu buttons
	public GameObject startButton;
	public GameObject loadButton;
	public GameObject optionsButton;
	public GameObject exitButton;
	//instructions menu
	public GameObject loadText;
	//options menu
	public GameObject slider;
	public GameObject checkBox;
	public GameObject musicVolume;
	//start game menu
	public GameObject commName;
	public GameObject tutCheckbox;
	public GameObject commTextField;
	public GameObject beginButton;
	
	void Start()
	{
		//assign
		editLabel = GameObject.Find("NTKFC").GetComponent<UILabel>();
	}

	// Use this for initialization
	void OnClick () {
		
		//disable back button and text
		this.gameObject.SetActive(false);
		if(loadText.activeSelf == true)
		{
			loadText.SetActive(false);
		}
		if(slider.activeSelf == true)
		{
			slider.SetActive(false);
			checkBox.SetActive(false);
			musicVolume.SetActive(false);
		}
		
		if(commName == true)
		{
			commName.SetActive(false);
			tutCheckbox.SetActive(false);
			commTextField.SetActive(false);
			beginButton.SetActive(false);
		}
		//Enable menu buttons
		startButton.SetActive(true);
		loadButton.SetActive(true);
		optionsButton.SetActive(true);
		exitButton.SetActive(true);
		
		//edit title
		editLabel.text = "Newly Trained Knights\n and the\n Flower Cultists";
		
	}
	

}
