using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewPlatformer
{
    public class Location : MonoBehaviour
    {
        [Header("Parametrs")]
        [SerializeField]
        int id;
        
        public int GetLocationId()
        {
            return id;
        }
        
    }
}