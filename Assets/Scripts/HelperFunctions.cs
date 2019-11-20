using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HelperFunctions : MonoBehaviour{

	public static bool gamePaused = false;
	
	private static GameObject tDestroyer;
	private static List<GameObject> gList;
	
	[SerializeField]
	private GameObject triangleDestroyer;
	
	public static GameObject GetDestroyer(){
		return tDestroyer;
	}

	private void Awake(){
		tDestroyer = triangleDestroyer;
	}

	[Serializable]
	public enum Team{
		Sphere,
		Cube,
		Neutral
	}

	public static Quaternion RotateTowards(Quaternion curRot, Vector3 targetRot, float rotSpeed){
		Quaternion rot = Quaternion.LookRotation(targetRot);
		return Quaternion.Lerp(curRot, rot, rotSpeed * Time.deltaTime);
	}

	public static List<GameObject> GetObjectsInRange(Vector3 center, float radius, LayerMask layerMask){
		
		Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);
		
		gList = new List<GameObject>();
		
		for(int i = 0; i < hitColliders.Length; i++){
			gList.Add(hitColliders[i].gameObject);
		}
		return gList;
	}

	public static Vector3 GetPointInRange(Vector3 position, float minRange, float maxRange){ //bool checkIfClear = false
		float x = Random.Range(-maxRange, maxRange);
		float y = Random.Range(-maxRange, maxRange);

		if(x > -minRange && x < minRange)
			x = minRange;
		if(y > -minRange && y < minRange){
			y = minRange;
		}
		
		Vector3 pos = position;
		pos.x += x;
		pos.z += y;
		return pos;
	}

	public static bool BiggerThanEqual(Vector3 a, Vector3 b){
		return a.x >= b.x && a.y >= b.y && a.z >= b.z;
	}
	
	public static bool SmallerThanEqual(Vector3 a, Vector3 b){
		return a.x <= b.x && a.y <= b.y && a.z <= b.z;
	}
	
	public static Vector3 RandomPoint(Vector3 min, Vector3 max){
		Vector3 v;
		v.x = Random.Range(min.x, max.x);
		v.y = Random.Range(min.y, max.y);
		v.z = Random.Range(min.z, max.z);
		return v;
	}
	
	public static float VectorLength(Vector3 vec){
		return Mathf.Sqrt((float) (Math.Pow(vec.x, 2) + Math.Pow(vec.y, 2) + Math.Pow(vec.z, 2)));
	}
	
	
	
}
