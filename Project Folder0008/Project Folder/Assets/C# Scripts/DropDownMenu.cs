using UnityEngine;
using System.Collections;

public class DropDownMenu : MonoBehaviour {
	UIPopupList uipopuplistObject;
	public GameObject popList;
	GameObject backgroundSprite;
	bool setScriptObject = false;
	GameObject selectedUnit = null;
	//is to be sent to other script to tell camera when to stop moving
	public bool cameraLock = false;
	bool created = false;
	GameObject storeUnit;
	GameObject Dropdown;
	GameObject newObject;
	public LayerMask gridMask;
	GameObject manager;
	
	// Use this for initialization
	void Start () {
		manager = GameObject.Find("Game Manager");
	}
	
	// Update is called once per frame
	void Update () {
		
		//if background sprite is created
		if(Dropdown!=null){
			
			//set it to this position (unsure where but other location is reseting scale so this is in update to counter act it)
			//backgroundSprite.transform.localScale = new Vector3(60.0f,60.0f,1.0f);
			
			//set camera lock to true
			cameraLock = true;
		}
		if(!created)
		{
			
			cameraLock = false;
			//get player input for seleceted grid
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{	
				//creates a ray from mouse position
				Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				//create a variable to contain rayhit location
				RaycastHit rayHit;
				
				//cast ray and output hit location to rayHit
				if(Physics.Raycast(mouseRay,out rayHit,Mathf.Infinity,gridMask.value))
				{
					//check if hit square collider contains a Unit
					if(rayHit.collider.gameObject.GetComponent<Grid>().returnUnit() != null 
						&& manager.GetComponent<gameManage>().playerTurn==true
						&&rayHit.collider.gameObject.GetComponent<Grid>().heldUnit.tag=="PlayerUnit"
						&&(selectedUnit==null||selectedUnit==rayHit.collider.gameObject.GetComponent<Grid>().heldUnit)){
						selectedUnit=rayHit.collider.GetComponent<Grid>().heldUnit;
						//set to false
						//created = false;
						storeUnit = rayHit.collider.gameObject.GetComponent<Grid>().returnUnit();
						//instantiate popup list (drop down menu)
						if(Dropdown ==null){
							Dropdown =  Instantiate(popList) as GameObject;
						}
						//set dropdwon to the created pop up lost
						Dropdown.GetComponent<UIPopupList>().eventReceiver= GameObject.Find("Game Manager");
						
						//uipopuplistObject needs to be set
						if(setScriptObject == false)
						{
							//set uipopuplist
							uipopuplistObject = Dropdown.GetComponent<UIPopupList>();
							
							//setscriptObject to true
							setScriptObject = true;
						}
						
						//set the parent of the created popup list to panel
						Dropdown.transform.parent = GameObject.Find("Anchor-MiddleRight").transform;
						//run Onclick
						uipopuplistObject.OnClick();
						
						newObject = GameObject.Find("Drop-down List");
						Bounds boundSize = NGUIMath.CalculateRelativeWidgetBounds(newObject.transform);
						newObject.transform.localPosition = new Vector3(-(boundSize.size.x*1.5f),((boundSize.size.y)),0);
						
						//if a pop up list is created
						if(uipopuplistObject.createdDropDownList == true)
						{		
								//drop down menu is created set to true
								created = true;
						}
						//set the popup list to location of returned Unit
						//Dropdown.transform.position = rayHit.collider.gameObject.transform.position;
						
						}
				}
					}
				}
			//}
		//}
		/*else if (created)
		{
			if(Input.GetMouseButton(0))
			{
				Destroy(GameObject.Find("Drop-Down List"));
			}
		}*/
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		//We need to figure out a way to get the dropdown menu to disappear
		//Press enter so select an option
		//if(Input.GetKeyDown(KeyCode.Return))
		//{
			//uipopuplistObject.OnItemPress(	
		//}

	}
	public void setCreated(bool newSet){
		created = newSet;
	}
	public GameObject returnStoreUnit(){
		return storeUnit;
	}
	public void resetSelectedUnit(){
		selectedUnit=null;
	}
}
