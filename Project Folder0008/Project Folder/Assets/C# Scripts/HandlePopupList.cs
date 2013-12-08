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
			GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().resetInactive();
			GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setMovementState(false);
			GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setAttackState(true);
			GameObject.Find("Panel").GetComponent<DropDownMenu>().setCreated(false);
			Destroy(GameObject.Find("Drop-down List"),0.0f);
		} else if (selectedValue=="Move"){
			if(created==false){
				created = true;
			} else {
				GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().resetInactive();
				GameObject.Find("Panel").GetComponent<DropDownMenu>().setCreated(false);
				GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setAttackState(false);
				GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setMovementState(true);
			Destroy(GameObject.Find("Drop-down List"),0.0f);
			}
		} else if (selectedValue=="Cancel"){
			GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManage>().resetInactive();
			GameObject.Find("Panel").GetComponent<DropDownMenu>().resetSelectedUnit();
			GameObject.Find("Panel").GetComponent<DropDownMenu>().setCreated(false);
			GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setAttackState(false);
			GameObject.Find("Panel").GetComponent<DropDownMenu>().returnStoreUnit().GetComponent<UnitGenerics>().setMovementState(false);
			foreach(GameObject gridObject in this.GetComponent<GridTool>().returnGridColliders()){
				gridObject.renderer.material = Resources.Load ("Transparent") as Material;
				gridObject.renderer.material.color = Color.black;
			}
			Destroy(GameObject.Find("Drop-down List"),0.0f);
		}
	}
}
