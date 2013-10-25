using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public GameObject heldUnit;
	public int X;
	public int Y;

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	//set x and y position of grid square location
	public void setXY(int j, int k){
		X = j;
		Y = k;
	}
	//When a gameobject enters the collider
	void OnTriggerEnter(Collider collider){
		if(heldUnit==null){
			//set held unit
			heldUnit = collider.gameObject;
			
			//Unset the units previous grid
			collider.gameObject.GetComponent<UnitGenerics>().onGrid.heldUnit = null;
			
			//set 'setGrid' to this gameobject(square collider)
			collider.gameObject.GetComponent<UnitGenerics>().setGrid(this.GetComponent<Grid>());
		}
	}
	//when gameobject that entered collider leaves reset heldUnit to null
	void OnTriggerExit(Collider collider){
		if(collider.gameObject==heldUnit){
			heldUnit=null;
		}
	}
	
	//return heldUnit
	public GameObject returnUnit(){
		return heldUnit;
	}
	
	//return X and Y
	public Vector2 returnXY(){
		return new Vector2(X,Y);
	}
}
