using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour{
	
	
	private void OnMouseUp(){
		UnitStats.SetClickedObject(gameObject);
	}
	
	
}