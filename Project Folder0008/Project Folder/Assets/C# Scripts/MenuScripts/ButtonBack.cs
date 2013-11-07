using UnityEngine;
using System.Collections;

public class ButtonBack : MonoBehaviour {
	
	UILabel editLabel;
	
	//menu buttons
	public GameObject startButton;
	public GameObject loadButton;
	public GameObject optionsButton;
	public GameObject exitButton;
	//Load Menu
	public GameObject deleteButton;
	public GameObject loadButton2;
	//options menu
	public GameObject slider;
	public GameObject checkBox;
	public GameObject musicVolume;
	//start game menu
	public GameObject commName;
	public GameObject tutCheckbox;
	public GameObject commTextField;
	public GameObject beginButton;
	public GameObject saveFileName;
	public GameObject saveTextField;
	
	void Start()
	{
		//assign
		editLabel = GameObject.Find("NTKFC").GetComponent<UILabel>();
	}

	// Use this for initialization
	void OnClick () {
		
		//disable back button and text
		this.gameObject.SetActive(false);
		if(loadButton2.activeSelf == true)
		{
			loadButton2.SetActive(false);
			deleteButton.SetActive(false);
			foreach(GameObject save in loadButton.GetComponent<ButtonLoad>().returnAutoSaves())
			{
				Destroy(save);
			}
			loadButton.GetComponent<ButtonLoad>().ClearList();
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
			saveFileName.SetActive(false);
			saveTextField.SetActive(false);
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
