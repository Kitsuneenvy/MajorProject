using UnityEngine;
using System.Collections;

public class BeginButton : MonoBehaviour {
	public GameObject saveData;
	
void OnClick()
	{
		saveData.GetComponent<StoreData>().DataStorage();
		
		Application.LoadLevel("Week6");	
	}
}
