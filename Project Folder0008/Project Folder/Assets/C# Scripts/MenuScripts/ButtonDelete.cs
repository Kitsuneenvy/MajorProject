using UnityEngine;
using System.Collections;
using System.IO;

public class ButtonDelete : MonoBehaviour {
	
	string file = "";
	public GameObject load;
	GameObject objectToDelete = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void holdFile(string fileName, GameObject sentObject)
	{
		file = fileName;
		objectToDelete = sentObject;
	}
	
	void OnClick()
	{
		File.Delete(Application.persistentDataPath+"/AutoSaves/"+file+".sav");
		load.GetComponent<ButtonLoad>().returnAutoSaves().Remove(objectToDelete);
		Destroy(objectToDelete);
		
	}
}
