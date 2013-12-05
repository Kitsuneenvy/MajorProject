using UnityEngine;
using System.Collections;

public class ButtonYes : MonoBehaviour {
	
	public GameObject dataStorage;
	
	void OnClick()
	{
		dataStorage.GetComponent<StoreData>().OverwriteFile();
	}
}
