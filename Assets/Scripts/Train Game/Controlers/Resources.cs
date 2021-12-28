using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Resources : MonoBehaviour
{
    [Header("Type Of Resource")]
    public TypesOfResource typeOfResource;

    GameObject cam;


    public enum TypesOfResource
    { 
        None=0,
        Wood=1,
        Stone=2, 
        Iron = 3
    }
    
    void Start()
    {
        cam = Camera.main.gameObject;

        //cam.GetComponent<BuildingsGrid>().FiilRes((int)transform.position.x, (int)transform.position.y, this);
    }   
}