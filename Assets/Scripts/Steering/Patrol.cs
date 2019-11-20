using UnityEngine;

public class Patrol : MonoBehaviour{
	private readonly float accuracyWP = 7.0f;

	private int currentWP;
	private readonly float timeSpeed = 10f;
    //float rotSpeed = 0.2f;

    public GameObject[] waypoints;
    
	private void Update(){
		if(HelperFunctions.gamePaused)
			return;

		
		if(waypoints.Length >= 1){
            var distance = Vector3.Distance(waypoints[currentWP].transform.position, transform.position);
            Vector3 direction = waypoints[currentWP].transform.position - transform.position;

            if (distance < accuracyWP){
				if (currentWP < waypoints.Length) currentWP++;
                if (currentWP >= waypoints.Length) currentWP = 0;
			}

            transform.LookAt(waypoints[currentWP].transform.position);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWP].transform.position, timeSpeed * Time.deltaTime);
			
		}
	}
}