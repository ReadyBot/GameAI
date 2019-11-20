using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Chase : MonoBehaviour{
	
	
	private float movementSpeed = 5f;
	private int rotationSpeed = 10;
	private int direction;

	void Direction(){
		int rng = Random.Range(-rotationSpeed, rotationSpeed);
		direction += rng;
		print(direction);
		if(direction > 145)
			direction = -145;
		if(direction < -145)
			direction = 145;
		transform.Rotate(0, Time.deltaTime * direction, 0);
	}

	private float dir;
	
	void Update(){
		print(dir);
		dir = SteeringAlgorithm.Direction(10, dir, 100);
		transform.Rotate(0, Time.deltaTime * dir, 0);
		transform.position += transform.forward * Time.deltaTime * movementSpeed;
		
	}
}