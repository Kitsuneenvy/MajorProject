using UnityEngine;
using System.Collections;

public class LoadOnHover : MonoBehaviour {
	GameObject loadText;
	public bool hover = false;
	void OnHover()
	{
		this.gameObject.GetComponent<UILabel>().color = Color.grey;
		hover = true;
	}
	
	void Update()
	{
		/*if(hover == true)
		{
			
			Ray mouseRay =	Camera.current.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;
			if(Physics.Raycast(mouseRay,out rayHit))
			{
				if(rayHit.collider.gameObject.name != "LoadText")
				{
					this.gameObject.GetComponent<UILabel>().color = Color.green;
					hover = false;
				}
			}
		}*/
		
	}
}
