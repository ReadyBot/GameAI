using UnityEngine;

public class StateMachine : MonoBehaviour{

	public enum MovementState{
		Idle,
		Wander,
		Seek,
        Patrol,
        Flee,
        Interpose
    }

    public enum CombatState{
	    Idle,
        Lookout,
	    Attack,
	    Capture,
        Defend,
        Death
    }

    public enum StatueState{
	    Cube,
	    Sphere,
	    Neutral
    }
    
    
}