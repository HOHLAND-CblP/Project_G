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
            if (type==6) cam.GetComponent<TheMainMainScript>().currentLevelTemp = nextLevels[0];
            if (isNeedButton)
            {
                button.SetActive(true);
            }
            else
            {
                GamePrefs.currentLevel = nextLevels[0];
                cam.GetComponent<TrackingTheHero>().faded.SetActive(true);
                GamePrefs.inout = type;
            }
        }
    }

    public void Diactivation()
    {
        if (isNeedButton)
        {
            button.SetActive(false);
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
