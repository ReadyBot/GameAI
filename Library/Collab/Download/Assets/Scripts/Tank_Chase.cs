using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Chase : MonoBehaviour{
	
	
	private float movementSpeed = 5f;
	private float dir;
	
	void Update(){
		print(dir);
		dir = SteeringAlgorithm.Direction(10, dir, 100);
		transform.Rotate(0, Time.deltaTime * dir, 0);
		transform.position += transform.forward * Time.deltaTime * movementSpeed;
		
	}
}