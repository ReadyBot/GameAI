using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HPController))]
public class FSMController : MonoBehaviour{
    //private float wanderTurnSpeed = 0.5f;
    private float maxSpeed = 8f, speed = 1f;
    private float rotSpeed = 1f;
    public float acceleration = 1f;
    public float fSpeedWhenRotating = 1.5f;

    private StateMachine.MovementState mState = StateMachine.MovementState.Wander;
    private StateMachine.CombatState cState = StateMachine.CombatState.Idle;

    private ObstacleAvoidance.AvoidResults result;

    private float rotateDir;
    private Rigidbody rigidbody;
    private ObstacleAvoidance oba;
    private Vector3 targetPosistion;
    private GameObject defenceTarget; //Has to be a statue atm
    private StatueScript statueScript;

    public HelperFunctions.Team team;
    private List<GameObject> objectsClose = new List<GameObject>();
    public LayerMask findObjectMask;
    public float findObjectRange;

    public float seekRange;
    public float attackRange, dps, defendRange, fleeDistance;
    private GameObject targetObject;
    private FSMController targetScript;
    private float targetObjectDistance = float.MaxValue;

    private float delayTimer, delayTime = 0.01f; //Delay time between fsm checks
    private float objectScanTimer, objectScanDelay = 0.1f; //Delay to find objects around
    private bool hitObstacle;

    public string cubeTag = "Cube", sphereTag = "Sphere";
    public string destroyerTag = "Destroyer", statueTag = "Statue";

    private bool isDead;
    private HPController hpC;

    [Header("Debug Stuff")] public bool wanderActive = true;

    public GameObject DefenceTarget{
        set{ defenceTarget = value; }
    }

    public StateMachine.MovementState MovementState{
        get{ return mState; }
    }

    public StateMachine.CombatState CombatState{
        get{ return cState; }
    }

    private void Awake(){
        oba = GetComponent<ObstacleAvoidance>();
        rigidbody = GetComponent<Rigidbody>();
        hpC = GetComponent<HPController>();
    }

    public void SetDefenceTarget(GameObject target){
        defenceTarget = target;
    }

    private void Update(){
        if(HelperFunctions.gamePaused)
            return;

        if(hpC.HP <= 0)
            cState = StateMachine.CombatState.Death;

        //If this object is dead, dont check anything
        if(isDead)
            return;

        rotSpeed = speed / maxSpeed;

        if(cState == StateMachine.CombatState.Attack){
            //Super acceleration if in attack mode :D
            speed += (acceleration * 3) * Time.deltaTime;
        } else{
            speed += acceleration * Time.deltaTime;
        }

        if(speed >= maxSpeed){
            speed = maxSpeed;
        }


        objectScanTimer += Time.deltaTime;
        if(objectScanTimer >= objectScanDelay){
            objectScanTimer = 0;
            //Heavy process, dont run too often
            objectsClose = HelperFunctions.GetObjectsInRange(transform.position, findObjectRange, findObjectMask);
        }


        delayTimer += Time.deltaTime;
        if(delayTimer >= delayTime){
        }

        delayTimer = 0;
        result = oba.AvoidObstacles(speed, maxSpeed, rotateDir);
        if(result.speed >= 0){
            speed = result.speed;
        }

        if(defenceTarget == null){
            statueScript = null;
        } else{
            if(statueScript == null){
                statueScript = defenceTarget.GetComponent<StatueScript>();
            }
        }

        StateCheck();


        StateAction();
        if(mState == StateMachine.MovementState.Idle)
            return;
        RotateObject(result.rotDir);
    }

    private void FixedUpdate(){
        if(HelperFunctions.gamePaused)
            return;

        //Dont move if you are idle, or dead
        if(cState != StateMachine.CombatState.Death && mState != StateMachine.MovementState.Idle){
            rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }
    }


    private void StateCheck(){
        FindClosestEnemy();
        switch(cState){
            case StateMachine.CombatState.Idle:
                if(defenceTarget == null){
                    cState = StateMachine.CombatState.Lookout;
                } else{
                    cState = StateMachine.CombatState.Defend;
                }

                break;

            case StateMachine.CombatState.Lookout:
                //TODO: Check if enemies is close
                //If no objects close, mState wander and break
                if(targetObject == null){
                    mState = StateMachine.MovementState.Wander;
                    break;
                }

                //Target object is now filtered to never be a friendly unit
                if(targetObject.CompareTag(cubeTag) || targetObject.CompareTag(sphereTag)){
                    cState = StateMachine.CombatState.Attack;
                } else if(targetObject.CompareTag(statueTag)){
                    mState = StateMachine.MovementState.Seek;
                    cState = StateMachine.CombatState.Capture;
                } else if(targetObject.CompareTag(destroyerTag)){
                    cState = StateMachine.CombatState.Lookout;
                    mState = StateMachine.MovementState.Flee;
                }

                break;

            case StateMachine.CombatState.Attack:
                //TODO: Check if enemy is dead, or im dead
                mState = StateMachine.MovementState.Seek;
                if(hpC.HP <= 0){
                    cState = StateMachine.CombatState.Death;
                    targetObject = null;
                } else if(targetObject == null){ //TODO: This might be a bug if the script is disabled
                    targetScript = null;
                    if(targetObject == null){
                        cState = StateMachine.CombatState.Idle;
                        break;
                    }
                }

                if(Vector3.Distance(transform.position, targetObject.transform.position) > (attackRange * 1.5f)){
                    targetObject = null;
                    cState = StateMachine.CombatState.Idle;
                    mState = StateMachine.MovementState.Wander;
                }

                break;
            case StateMachine.CombatState.Defend:
                //TODO: Check if enemy is close, then attack                
                if(targetObject != null && targetObject.CompareTag(destroyerTag)){
                    mState = StateMachine.MovementState.Interpose;
                    break;
                }

                if(statueScript.Team != team){
                    defenceTarget = null;
                    statueScript = null;
                    cState = StateMachine.CombatState.Idle;
                    break;
                }

                if(!defenceTarget.activeInHierarchy){
                    cState = StateMachine.CombatState.Lookout;
                    break;
                } else{
                    if(Vector3.Distance(transform.position, defenceTarget.transform.position) > defendRange){
                        mState = StateMachine.MovementState.Seek;
                    } else{
                        mState = StateMachine.MovementState.Wander;
                    }
                }

                break;

            case StateMachine.CombatState.Capture:
                if(targetObject == null){
                    cState = StateMachine.CombatState.Lookout;
                }

                break;
        }
    }

    private void StateAction(){
        switch(cState){
            case StateMachine.CombatState.Defend:

                break;

            case StateMachine.CombatState.Attack:
                if(Vector3.Distance(transform.position, targetObject.transform.position) > attackRange){
                    if(!AvoidObstacle())
                        RotateObject(targetObject.transform.position);
                } else{
                    if(targetObject == null){
                        cState = StateMachine.CombatState.Idle;
                        mState = StateMachine.MovementState.Wander;
                        break;
                    }

                    if(targetScript == null){
                        targetScript = targetObject.GetComponent<FSMController>();
                        if(targetScript == null){
                            targetObject = null;
                            //Debug.LogError("No target script found!");
                            return;
                        }
                    }

                    //Start attack animation if not running
                    targetScript.DealDamage(dps * Time.deltaTime);
                }

                break;

            case StateMachine.CombatState.Death:
                Collider c = GetComponent<BoxCollider>();
                if(c == null){
                    GetComponent<SphereCollider>().enabled = false;
                } else{
                    c.enabled = false;
                }

                if(defenceTarget != null){
                    defenceTarget.GetComponent<StatueScript>().ReduceDefenderCount();
                }

                isDead = true;
                SpawnController.DestroyObject(gameObject);
                break;

            case StateMachine.CombatState.Capture:

                //TODO: BUG: Still not working
                if(statueScript == null){
                    statueScript = targetObject.GetComponent<StatueScript>();
                    if(statueScript == null){
                        //Debug.LogError("No target script found!");
                        return;
                    }
                }

                if(statueScript.Team == team){
                    targetObject = null;
                    statueScript = null;
                    cState = StateMachine.CombatState.Idle;
                    mState = StateMachine.MovementState.Wander;
                    break;
                }

                if(Vector3.Distance(transform.position, targetObject.transform.position) > 3f){
                    RotateObject(targetObject.transform.position);

                    //Walk forward with fixed update
                } else{
                    statueScript.DoCapture(team, (int) hpC.MaxHp);
                    cState = StateMachine.CombatState.Death;
                }

                break;
        }

        switch(mState){
            case StateMachine.MovementState.Wander:
                //TODO: Check obstacle avoidance, then rotate else wander rotate
                if(!AvoidObstacle()){
                    if(!wanderActive)
                        break;
                    rotateDir = Wander.Direction(rotSpeed, rotateDir, 10);
                }

                break;
            case StateMachine.MovementState.Seek:
                //TODO: Check obstacle avoidance, then rotate or move along path?
                //TODO: Check if enemy is close, then attack or flee
                if(!AvoidObstacle()){
                    if(defenceTarget != null){
                        RotateObject(defenceTarget.transform.position);
                        break;
                    }

                    if(targetObject != null){
                        Vector3 disVector = targetObject.transform.position - transform.position;
                        if(HelperFunctions.VectorLength(disVector) < seekRange){
                            if(cState != StateMachine.CombatState.Capture)
                                RotateObject(disVector);
                        }
                    }
                }

                break;
            case StateMachine.MovementState.Flee:
                if(!AvoidObstacle()){
                    Vector3 disVector = (targetObject.transform.position - transform.position);
                    if(HelperFunctions.VectorLength(disVector) <= fleeDistance){
                        RotateObject(transform.position - disVector);
                    }
                }

                break;
            case StateMachine.MovementState.Interpose:
                if(targetObject == null){
                    mState = StateMachine.MovementState.Wander;
                    break;
                }

                if(defenceTarget != null){
                    Vector3 intercept = ((defenceTarget.transform.position - targetObject.transform.position) / 2) +
                                        targetObject.transform.position;
                    RotateObject(intercept);
                }

                break;
        }
    }

    private bool RotateObject(Vector3 targetRot){
        targetRot.y = 0.5f;
        transform.LookAt(targetRot);
        return true;
    }

    private void RotateObject(float rotation){
        transform.Rotate(0, rotation * Time.deltaTime * 10, 0);
    }

    private void FindClosestEnemy(){
        targetObject = null;
        if(objectsClose.Count == 0)
            return;
        foreach(GameObject obj in objectsClose){
            if(obj == null)
                continue;
            if(obj == gameObject)
                continue;
            float tmp = Vector3.Distance(transform.position, obj.transform.position);
            if(obj.CompareTag(destroyerTag)){
                targetObject = obj;
                break;
            }

            if(obj.CompareTag(cubeTag) && team == HelperFunctions.Team.Sphere ||
               obj.CompareTag(sphereTag) && team == HelperFunctions.Team.Cube){
                //TODO: Same if as statue, but with new script for units
                if(tmp < targetObjectDistance){
                    targetObject = obj;
                    targetObjectDistance = tmp;
                }
            } else if(obj.CompareTag(statueTag) && targetObject == null){
                StatueScript ss = obj.GetComponent<StatueScript>();
                if(team != ss.Team){
                    targetObject = obj;
                    statueScript = ss;
                }
            }
        }
    }


    //TODO: Maybe not needed?
    private bool AvoidObstacle(){
        if(cState == StateMachine.CombatState.Capture)
            return false;

        if(result.obstacleHit){
            rotateDir = result.rotDir;
            hitObstacle = true;
            return true;
        }

        if(hitObstacle){
            hitObstacle = false;
            //TODO: Maybe change values from 5 to variable? based on speed
            if(rotateDir > 0){
                rotateDir = 5;
            } else if(rotateDir < 0){
                rotateDir = -5;
            }

            return true;
        }

        return false;
    }

    public bool IsDead(){
        return isDead;
    }

    public void DealDamage(float dmg){
        hpC.DealDamage(dmg);
    }
}