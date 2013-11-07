using UnityEngine;
using System.Collections;
using System.IO;

public class ButtonLoad2 : MonoBehaviour {
	
	string file = "";
	string[] mission;
	
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
		mission = File.ReadAllLines("Assets/AutoSaves/"+file);
		saveData.GetComponent<StoreData>().MissionToLoad(mission);
		Application.LoadLevel("Week6");
	}
}
