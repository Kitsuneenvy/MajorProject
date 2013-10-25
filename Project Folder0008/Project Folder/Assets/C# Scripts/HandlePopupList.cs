using UnityEngine;
using System.Collections;

public class HandlePopupList : MonoBehaviour {
	bool created = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnSelectionChange(string selectedValue){
		if(selectedValue=="Attack"){
			Debug.Log("Selected Attack");
			GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setMovementState(false);
			GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setAttackState(true);
			GameObject.Find("Panel").GetComponent<DropDownMenu>().setCreated(false);
			Destroy(GameObject.Find("Drop-down List"),0.0f);
		} else if (selectedValue=="Move"){
			if(created==false){
				created = true;
			} else {
				GameObject.Find("Panel").GetComponent<DropDownMenu>().setCreated(false);
				GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setAttackState(false);
				GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setMovementState(true);
			Debug.Log("Selected Move");
			Destroy(GameObject.Find("Drop-down List"),0.0f);
			}
		} else if (selectedValue=="Cancel"){
		GameObject.Find("Panel").GetComponent<DropDownMenu>().resetSelectedUnit();
			GameObject.Find("Panel").GetComponent<DropDownMenu>().setCreated(false);
				GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setAttackState(false);
				GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setMovementState(false);
			Debug.Log("Cancelled");
			Destroy(GameObject.Find("Drop-down List"),0.0f);
		}
	}
}
