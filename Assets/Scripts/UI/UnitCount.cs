using UnityEngine;
using UnityEngine.UI;

public class UnitCount : MonoBehaviour{

	public Text cubeCount, sphereCount;
	
	void Update(){
		if(SpawnController.GetObjectList(SpawnController.SpawnType.Cube) != null){
			cubeCount.text = "" + SpawnController.GetObjectList(SpawnController.SpawnType.Cube).Count;
		} else{
			cubeCount.text = "" + 0;
		}
		if(SpawnController.GetObjectList(SpawnController.SpawnType.Sphere) != null){
			sphereCount.text = "" + SpawnController.GetObjectList(SpawnController.SpawnType.Sphere).Count;
		} else{
			sphereCount.text = "" + 0;
		}
	}
}