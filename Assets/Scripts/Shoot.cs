using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour{
	private GameObject targetObject;
	private bool ready = false;


	public void SetTarget(GameObject target){
		targetObject = target;
		ready = true;
	}

	void Update(){
		if(!ready)
			return;

		if(targetObject == null){
			Destroy(gameObject);
			return;
		}

		transform.LookAt(targetObject.transform.position);
		transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, 10 * Time.deltaTime);
		if(Vector3.Distance(targetObject.transform.position, transform.position) < 0.5f){
			HPController hpc = targetObject.GetComponent<HPController>();
			hpc.DealDamage(hpc.MaxHp / 5);
			Destroy(gameObject);
		}
	}
}