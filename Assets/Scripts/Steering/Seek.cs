using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    public GameObject target;
    private readonly float timeSpeed = 10f;

    float Radius(float radius)
    {
        return radius;
    }
    
    public void SeekObject(GameObject target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, timeSpeed * Time.deltaTime);
        transform.LookAt(target.transform);
    }
}
