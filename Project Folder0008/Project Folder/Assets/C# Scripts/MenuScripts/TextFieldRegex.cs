using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class TextFieldRegex : MonoBehaviour {

	UILabel uiLabelObject;
	
	void Start()
	{
		uiLabelObject = this.GetComponent<UILabel>();
	}
	// Update is called once per frame
	void Update () {
		
		uiLabelObject.text = Regex.Replace(uiLabelObject.text,"[^a-zA-Z0-9 ]","");
	}
}
