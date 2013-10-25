using UnityEngine;
using System.Collections;

public class DisableButton: MonoBehaviour {
	
	void OnClick()
	{
		//deactivate buttons
		GameObject.Find("StartButton").SetActive(false);
		GameObject.Find("ExitButton").SetActive(false);
		GameObject.Find("OptionsButton").SetActive(false);
		GameObject.Find("InstructionsButton").SetActive(false);
	}
}
