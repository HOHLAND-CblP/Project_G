using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewPlatformer
{
    public class Hero : MonoBehaviour
    {
        [Header("Hero Parametrs")]
        [SerializeField]
        float maxSpeed;
        [SerializeField]
        GameObject notificationPanel;

        float speed;


        [Header("Hero Components")]
        [SerializeField]
        Animator animator;
        public string nameHero;
        public Sprite iconFace;
        int locationId;
        
        
        AnimationState animState;
        Door curDoor;
        NPC curNPC;


        [Header("UI Components")]
        [SerializeField]
        GameObject keyButton;
        [SerializeField]
        GameObject dialogButton;
        [SerializeField]
        GameObject dialogPanel;

        GameObject cam;
        Rigidbody2D rb;


        //Mods
        [HideInInspector]
        public bool dialogMode = false;


        bool facingLeft;

        
        enum AnimationState
        {
            idle = 0,
            walk = 1,
        }

        public int GetLocation()
        {
            return locationId;
        }


        void Start()
        {
            facingLeft = true;

            animState = 0;

            rb = GetComponent<Rigidbody2D>();
            cam = Camera.main.gameObject;
        }

        
        void Update()
        {
#if UNITY_EDITOR_WIN
            speed = Input.GetAxis("Horizontal");
#endif

            if (speed < 0 && !facingLeft)
                Flip();
            if (speed > 0 && facingLeft)
                Flip();

            if (speed != 0 && animState==AnimationState.idle)
            {
                animator.SetBool("Walk", true);
                animState = AnimationState.walk;
            }
            if (speed == 0 && animState == AnimationState.walk)
            {
                animator.SetBool("Walk", false);
                animState = AnimationState.idle;
            }
        }


        private void FixedUpdate()
        {
            transform.Translate(Vector2.right * speed * maxSpeed * Time.deltaTime);
        }


        //зеркалное отражение персонажа, взависимости от стороны в которую он двигается
        public void Flip()
        {
            facingLeft = !facingLeft; 
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public void GoRight()
        {
            speed = Mathf.Lerp(speed, 1, 1);
        }
        public void Stop()
        {
            speed = Mathf.Lerp(speed, 0, 1);
        }
        public void GoLeft()
        {
            speed = Mathf.Lerp(speed, -1, 1);
        }

        void Teleport()
        {
            transform.position = curDoor.tpPosition;
        }

        void Dialog()
        {
            dialogMode = true;
            cam.GetComponent<NovellaDialContoler>().NewDialog(curNPC.dialogText);
            //dialogPanel.GetComponent<NovellaDialog>().NewDialog(curNPC.dialogText);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Door")
            {
                curDoor = col.GetComponent<Door>();
                keyButton.SetActive(true);
                keyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                keyButton.GetComponent<Button>().onClick.AddListener(Teleport);
            }

            if (col.tag == "NPC")
            {
                curNPC = col.GetComponent<NPC>();
                dialogButton.SetActive(true);
                dialogButton.GetComponent<Button>().onClick.RemoveAllListeners();
                dialogButton.GetComponent<Button>().onClick.AddListener(Dialog);
            }

            if (col.tag == "Location")
            {
                locationId = col.GetComponent<Location>().GetLocationId();
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag == "Door" && curDoor.gameObject == col.gameObject)
            {
                curDoor = null;
                keyButton.SetActive(false);
            }

            if (col.tag == "NPC")
            {
                curNPC = null;
                dialogButton.SetActive(false);
            }
        }
    }
}