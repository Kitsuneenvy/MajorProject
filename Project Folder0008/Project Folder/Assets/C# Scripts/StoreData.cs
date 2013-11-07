using UnityEngine;
using System.Collections;
using System.IO;

public class StoreData : MonoBehaviour {
	
	string autoSaveName = "";
	string commanderName = "";
	public GameObject saveField;
	public GameObject commField;
	string mission = "";
	bool saved = false;
	
	// Use this for initialization
	void Start () {
		
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Update()
	{
		if(Application.loadedLevelName != "OptionsWorking" && saved == false)
		{
			saved = true;
			CreateFile();
		}
	}
	public void DataStorage()
	{Debug.Log("RUN");
		autoSaveName =  saveField.GetComponent<UIInput>().text;
		commanderName = commField.GetComponent<UIInput>().text;
		Debug.Log(autoSaveName);
	}
	
	public string returnAutoSaveName()
	{
		return autoSaveName;
	}
	
	string returnCommName()
	{
		return commanderName;
	}
	
	void CreateFile()
	{
		Debug.Log(autoSaveName);
		
		if(!File.Exists("Assets/AutoSaves/"+autoSaveName+".sav"))
		{
			File.Create("Assets/AutoSaves/"+autoSaveName+".sav");
		}
	}
	
	public void MissionToLoad(string[] currentMission)
	{
		mission = currentMission[0];
	}
	
	public string ReturnMission()
	{
		return mission;
	}
}
