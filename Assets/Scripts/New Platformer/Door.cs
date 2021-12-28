using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewPlatformer
{
    public class Door : MonoBehaviour
    {
        [Header("Parametrs")]
        [SerializeField]
        public Vector2 tpPosition;

        [Header("Components")]
        public Sprite doorOpen;
        public Sprite doorClose;
        private SpriteRenderer sr;
        
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }



        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Hero")
                if (doorOpen)
                    sr.sprite = doorOpen;
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag == "Hero")
                if (doorClose)
                    sr.sprite = doorClose;
        }
    }
}