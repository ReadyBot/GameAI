using UnityEngine;
using UnityEngine.UI;

public class PausedText : MonoBehaviour{

	public Text text;
	
	void Update(){

		text.enabled = HelperFunctions.gamePaused;

	}
}