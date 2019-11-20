using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour{
	public LayerMask findObjectMask;
	public GameObject shoot;
	public float shootDelay;

	private GameObject targetObject;
	private List<GameObject> objectsClose = new List<GameObject>();
	private float targetObjectDistance = float.MaxValue;
	private float shootTimer;


	private void FindClosestEnemy(){
		targetObject = null;
		if(objectsClose.Count == 0)
			return;
		foreach(GameObject obj in objectsClose){
			if(obj == gameObject)
				continue;
			float tmp = Vector3.Distance(transform.position, obj.transform.position);

			if(obj.CompareTag("Cube") || obj.CompareTag("Sphere") && tmp < targetObjectDistance){
				targetObject = obj;
				targetObjectDistance = tmp;
			}
		}
	}

	private void FireShoot(){
		GameObject spawnShoot = Instantiate(shoot, transform.position, Quaternion.identity);
		spawnShoot.GetComponent<Shoot>().SetTarget(targetObject);
	}

	void Update(){
		if(HelperFunctions.gamePaused)
			return;

		objectsClose = HelperFunctions.GetObjectsInRange(transform.position, 8f, findObjectMask);
		FindClosestEnemy();
		shootTimer += Time.deltaTime;

		if(targetObject != null && shootTimer >= shootDelay){
			shootTimer = 0;
			FireShoot();
		}
	}
}