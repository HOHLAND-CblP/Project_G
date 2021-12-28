using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewPlatformer
{
    public class TrackingTheHero : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        GameObject hero;
    


        void Start()
        {
            
        }


        void Update()
        {
            transform.position = new Vector3(hero.transform.position.x, hero.transform.position.y+2, transform.position.z);
        }
    }
}
