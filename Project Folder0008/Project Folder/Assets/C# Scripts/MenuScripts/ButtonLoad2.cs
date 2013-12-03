using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ButtonLoad2 : MonoBehaviour {
	
	string file = "";
	string mission = "";
	
	public GameObject saveData;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void holdFile(string fileName)
	{
		file = fileName;
	}
	
	void OnClick()
	{
		if(file != "")
		{
			foreach(string line in  File.ReadAllLines(Application.persistentDataPath+"/AutoSaves/"+file))
			{
				mission = line;
			}
			saveData.GetComponent<StoreData>().MissionToLoad(mission);
			saveData.GetComponent<StoreData>().setAutoSaveName(file);
			Application.LoadLevel("Main");
		}
	}
	
	public string ReturnFileName()
	{
		return file;
	}
}
