using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour{
	
	public GameObject sphere, cube;
	public Transform spawnedObjectParent;
	
	
	private static Dictionary<SpawnType, List<GameObject>> SpawnedObjects = new Dictionary<SpawnType, List<GameObject>>();
	
	private static Transform sop = null; //Spawned Object Parent

	private static GameObject staticCube, staticSphere;
	
	public enum SpawnType{
		Sphere,
		Cube
	}

	private void Awake(){
		sop = spawnedObjectParent;
		staticCube = cube;
		staticSphere = sphere;
	}

	public static List<GameObject> GetObjectList(SpawnType type){
		if(SpawnedObjects.ContainsKey(type)){
			//print(SpawnedObjects[type].Count);
			return SpawnedObjects[type];
		}
		return null;
	}

	public static void DestroyObject(GameObject obj){
		foreach(KeyValuePair<SpawnType,List<GameObject>> pair in SpawnedObjects){
			if(pair.Value.Contains(obj)){
				pair.Value.Remove(obj);
				Destroy(obj);
				break;
			}
		}
	}
	
	public static GameObject SpawnObject(SpawnType spawnType, Vector3 position, Quaternion rotation, Transform parent = null){
		GameObject obj = null;
		GameObject prefab = null;
		if(spawnType == SpawnType.Cube)
			prefab = staticCube;
		else if(spawnType == SpawnType.Sphere)
			prefab = staticSphere;
		if(parent == null)
			parent = sop;
		
		obj = Instantiate(prefab, position, rotation, parent);
		
		List<GameObject> objList = GetObjectList(spawnType);
		if(objList != null){
			objList.Add(obj);
		} else{
			objList = new List<GameObject>();
			objList.Add(obj);
			SpawnedObjects.Add(spawnType, objList);
		}
		
		return obj;
	}

	
	private void Update(){
		if(HelperFunctions.gamePaused)
			return;

        if (Input.GetKeyUp(KeyCode.T)){
			SpawnObject(SpawnType.Sphere, new Vector3(-27f, 0.5f, 24f), Quaternion.identity);
        }
		if(Input.GetKeyUp(KeyCode.Y)){
			SpawnObject(SpawnType.Cube, new Vector3(15f, 0.5f, 6f), Quaternion.identity);
		}

		if(Input.GetKeyUp(KeyCode.U)){
			StartCoroutine(SpawnObjects(200));
		}
		
	}

	IEnumerator SpawnObjects(int count, float delay = 1000f){
		for(int i = 0; i < count; i++){
			SpawnObject(SpawnType.Sphere, new Vector3(-27f, 0.5f, 24f), Quaternion.identity);
			SpawnObject(SpawnType.Cube, new Vector3(15f, 0.5f, 6f), Quaternion.identity);
			yield return delay;
		}
	}
	
	
}
