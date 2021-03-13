using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class RespawnMode : MonoBehaviour
{
    [SerializeField]
    int maxTrain;
    int curTrain;

    public List<Color> colorsTrain;

    public GameObject dirStationButPref;    // префаб стрелочек
    public GameObject canvas;               // канвас для задания родителя для стрелочек
    GameObject dirStationBut_1;             // первая стрелочка
    GameObject dirStationBut_2;             // вторая стрелочка
    GameObject curRespawnStation;           //текущая выбраная станция 

    [Space] [Space]

    public GameObject TrainPref; //префаб поезда

    List<GameObject> stations = new List<GameObject>();


    void Start()
    {
        curTrain = 0;

        dirStationBut_1 = null;
        dirStationBut_2 = null;
        curRespawnStation = null;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // при нажатие на ЛКМ
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //берем координаты курсора
            GameObject cell = GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
 
            if (curTrain < maxTrain && stations.Count>1)
            {
                if (cell && cell.GetComponent<Station>())
                {
                    if (cell != curRespawnStation)
                        if (curRespawnStation != null) //если станция выбрана
                        {
                            curRespawnStation = cell;                   // Записываем в текущую станцию ссылку на станцию, в которую попал луч
                            Destroy(dirStationBut_1.gameObject);        // Удаляем стрелочки для предыдущей станции
                            if (dirStationBut_2.gameObject != null)     // Так как не все станции имеют два выезда, то не всегда стрелочек две штуки
                                Destroy(dirStationBut_2.gameObject);    // Для того чтобы избежать ошибок удаляется одна стрелочка
                            DirButtonInstantiate();                     // Создаём стрелочеки
                        }
                        else    // если станция не выбрана
                        {
                            curRespawnStation = cell;       //записываем в текущую станцию ссылку на станцию, в которую попал луч
                            DirButtonInstantiate();         //создаем стрелочеки
                        }
                }
            }
        }

        if (dirStationBut_1)
            dirStationBut_1.transform.position = curRespawnStation.transform.position;
        if (dirStationBut_2)
            dirStationBut_2.transform.position = curRespawnStation.transform.position;
    }


    public void RandomRespawn()
    {
        if (curTrain < maxTrain && stations.Count > 1)
        {
            curRespawnStation = stations[Random.Range(0, stations.Count - 1)];

            Quaternion ang = Quaternion.Euler(curRespawnStation.GetComponent<Station>().GetAngles());   //получаем угол ж/д на которой находиться станция

            int rand;

            if (ang == Quaternion.Euler(0, 0, 0)) //выставляем стрелочкам выполненение нужных функций
            {
                rand = Random.Range(0, 1);

                switch (rand)
                {
                    case 0:
                        RightArrow();
                        break;

                    case 1:
                        LeftArrow();
                        break;
                }
            }
            else if (ang == Quaternion.Euler(0, 0, 45))
            {
                rand = Random.Range(0, 1);

                switch (rand)
                {
                    case 0:
                        UpRightArrow();
                        break;

                    case 1:
                        DownLeftArrow();
                        break;
                }
            }
            else if (ang == Quaternion.Euler(0, 0, 90))
            {
                rand = Random.Range(0, 1);

                switch (rand)
                {
                    case 0:
                        UpArrow();
                        break;

                    case 1:
                        DownArrow();
                        break;
                }
            }
            else if (ang == Quaternion.Euler(0, 0, 135))
            {
                rand = Random.Range(0, 1);

                switch (rand)
                {
                    case 0:
                        UpLeftArrow();
                        break;

                    case 1:
                        DownRightArrow();
                        break;
                }
            }


            GetComponent<GameControler>().ActDeactRespawnTrainMode();    // Костыль
        }
    }


    void DirButtonInstantiate()     //функция создания стрелочек для выбора направления движения
    {
        Quaternion ang = Quaternion.Euler(curRespawnStation.GetComponent<Station>().GetAngles());   //получаем угол ж/д на которой находиться станция

        dirStationBut_1 = Instantiate(dirStationButPref, curRespawnStation.transform.position, ang, canvas.transform); // создаем стрелочки

        dirStationBut_2 = Instantiate(dirStationButPref, curRespawnStation.transform.position, ang, canvas.transform);
        dirStationBut_2.transform.localScale = new Vector3(-dirStationBut_2.transform.localScale.x, dirStationBut_2.transform.localScale.y, dirStationBut_2.transform.localScale.z);


        if (ang == Quaternion.Euler(0, 0, 0)) //выставляем стрелочкам выполненение нужных функций
        {
            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(RightArrow);
            dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LeftArrow);
        }
        else if (ang == Quaternion.Euler(0, 0, 45))
        {
            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpRightArrow);
            dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownLeftArrow);
        }
        else if (ang == Quaternion.Euler(0, 0, 90))
        {
            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpArrow);
            dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownArrow);
        }
        else if (ang == Quaternion.Euler(0, 0, 135))
        {
            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpLeftArrow);
            dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownRightArrow);
        }
    }


    void LeftArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x - 0.501f, curRespawnStation.transform.position.y);

        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void RightArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x + 0.501f, curRespawnStation.transform.position.y);

        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void UpArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x, curRespawnStation.transform.position.y + 0.501f);


        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void DownArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x, curRespawnStation.transform.position.y - 0.501f);

        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }

    void UpRightArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x + 0.501f, curRespawnStation.transform.position.y + 0.501f);

        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void DownLeftArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x - 0.501f, curRespawnStation.transform.position.y - 0.501f);

        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void UpLeftArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x - 0.501f, curRespawnStation.transform.position.y + 0.501f);

        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void DownRightArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curRespawnStation.transform.position.x + 0.501f, curRespawnStation.transform.position.y - 0.501f);

        RespawnTrain(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode(); ;
    }


    public void DiactivateRespMode()
    {
        curRespawnStation = null;
        if (dirStationBut_1 != null)
            Destroy(dirStationBut_1.gameObject);
        if (dirStationBut_2 != null)
            Destroy(dirStationBut_2.gameObject);
    }


    void RespawnTrain(Vector2[] points)
    {
        curTrain++;

        GameObject train = Instantiate(TrainPref, curRespawnStation.transform.position, Quaternion.identity);
        train.SetActive(false);

        stations.Remove(curRespawnStation);

        int rand = Random.Range(0, stations.Count-1);
        int rand_color = Random.Range(0, colorsTrain.Count);

        stations.Add(curRespawnStation);

        stations[rand].GetComponent<Station>().SetColor(colorsTrain[rand_color]);

        train.GetComponent<TrainScript>().CreateTrain(colorsTrain[rand_color], gameObject, points, stations[rand], 3);

        stations.RemoveAt(rand);
        colorsTrain.RemoveAt(rand_color);

        train.SetActive(true);
    }


    public void AddStation (GameObject station)
    {
        stations.Add(station);
    }


    public void ArrivedAtStation(GameObject station, Color color)
    {
        curTrain--;
        stations.Add(station);
        colorsTrain.Add(color);
    }

    public void DeleteStation(GameObject station)
    {
        stations.Remove(station);
    }
}