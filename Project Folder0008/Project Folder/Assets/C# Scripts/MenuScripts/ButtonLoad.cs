using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ButtonLoad : MonoBehaviour {
	
	UILabel editLabel;
	
	public GameObject backButton;
	public GameObject autoSave;
	public GameObject background;
	public GameObject loadButton2;
	public GameObject startButton;
	public GameObject exitButton;
	public GameObject optionsButton;
	public GameObject deleteButton;
	
	List<GameObject> holdAutoSaves = new List<GameObject>();
	List<string> splitString = new List<string>();
	
	GameObject temp = null;
	string tempString = "";
	float increaseY = 0.2f;
	void Start()
	{
		//assign
		editLabel = GameObject.Find("NTKFC").GetComponent<UILabel>();
	}
	
	void OnClick() {
		
		//disable menu buttons
		this.gameObject.SetActive(false);
		startButton.SetActive(false);
		exitButton.SetActive(false);
		optionsButton.SetActive(false);
		
		increaseY = 100f;
		//change title
		editLabel.text = "Load Menu";
		
		//enable buttons
		backButton.SetActive(true);
		deleteButton.SetActive(true);
		loadButton2.SetActive(true);
		//display saved games
		foreach(string file in Directory.GetFiles(Application.persistentDataPath+"/AutoSaves/"))
		{
			temp = GameObject.Instantiate(autoSave) as GameObject;
			holdAutoSaves.Add(temp);
			temp.transform.parent = GameObject.Find("Menu").transform;
			tempString = file;
			foreach(string split in tempString.Split('/'))
			{
				if(split.Contains(".sav"))
				{
					if(!split.Contains("AutoSaves"))
					{
						tempString = split;
					}
				}
			}
			tempString = tempString.Split('.')[0];
			temp.GetComponentInChildren<UILabel>().text = tempString;
			temp.transform.localScale = new Vector3(1,1,0);
			Vector3 newPosition = new Vector3(0,increaseY,0.5f);
			temp.transform.localPosition = newPosition;
			increaseY -= 25.0f;
			
		}
	}
	
	public List<GameObject> returnAutoSaves()
	{
		return holdAutoSaves;
	}
	
	public void ClearList()
	{
		holdAutoSaves.Clear();
	}
}
