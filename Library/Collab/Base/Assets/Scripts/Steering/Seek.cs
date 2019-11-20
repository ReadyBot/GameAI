using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    float Radius(float radius)
    {
        return radius;
    }

    void Start()
    {
        //TODO declare input radius when called.
    }

    // Update is called once per frame
    void Update()
    {
        //TODO Make timer and call upon collider ever 2s(?)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
    }
}
