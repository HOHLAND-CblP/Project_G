using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{
    public int type;
    public GameObject[] nextLevels;
    public GameObject button;
    public GameObject darkSide;
    GameObject cam;
    public bool isNeedButton;
    public bool isNeedAnimation;
    [Header("itemsPref")]
    public GameObject[] itemFridgePref, itemHealthPref; 
    public GameObject emptyFridgeMessage, emptyHealthBagMessage;


    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void Activation()
    {
        if (!GamePrefs.inDialog)
        {
            cam.GetComponent<TheMainMainScript>().darkSide = darkSide;
            if (type == 6) cam.GetComponent<TheMainMainScript>().currentLevelTemp = nextLevels[0];
            if (isNeedButton)
            {
                button.SetActive(true);
                if (GamePrefs.countOfPlots == 1 && GamePrefs.countOfPlotMoment == 18 && GamePrefs.countOfHint == 1)
                {
                    cam.GetComponent<TheMainMainScript>().OpenHint("Нажмите на кнопку, чтобы посмотреть предмет", 180);
                }
            }
            else if (type == 3 || type == 4)
            {

                GamePrefs.countsOfcountOfDialogs[cam.GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId] = 
                    cam.GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().countOfDialogs;
                cam.GetComponent<TheMainMainScript>().currentLevel = nextLevels[0];
                cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
                GamePrefs.inout = type;
            }
            if (GamePrefs.countOfPlots == 1 && GamePrefs.countOfPlotMoment == 22 && GamePrefs.countOfHint == 7)
            {
                cam.GetComponent<TheMainMainScript>().StartDialog();
            }
        }
        if (isNeedAnimation)
        {
            GetComponent<Animator>().SetBool("Open", true);
        }
    }

    public void Diactivation()
    {
        if (isNeedButton)
        {
            button.SetActive(false);
        }
        if (isNeedAnimation)
        {
            GetComponent<Animator>().SetBool("Open", false);
        }
    }

    public void AddFridge(int id)
    {
        if (GamePrefs.amountOfFood == 0)
        {
            Destroy(cam.GetComponent<TheMainMainScript>().fridge.transform.GetChild(0).gameObject);
        }
        GameObject temp = Instantiate(itemFridgePref[id]);
        temp.transform.SetParent(cam.GetComponent<TheMainMainScript>().fridge.transform);
        temp.transform.localScale = new Vector3(1, 1, 1);
    }

    public void AddHealthBag(int id)
    {
        if (GamePrefs.amountOfHealth == 0)
        {
            Destroy(cam.GetComponent<TheMainMainScript>().healthBag.transform.GetChild(0).gameObject);
        }
        GameObject temp = Instantiate(itemHealthPref[id]);
        temp.transform.SetParent(cam.GetComponent<TheMainMainScript>().healthBag.transform);
        temp.transform.localScale = new Vector3(1, 1, 1);
    }
}
