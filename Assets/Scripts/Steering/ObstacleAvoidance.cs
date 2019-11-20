using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour{
	
	[Serializable]
	public struct SteeringRay{
		public Transform rayOrigin;
		public float rayLength;
	}

	[Serializable]
	public struct SteeringRays{
		public SteeringRay rayFront, rayLeft, rayRight;
	}

	public struct InputParameters{
		public float speed, maxSpeed;
		public float rotDir;
		//TODO: Add functionality for rayMask
		
	}

	public struct AvoidResults {
		public float rotDir;
		public float speed;
		public bool obstacleHit;
	}

	public SteeringRays steeringRays;
	public LayerMask rayMask;
	
	//public float sideRayRange = 3f, midRayRange = 1f;
	public float sideRotSpeed;
	
	
	private GameObject lastHitTarget;
	//private float dir;
	private int sideRayCounter;
	private List<bool> rayResult = new List<bool>();

	private AvoidResults aResult;
	private Vector3 hitPoint;
	private bool rightRayHit;

	private bool lastObHit;
	
	public AvoidResults AvoidObstacles(float speed, float maxSpeed, float rotDir){
		ClearResult();
		rightRayHit = false;
		float dir = rotDir;
		float lastDir = dir;
		hitPoint = Vector3.zero;
		
		//TODO: Left AND Right ray hit = double right turn
		//TODO: Change range based on speed
		if(DoRay(steeringRays.rayLeft.rayOrigin, steeringRays.rayLeft.rayLength)){
			dir += sideRotSpeed;
			rightRayHit = true;
			lastObHit = true;
			aResult.obstacleHit = true;
		}

		if(DoRay(steeringRays.rayRight.rayOrigin, steeringRays.rayRight.rayLength)){
			aResult.obstacleHit = true;
			lastObHit = true;
			if(rightRayHit)
				dir += sideRotSpeed;
			else
				dir -= sideRotSpeed;
		}

		if(DoRay(steeringRays.rayFront.rayOrigin, steeringRays.rayFront.rayLength)){
			float length = HelperFunctions.VectorLength(hitPoint - steeringRays.rayFront.rayOrigin.position);
			aResult.speed = (length / steeringRays.rayFront.rayLength ) * maxSpeed;
		} else {
			aResult.speed = -1;
		}
		aResult.rotDir = dir;
		return aResult;
	}

	private void ClearResult(){
		aResult.obstacleHit = false;
		aResult.rotDir = 0;
		aResult.speed = 0;
	}
	
	private void DrawRay(Vector3 start, Vector3 direction, Color color, float range = 1){
		Debug.DrawRay(start, direction * range, color);
	}

	private bool DoRay(Transform ray, float range){
		bool result = false;
		result = ShootRay(ray, range);
		if(!result)
			DrawRay(ray.position, ray.forward, Color.red, range);
		return result;
	}

	private bool ShootRay(Transform origin, float range){
		RaycastHit hit;
		if(Physics.Raycast(origin.position, origin.forward, out hit, range)){
			Debug.DrawRay(origin.position, origin.TransformDirection (Vector3.forward) * hit.distance, Color.yellow);
			lastHitTarget = hit.transform.gameObject;
			hitPoint = hit.point;
			return true;
		}
		return false;
	}
	
	
	
}