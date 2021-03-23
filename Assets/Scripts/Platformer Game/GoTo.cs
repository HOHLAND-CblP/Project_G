using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoTo : MonoBehaviour
{
    public GameObject destination;
    public float speed;

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination.transform.position, speed * Time.deltaTime);
        if (Math.Abs(transform.position.x - destination.transform.position.x) < 1) 
            Destroy(gameObject);
    }
}
