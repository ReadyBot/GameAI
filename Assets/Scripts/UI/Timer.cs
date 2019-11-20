using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{

	public Text timerText;

	private int min = 0, sec = 0;
	private float dec = 0;
	
	private void Update(){
		if(HelperFunctions.gamePaused)
			return;

		
		dec += Time.deltaTime;
		
		if(dec > 1){
			dec -= 1;
			sec += 1;
		}

		if(sec == 60){
			sec = 0;
			min += 1;
		}

		if(min == 99){
			min = 0;
		}
		
		CalculateText();
	}
	
	private void CalculateText(){
		string str = "";

		if(min < 10)
			str += "" + 0;
		str += min + ":";

		if(sec < 10)
			str += "" + 0;
		str += sec + ".";

		int decInt = (int) (dec * 100);
		if(decInt < 10)
			str += "" + 0;
		str += decInt;
		
		timerText.text = str;

	}
	
	
	
}