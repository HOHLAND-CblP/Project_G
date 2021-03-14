using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Сrutch : MonoBehaviour
{
    GameObject cam;
    [SerializeField]
    GameObject silhouette1, silhouette2, monster, man, layer2, blackScreen, generalCabinet, blackScreen2, house;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    public void StartDialog()
    {
        cam.GetComponent<TheMainMainScript>().StartDialog();
    }

    public void Prolog1()
    {
        silhouette1.SetActive(false);
        silhouette2.SetActive(false);
        monster.SetActive(true);
    }

    public void Prolog2()
    {
        layer2.GetComponent<Animator>().SetBool("anim4", true);
        blackScreen.GetComponent<Animator>().SetBool("anim4", true);
    }

    public void Prolog3()
    {
        monster.GetComponent<SpriteRenderer>().sortingOrder = man.GetComponent<SpriteRenderer>().sortingOrder;
        monster.GetComponent<Animator>().enabled = true;
    }

    public void Prolog4()
    {
        cam.GetComponent<TheMainMainScript>().currentLevel = generalCabinet;
        cam.transform.position = new Vector3(cam.GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV 
            + (cam.GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV - cam.GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV) / 2, 
            cam.GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY+3, cam.transform.position.z);
        blackScreen2.GetComponent<Animator>().enabled = true;
    }

    public void Prolog5()
    {
        blackScreen2.GetComponent<SpriteRenderer>().enabled = true;
        blackScreen2.GetComponent<Animator>().SetBool("endOfPlot", true);
    }

    public void Prolog6()
    {
        cam.GetComponent<TheMainMainScript>().currentLevel = house;
        cam.GetComponent<TrackingTheHero>().enabled = true;
        cam.GetComponent<TheMainMainScript>().left.SetActive(true);
        cam.GetComponent<TheMainMainScript>().right.SetActive(true);
        cam.GetComponent<TheMainMainScript>().phoneButton.SetActive(true);
        cam.GetComponent<TheMainMainScript>().pauseButton.SetActive(true);
    }
}
