using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameControler : MonoBehaviour
{
    bool respawnTrainMode;          // режим респавна поездов
    public GameObject RespawnUI;    // UI объекты для режима респавна

    public LayerMask needLayerMask;

    [Space(20)]

    [SerializeField]
    bool buildingMode;
    public GameObject buildingUI;
    public GameObject buildingModeButton;
    public GameObject respawnModeButton;


    [Space(20)]
    public GameObject speedPanel;
    GameObject curTrain;


    [Space(20)]

    public bool zooming;
    public float zoomSpeed;


    List<GameObject> buildings = new List<GameObject>();

    


    private void Awake()
    {
        //Application.targetFrameRate = 30;
    }


    void Start()
    {
        GetComponent<BuildingsGrid>().enabled = false;
        GetComponent<RespawnMode>().enabled = false;
        GetComponent<RailwayControler>().enabled = false;
        buildingUI.SetActive(false);
        buildingMode = false;
        respawnTrainMode = false;

        foreach(var b in buildings)
        {
            GetComponent<BuildingsGrid>().FillGrid(b.transform.position.x, b.transform.position.y, b);
        }
        buildings = null;

        GetComponent<RailwayControler>().StartConect();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject build = GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            if (build!=null)
            {
                build.GetComponent<Building>().ClickOnTheCell();
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
            ActDeactBuildingMode();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }



    public void DiactivateAllMods()     // Диактивация всех модов
    {
        if (buildingMode)
            ActDeactBuildingMode();

        if (respawnTrainMode)
            ActDeactRespawnTrainMode();
    }

    public void ActDeactBuildingMode() //активация и деактивация режима строительства
    {
        if (!buildingMode)
        {
            DiactivateAllMods();
            GetComponent<BuildingsGrid>().enabled = true;
            buildingMode = true;
            buildingUI.SetActive(true);
            buildingModeButton.SetActive(false);
            respawnModeButton.SetActive(false);
        }
        else
        {
            GetComponent<BuildingsGrid>().DeactBuildingMode();
            buildingMode = false;
            buildingUI.SetActive(false);
            buildingModeButton.SetActive(true);
            respawnModeButton.SetActive(true);
        }
    }

    public void ActDeactRespawnTrainMode() //активация и деактивация режима респавна поездов
    {
        if (!respawnTrainMode)
        {
            DiactivateAllMods();
            respawnTrainMode = true;
            respawnModeButton.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            GetComponent<RespawnMode>().enabled = true;
            //buildingModeButton.SetActive(false);
        }
        else
        {
            respawnTrainMode = false;
            respawnModeButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
            GetComponent<RespawnMode>().DiactivateRespMode();
            GetComponent<RespawnMode>().enabled = false;
            //buildingModeButton.SetActive(true);
        }
    }



    public void AddBuildings(GameObject build)
    {
        if (buildings != null)
            buildings.Add(build);
    }



    public void ActivateSpeedPanel(GameObject train)
    {
        speedPanel.SetActive(true);
        curTrain = train;
        speedPanel.GetComponent<Image>().color = curTrain.GetComponent<SpriteRenderer>().color;
    }

    public void DeactSpeedPanel(GameObject train)
    {
        if (train == curTrain)
            speedPanel.SetActive(false);
    }

    public void SpeedChange(float speed)
    {
        if (curTrain != null)   
            curTrain.GetComponent<TrainScript>().SpeedChange(speed);
    }
}