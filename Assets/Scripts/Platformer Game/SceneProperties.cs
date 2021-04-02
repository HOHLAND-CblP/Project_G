using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProperties : MonoBehaviour
{
    public string title;
    public int sceneId;
    public int countOfDialogs;
    public TextAsset[] dialogs;

    [Header ("Objects")]
    public GameObject teleport;
    public GameObject doorIn;
    public GameObject doorOut;
    public GameObject busStop;
    public GameObject computer;
    public GameObject safeObject;
    public GameObject cash;
    public GameObject registry;
    public GameObject bar;
    public GameObject fridge;
    public GameObject bed;
    public GameObject healthBag;

    [Header ("Edge of camera translate")]
    public float leftV;
    public float rightV;
    public float groundX;
    public float groundY;

    private void Start()
    {
        countOfDialogs = GamePrefs.countsOfcountOfDialogs[sceneId];
    }
}