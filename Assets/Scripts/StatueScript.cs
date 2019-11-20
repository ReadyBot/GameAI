using System;
using UnityEngine;

public class StatueScript : MonoBehaviour{
	
	//TODO: BUG: Sphere enter statue, bot no convert

	public int DefenderCount{
		get{ return curDefenders; }
	}

	public int DefendMax{
		get{
			if(settings.team == HelperFunctions.Team.Cube)
				return settings.defenceCountCube;
			if(settings.team == HelperFunctions.Team.Sphere)
				return settings.defenceCountSphere;
			return 0;
		}
	}

	public HelperFunctions.Team Team{
		get{ return settings.team; }
	}

	[Serializable]
	public struct StatueSettings{
		public GameObject cubePrefab;
		public GameObject spherePrefab;
		public GameObject cubeStatue, sphereStatue, neutralStatue;
		[Range(0f, 10f)]
		public float spawnRadius;
		[Range(0, 10)]
		public int defenceCountCube;
		[Range(0, 10)]
		public int defenceCountSphere;
		public float spawnIntervalCube;
		public float spawnIntervalSphere;
		public HelperFunctions.Team team;
	}

	public StatueSettings settings;
	
	private int curDefenders = 0;
	private float spawnTimer;
	private Vector3 spawnPoint;

	private FSMController fsm;
	private GameObject spawnedObject;
	private StateMachine.StatueState state;
	private int captureProgress;
	private int captureLimit = 6;
	//Positive capture side is for cube, negative is for sphere
	//When capping from enemy to yours, if you get prograss to 0
	// the statue will become neutral first

	private void Awake(){
		if(settings.team == HelperFunctions.Team.Cube){
			state = StateMachine.StatueState.Cube;
			captureProgress = captureLimit;
		} else if(settings.team == HelperFunctions.Team.Sphere){
			state = StateMachine.StatueState.Sphere;
			captureProgress = captureLimit * -1;
		} else
			state = StateMachine.StatueState.Neutral;
		
		SetVisualStatue();
		
	}

	public void ReduceDefenderCount(){ curDefenders--; }

	public void DoCapture(HelperFunctions.Team toTeam, int amount){
		
		if(toTeam == HelperFunctions.Team.Cube){
			captureProgress += amount;
		} else if(toTeam == HelperFunctions.Team.Sphere){
			captureProgress -= amount;
		}
		
		if(captureProgress == 0){
			//Set to Neutral team
			state = StateMachine.StatueState.Neutral;
			settings.team = HelperFunctions.Team.Neutral;
			SetVisualStatue();
		} else if(captureProgress == captureLimit){
			//Set to Cube team
			state = StateMachine.StatueState.Cube;
			settings.team = HelperFunctions.Team.Cube;
			SetVisualStatue();
		} else if(captureProgress == -captureLimit){
			//Set to Sphere team
			state = StateMachine.StatueState.Sphere;
			settings.team = HelperFunctions.Team.Sphere;
			SetVisualStatue();
		}
	}

	private void SpawnObject(){
		//TODO: Spawn logic
		if(settings.team == HelperFunctions.Team.Neutral)
			return;

		spawnTimer += Time.deltaTime;
		if(settings.team == HelperFunctions.Team.Cube && spawnTimer >= settings.spawnIntervalCube){
			spawnTimer = 0;
			spawnPoint = HelperFunctions.GetPointInRange(transform.position, 1f, settings.spawnRadius);
			spawnedObject = SpawnController.SpawnObject(SpawnController.SpawnType.Cube, spawnPoint, Quaternion.identity);
			
		} else if(settings.team == HelperFunctions.Team.Sphere && spawnTimer >= settings.spawnIntervalSphere){
			spawnTimer = 0;
			spawnPoint = HelperFunctions.GetPointInRange(transform.position, 1f, settings.spawnRadius);
			spawnedObject = SpawnController.SpawnObject(SpawnController.SpawnType.Sphere, spawnPoint, Quaternion.identity);
			
		} else{
			return;
		}

		fsm = spawnedObject.GetComponent<FSMController>();
		if(settings.team == HelperFunctions.Team.Cube && curDefenders < settings.defenceCountCube){
			fsm.SetDefenceTarget(gameObject);
			curDefenders++;
		} else if(settings.team == HelperFunctions.Team.Sphere && curDefenders < settings.defenceCountSphere){
			fsm.SetDefenceTarget(gameObject);
			curDefenders++;
		}

	}

	private void SetVisualStatue(){
		switch(state){
			case StateMachine.StatueState.Cube:
				spawnedObject = settings.cubePrefab;
				settings.cubeStatue.SetActive(true);
				settings.sphereStatue.SetActive(false);
				settings.neutralStatue.SetActive(false);
				break;
			case StateMachine.StatueState.Sphere:
				spawnedObject = settings.spherePrefab;
				settings.cubeStatue.SetActive(false);
				settings.sphereStatue.SetActive(true);
				settings.neutralStatue.SetActive(false);
				break;
			case StateMachine.StatueState.Neutral:
				spawnedObject = null;
				settings.cubeStatue.SetActive(false);
				settings.sphereStatue.SetActive(false);
				settings.neutralStatue.SetActive(true);
				break;
		}
	}
	
	
	
	

	private void Update(){
		if(HelperFunctions.gamePaused)
			return;

		SpawnObject();
	}
}
