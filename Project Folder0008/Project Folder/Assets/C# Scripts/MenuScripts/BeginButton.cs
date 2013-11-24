using UnityEngine;
using System.Collections;

public class BeginButton : MonoBehaviour {
	public GameObject saveData;
	public UISprite tutorialCheck;
	
void OnClick()
	{
		saveData.GetComponent<StoreData>().DataStorage();
		if(tutorialCheck.alpha==1){
		Application.LoadLevel("Tutorial");	
		} else {
			Application.LoadLevel("Week6");
		}
	}
}
