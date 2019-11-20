using UnityEngine;

public class SteeringAlgorithm : MonoBehaviour{

	public static float Direction(float rotSpeed, float dir, int turnLimit = 145){
		float rng = Random.Range(-1, 2);
		dir += rng * rotSpeed;
		if(dir > turnLimit)
			dir = turnLimit * -1;
		if(dir < -turnLimit)
			dir = turnLimit;
		return dir;
	}
	
}