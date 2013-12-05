using UnityEngine;
using System.Collections;
using System.IO;

public class BeginButton : MonoBehaviour {
	public GameObject saveData;
	public UISprite tutorialCheck;
	public GameObject savFileTxtFldLabel;
	public GameObject overwritePanel;
	public GameObject backButton;
	
void OnClick()
	{
		if(File.Exists(Application.persistentDataPath+"/AutoSaves/"+savFileTxtFldLabel.GetComponent<UILabel>().text+".sav"))
		{
			overwritePanel.SetActive(true);
			backButton.GetComponent<ButtonBack>().enabled = false;
			this.GetComponent<BeginButton>().enabled = false;
		}
		else
		{
			saveData.GetComponent<StoreData>().DataStorage();
			if(tutorialCheck.alpha==1){
				Application.LoadLevel("Tutorial");	
			} 
			else {
				Application.LoadLevel("Main");
			}
		}
	}
}
