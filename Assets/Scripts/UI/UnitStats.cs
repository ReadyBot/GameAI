using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour{

	public Text teamText, moveStateText, combatStateText;
	public Slider hpSlider;

	private HPController hpc;
	private FSMController fsm;
	
	private static GameObject clickedObject = null;
	private static bool newObjClicked = false;
	
	public static void SetClickedObject(GameObject obj){
		clickedObject = obj;
		newObjClicked = true;
	}


	private void Update(){

		if(newObjClicked){
			newObjClicked = false;
			hpc = clickedObject.GetComponent<HPController>();
			fsm = clickedObject.GetComponent<FSMController>();
		}

		if(hpSlider == null || fsm == null)
			return;

		hpSlider.maxValue = hpc.MaxHp;
		hpSlider.value = hpc.HP;

		if(fsm.team == HelperFunctions.Team.Cube)
			teamText.text = "Cube";
		else if(fsm.team == HelperFunctions.Team.Sphere)
			teamText.text = "Sphere";
		else if(fsm.team == HelperFunctions.Team.Neutral)
			teamText.text = "Neutral";
		
		UpdateStateText();

	}


	private void UpdateStateText(){
		
		switch(fsm.MovementState){
			case StateMachine.MovementState.Idle:
				moveStateText.text = "Idle";
				break;
			case StateMachine.MovementState.Wander:
				moveStateText.text = "Wander";
				break;
			case StateMachine.MovementState.Flee:
				moveStateText.text = "Flee";
				break;
			case StateMachine.MovementState.Seek:
				moveStateText.text = "Seek";
				break;
			case StateMachine.MovementState.Patrol:
				moveStateText.text = "Patrol";
				break;
			case StateMachine.MovementState.Interpose:
				moveStateText.text = "Interpose";
				break;
		}
		
		switch(fsm.CombatState){
			case StateMachine.CombatState.Idle:
				combatStateText.text = "Idle";
				break;
			case StateMachine.CombatState.Defend:
				combatStateText.text = "Defend";
				break;
			case StateMachine.CombatState.Lookout:
				combatStateText.text = "Lookout";
				break;
			case StateMachine.CombatState.Death:
				combatStateText.text = "Death";
				break;
			case StateMachine.CombatState.Attack:
				combatStateText.text = "Attack";
				break;
			case StateMachine.CombatState.Capture:
				combatStateText.text = "Capture";
				break;
		}
		
	}
	
}