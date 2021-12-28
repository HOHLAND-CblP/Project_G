using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    // Components
    GameObject cam;
    GameObject canvas;


    string res;

    int levelModif;
    int countResInTime;
    [SerializeField]
    int countRes = 0;

    bool mouseButPress = false;

    [Header("Modification Panel Prefab")]
    public GameObject modPanel;
    GameObject curModifPanel;

    [Header("Star Prefab")]
    [SerializeField]
    GameObject starPref;
    List<GameObject> stars;


    private void Start()
    {
        cam = Camera.main.gameObject;
        canvas = cam.GetComponent<GameControler>().GetCanvas();

        levelModif = 1;
        countResInTime = 1;

        stars = new List<GameObject>();
        enabled = false;
    }

    public void BuildFactory(string res)
    {
        this.res = res;
        StartCoroutine(AddResources());

        switch(res)
        {
            case "Wood":
                GetComponent<SpriteRenderer>().color = new Color32(215, 151, 101, 255);
                break;

            case "Stone":
                GetComponent<SpriteRenderer>().color = new Color32(137, 164, 166, 255);
                break;

            case "Iron":
                GetComponent<SpriteRenderer>().color = new Color32(173, 168, 168, 255);
                break;
        }
    }
    public void BuildFactory()
    {
        res = cam.GetComponent<BuildingsGrid>().GetResourceFromRes((int)transform.position.x, (int)transform.position.y).name;
        StartCoroutine(AddResources());

        switch (res)
        {
            case "Wood":
                GetComponent<SpriteRenderer>().color = new Color32(215, 151, 101, 255);
                break;

            case "Stone":
                GetComponent<SpriteRenderer>().color = new Color32(137, 164, 166, 255);
                break;

            case "Iron":
                GetComponent<SpriteRenderer>().color = new Color32(173, 168, 168, 255);
                break;
        }
    }


    public string TypeOfRes()
    {
        return res;
    }

    public int TakeRespurce(int countOfResTaken)
    {
        if (countRes<countOfResTaken)
        {
            int temp = countRes;
            countRes = 0;
            return temp;
        }
        else
        {
            countRes -= countOfResTaken;
            return countOfResTaken;
        }
    }


    public void ClickOnCell()
    {
        mouseButPress = false;

        if (curModifPanel == null)
            if (levelModif < 3)
            {
                curModifPanel = Instantiate(modPanel, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity, canvas.transform);
                curModifPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Modification);
                enabled = true;
            }
            else
            {
                cam.GetComponent<GameControler>().notificationPanel.GetComponent<TrainNotification>().NewNotice("Достигнуто максимальное улучшение.");
            }
    }


    void Modification()
    {
        switch (res)
        {
            case "Wood":
                if (PlayerPrefs.GetInt("StoneCount") >= 15 * levelModif)
                {
                    PlayerPrefs.SetInt("StoneCount", PlayerPrefs.GetInt("StoneCount") - 15 * levelModif);
                    Modification_();
                }
                else
                {
                    cam.GetComponent<GameControler>().notificationPanel.GetComponent<TrainNotification>().NewNotice("Улучшение не произошло. Нехватает ресурсов.");
                }
                break;

            case "Stone":
                if (PlayerPrefs.GetInt("WoodCount") >= 15 * levelModif)
                {
                    PlayerPrefs.SetInt("WoodCount", PlayerPrefs.GetInt("WoodCount") - 15 * levelModif);
                    Modification_();
                }
                else
                    cam.GetComponent<GameControler>().notificationPanel.GetComponent<TrainNotification>().NewNotice("Улучшение не произошло. Нехватает ресурсов.");
                break;

            case "Iron":
                if (PlayerPrefs.GetInt("WoodCount") >= 10 * levelModif && PlayerPrefs.GetInt("StoneCount") >= 10 * levelModif)
                {
                    PlayerPrefs.SetInt("WoodCount", PlayerPrefs.GetInt("WoodCount") - 10 * levelModif);
                    PlayerPrefs.SetInt("StoneCount", PlayerPrefs.GetInt("StoneCount") - 10 * levelModif);
                    Modification_();
                }
                else
                    cam.GetComponent<GameControler>().notificationPanel.GetComponent<TrainNotification>().NewNotice("Улучшение не произошло. Нехватает ресурсов.");
                break;
        }

        Destroy(curModifPanel.gameObject);
        enabled = false;  
    }

    void Modification_()
    {
        levelModif++;

        switch (levelModif)
        {
            case 2:
                stars.Add(Instantiate(starPref, new Vector3(transform.position.x - 0.37f, transform.position.y + 0.378f, 0), Quaternion.identity, transform));
                countResInTime = 2;
                break;
            case 3:
                stars.Add(Instantiate(starPref, new Vector3(transform.position.x - 0.12f, transform.position.y + 0.378f, 0), Quaternion.identity, transform));
                countResInTime = 3;
                break;
        }

        Debug.Log("Factory Is Update");
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            if (mouseButPress)
            {
                mouseButPress = false;
                Destroy(curModifPanel.gameObject);
                enabled = false;
            }


        if (Input.GetMouseButtonDown(0))
        {
            mouseButPress = true;
        }


        curModifPanel.transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
    }


    IEnumerator AddResources()
    {
        yield return new WaitForSeconds(2);

        /*switch (res)
        {
            case Resource.TypesOfResource.Wood:
                PlayerPrefs.SetInt("WoodCount", PlayerPrefs.GetInt("WoodCount") + countResInTime);
                break;

            case Resource.TypesOfResource.Stone:
                PlayerPrefs.SetInt("StoneCount", PlayerPrefs.GetInt("StoneCount") + countResInTime);
                break;

            case Resource.TypesOfResource.Iron:
                PlayerPrefs.SetInt("IronCount", PlayerPrefs.GetInt("IronCount") + countResInTime);
                break;
        }*/
        countRes += countResInTime;

        StartCoroutine(AddResources());
    }
}