using UnityEngine;
using System.Collections;

public class ButtonNo : MonoBehaviour {
	
	public GameObject overwritePanel;
	public GameObject backButton;
	public GameObject buttonBegin;
	
	void OnClick()
	{
		backButton.GetComponent<ButtonBack>().enabled = true;
		buttonBegin.GetComponent<BeginButton>().enabled = true;
		overwritePanel.SetActive(false);
	}
}
