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
    // Режимы
    bool respawnTrainMode;  // Режим респавна поездов
    bool buildingMode;      // Режим строительства    

    // UI элементы респавна
    public GameObject RespawnUI;    // Кнопка респавна


    // UI элементы строительства
    [Space(20)]
    public GameObject buildingUI;           // Кнопка запуска режима строительства
    public GameObject buildingModeButton;   // Объект объединяющий кнопки для строительства


    // Переключение скоростей поездов
    [Space(20)]
    public GameObject speedPanel;   // Панель скоростей поездов (UI-элемент)
    GameObject curTrain;            // Текущий выбранный поезд

    // Сообщение об конце анимации
    [HideInInspector]
    public AnimationEnded animEnd;

    // Блокировка панели скоростей из вне
    public bool blockSpeedPanel;

    // Список зданий находяшихся в начале сцены
    List<GameObject> buildings = new List<GameObject>();    // Был создан из-за незнания возможности настройки порядкового запуска скриптов
                                                            // Каждый Building на сцене в Awake отправляет информаию в этот список, 
                                                            // а затем добавляется в сетку (массив Grid)
   


    private void Awake()
    {
        Application.targetFrameRate = 300;
    }


    void Start()
    {
        GetComponent<BuildingsGrid>().enabled = false;      // Выключаем управляющий скрипт режимом строительства
        GetComponent<RespawnMode>().enabled = false;        // Выключаем управляющий скрипт режимом респавна
        GetComponent<RailwayControler>().enabled = false;   // Выключаем управляющий скрипт жд дорогой

        buildingUI.SetActive(false);    // Ставим в инвиз UI-элементы строителства

        buildingMode = false;           // Отключаем режимы 
        respawnTrainMode = false;

        foreach(var b in buildings) // Добавляем объекты из списка building в сетку (массив Grid)
        {
            GetComponent<BuildingsGrid>().FillGrid(b.transform.position.x, b.transform.position.y, b);
        }
        buildings = null;

        GetComponent<RailwayControler>().StartConect(); // Запускаем функцию соединения станций (визуально)
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))        // Обработка нажатия на мышку
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject build = GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            if (build!=null)
            {
                build.GetComponent<Building>().ClickOnTheCell();
            }
        }

        if (Input.GetKeyDown(KeyCode.B))        // Активация/деактивация режима строительства
            ActDeactBuildingMode();

        
        if (Input.GetKeyDown(KeyCode.Escape))   // Выход в меню
        {
            SceneManager.LoadScene(0);
        }
    }


    // Диактивация всех модов
    public void DiactivateAllMods()     
    {
        if (buildingMode)
            ActDeactBuildingMode();

        if (respawnTrainMode)
            ActDeactRespawnTrainMode();
    }

    // Активация и деактивация режима строительства
    public void ActDeactBuildingMode() 
    {
        if (!buildingMode)
        {
            DiactivateAllMods();
            GetComponent<BuildingsGrid>().enabled = true;
            buildingMode = true;
            buildingUI.SetActive(true);
            buildingModeButton.SetActive(false);
            RespawnUI.SetActive(false);
        }
        else
        {
            GetComponent<BuildingsGrid>().DeactBuildingMode();
            buildingMode = false;
            buildingUI.SetActive(false);
            buildingModeButton.SetActive(true);
            RespawnUI.SetActive(true);
        }
    }

    // Активация и деактивация режима респавна поездов
    public void ActDeactRespawnTrainMode() 
    {
        if (!respawnTrainMode)
        {
            DiactivateAllMods();
            respawnTrainMode = true;
            RespawnUI.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            GetComponent<RespawnMode>().enabled = true;
            //buildingModeButton.SetActive(false);
        }
        else
        {
            respawnTrainMode = false;
            RespawnUI.transform.GetChild(0).GetComponent<Text>().color = Color.black;
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
        if (!blockSpeedPanel)
        {
            speedPanel.SetActive(true);
            curTrain = train;
            speedPanel.GetComponent<Image>().color = curTrain.GetComponent<SpriteRenderer>().color;
        }
    }

    public void DeactSpeedPanel(GameObject train) // Нужна чтобы убирать панельку при исчезновении поезда со сцены (уничтожение/доехал до станции)
    {
        if (train == curTrain)
            speedPanel.SetActive(false);
    }

    public void SpeedChange(float speed)
    {
        if (curTrain != null)   
            curTrain.GetComponent<TrainScript>().SpeedChange(speed);
    }

    public void AnimationEnded()
    {
        animEnd();
    }
}