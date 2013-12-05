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
	FileStream createdFile;
	public UISprite tutorialCheck;
	bool overwrite = false;
	// Use this for initialization
	void Start () {
		
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Update()
	{
		if(Application.loadedLevelName != "OptionsWorking" && saved == false && !File.Exists(Application.persistentDataPath+"/AutoSaves/"+autoSaveName))
		{
			saved = true;
			CreateFile();
		}
	}
	public void DataStorage()
	{
		autoSaveName =  saveField.GetComponent<UIInput>().text;
		commanderName = commField.GetComponent<UIInput>().text;
		
		if(overwrite == true)
		{
			OverwriteFile();
		}
	}
	
	public string returnAutoSaveName()
	{
		return autoSaveName;
	}
	
	public string returnCommName()
	{
		return commanderName;
	}
	
	void CreateFile()
	{
		if(!Directory.Exists(Application.persistentDataPath+"/AutoSaves"))
		{
			Directory.CreateDirectory(Application.persistentDataPath+"/AutoSaves");
		}
		if(!File.Exists(Application.persistentDataPath+"/AutoSaves/"+autoSaveName + ".sav"))
		{
			createdFile = File.Create(Application.persistentDataPath+"/AutoSaves/"+autoSaveName + ".sav");
			createdFile.Close();
			using(StreamWriter sw = new StreamWriter(Application.persistentDataPath+"/AutoSaves/"+autoSaveName+".sav",false))
			{
				sw.WriteLine(commanderName);
			}
		}
	}
	
	public void OverwriteFile()
	{
		overwrite = true;
		
		if(autoSaveName == null || autoSaveName == "")
		{
			DataStorage();
		}
		else
		{
			using(StreamWriter sw = new StreamWriter(Application.persistentDataPath+"/AutoSaves/"+autoSaveName+".sav",false))
			{
				sw.WriteLine(commanderName);
			}
			if(tutorialCheck.alpha==1){
					Application.LoadLevel("Tutorial");	
				} 
				else {
					Application.LoadLevel("Main");
				}
		}
	}
	
	public void MissionToLoad(string currentMission)
	{
		mission = currentMission;
	}
	
	public string ReturnMission()
	{
		return mission;
	}
	
	public void setAutoSaveName(string name)
	{
		autoSaveName = name;
	}

}
