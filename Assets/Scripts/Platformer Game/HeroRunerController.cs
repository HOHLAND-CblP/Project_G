using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRunerController : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce;
    private Rigidbody2D rb;
    private GameObject cam;
    private Animator anim;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("run", true);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Jump();
        if (Input.GetKeyDown(KeyCode.S)) Slide();
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Slide()
    {
        anim.SetBool("slide", true);
    }
}
