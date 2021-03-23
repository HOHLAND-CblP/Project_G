using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroControler : MonoBehaviour
{
    public int theLeft;

    public float maxSpeed = 25;
    public float moveH;
    public float check;

    public Transform groundCheck;
    public LayerMask whatIsGrounded;
    public float groundCircle;
    public bool grounded;
    public float satiety = 100, health = 100;
    public double cash = 1000;
    private int unHealthed = 0;

    private Rigidbody2D rb;
    public Animator anim;
    public GameObject cam;
    public bool facingRight, headAche = false;
    private bool hungry = true, healthed = false;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        facingRight = true;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Hungried());
    }

    private void FixedUpdate()
    {

        if (satiety <= 0)
        {
            hungry = false;
            StopCoroutine(Hungried());
        }

        if (health <= 0)
            healthed = false;

        if (satiety > 25 && satiety <= 50)
        {
            healthed = true;
            unHealthed = 5;
        }
        else if (satiety <= 25)
            unHealthed = 10;

        if (satiety > 50)
            healthed = false;

        Vector3 theScale = transform.localScale;

        if (theScale.x < 0)
            theLeft = -1;
        else
            theLeft = 1;

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCircle, whatIsGrounded);

        if (moveH > 0 || moveH < 0)
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);

        transform.Translate(maxSpeed * moveH * Time.deltaTime, 0, 0);

        if (facingRight && moveH < 0) //зеркалное отражение персонажа, взависимости от стороны в которую он двигается
            Flip();
        else if (moveH > 0 && !facingRight)
            Flip();
    }

    private void Update()
    {
        if (!GamePrefs.inDialog && Input.GetKeyDown(KeyCode.D))
            GoRight();

        if (!GamePrefs.inDialog && Input.GetKeyDown(KeyCode.A))
            GoLeft();

        if (!GamePrefs.inDialog && Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            Stop();

        if (!GamePrefs.inDialog && Input.GetKeyDown(KeyCode.E))
        {
            //if (cam.GetComponent<House>())
            //{
            //    if (cam.GetComponent<House>().nearFridge)
            //        cam.GetComponent<House>().OpenFridge();
            //    if (cam.GetComponent<House>().nearHealthBag)
            //        cam.GetComponent<House>().OpenHealthBag();
            //    if (cam.GetComponent<House>().nearBed && cam.GetComponent<House>().animBedButton.gameObject.activeSelf)
            //        cam.GetComponent<House>().GoBed();
            //}
            //if (cam.GetComponent<Station>() && cam.GetComponent<Station>().nearComp)
            //    cam.GetComponent<Station>().OpenComputer();
            //if (cam.GetComponent<Shop>() && cam.GetComponent<Shop>().nearCashier)
            //    cam.GetComponent<Shop>().OpenShop();
            //if (cam.GetComponent<Hospital>() && cam.GetComponent<Hospital>().nearDoctor)
            //    cam.GetComponent<Hospital>().OpenShop();
            //if (cam.GetComponent<TheMainMainScript>() && cam.GetComponent<Street>().nearStop)
            //    cam.GetComponent<TheMainMainScript>().OpenMap();
            //if (cam.GetComponent<TrackingTheHero>().nearDoorIn)
            //    cam.GetComponent<TrackingTheHero>().GoIn();
            //if (cam.GetComponent<TrackingTheHero>().nearDoorOut)
            //    cam.GetComponent<TrackingTheHero>().GoOut();
        }

        if (!GamePrefs.inDialog && Input.GetKeyDown(KeyCode.T))
            cam.GetComponent<TrackingTheHero>().phone.gameObject.GetComponent<Phone>().OpenPhone();

    }

    public void Flip()
    {
        facingRight = !facingRight; //зеркалное отражение персонажа, взависимости от стороны в которую он двигается
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void GoRight()
    {
        moveH = Mathf.Lerp(moveH, 1, 1);
    }

    public void Stop()
    {
        moveH = Mathf.Lerp(moveH, 0, 1);
    }

    public void GoLeft()
    {
        moveH = Mathf.Lerp(moveH, -1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<ObjectProperties>().Activation();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<ObjectProperties>().Diactivation();
    }

    IEnumerator Hungried()
    {
        while (hungry)
        {
            yield return new WaitForSeconds(60);
            satiety -= 1;
            if (healthed) health -= unHealthed;
        }
    }
}
