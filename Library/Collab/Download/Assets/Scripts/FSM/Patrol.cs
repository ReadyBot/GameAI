using UnityEngine;

public class Patrol : MonoBehaviour{
	private readonly float accuracyWP = 1.0f;

	private int currentWP;
	private readonly float timeSpeed = 20f;

	public GameObject[] waypoints;

	private void Start(){
	}
    
	private void Update(){
		var distance = Vector3.Distance(waypoints[currentWP].transform.position, transform.position);

		if(waypoints.Length > 1){ 
			if(distance < accuracyWP){
				currentWP++;
				if(currentWP >= waypoints.Length) currentWP = 0;
			}

			transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWP].transform.position, timeSpeed * Time.deltaTime);
			transform.LookAt(waypoints[currentWP].transform);
		}
	}
}