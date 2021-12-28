using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class RespawnMode : MonoBehaviour
{
    [SerializeField]
    int maxTrain;
    [SerializeField]
    int curTrain;

    [Header ("Colors")]
    public List<Color> colorsTrain;

    public MiniMission miniMission; // Ссылка на объект с MiniMission

    public GameObject dirStationButPref;    // префаб стрелочек
    public GameObject canvas;               // канвас для задания родителя для стрелочек
    GameObject dirStationBut_1;             // первая стрелочка
    GameObject dirStationBut_2;             // вторая стрелочка
    GameObject curStation;           // текущая выбраная станция 

    [Space] [Space]

    public GameObject TrainPref; // префаб поезда

    List<GameObject> stations = new List<GameObject>();


    void Start()
    {
        curTrain = 0;

        dirStationBut_1 = null;
        dirStationBut_2 = null;
        curStation = null;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // При нажатие на ЛКМ
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // Получаем координаты курсора

            // Получаем клетку из сетки строительства по клетке, на которую нажали
            GameObject cell = GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y)); 
            
            // Если на карте еще достаточно поездов для респавна и станций больше 1
            if (curTrain < maxTrain /*&& stations.Count>1*/)
            {
                // Если был получена клетка из сетки строительства, и на ней станция
                if (cell && cell.GetComponent<Station>())
                {
                    if (cell != curStation)
                        if (curStation != null) //если станция выбрана
                        {
                            // Записываем в curStation ссылку на станцию, на которую нажали
                            curStation = cell;                   

                            // Удаление UI стрелочек ------------------
                            if (dirStationBut_1)                        // Проверяем наличие первой UI стрелочки
                                Destroy(dirStationBut_1.gameObject);   
                            if (dirStationBut_2)                        // Провереям наличие второй UI стрелочки
                                Destroy(dirStationBut_2.gameObject);   
                            //-----------------------------------------

                            // Создание новых UI стрелочек
                            DirButtonInstantiate();                     // Создаём стрелочеки
                        }
                        else    // если станция не выбрана
                        {  
                            curStation = cell;       //записываем в текущую станцию ссылку на станцию, в которую попал луч
                            DirButtonInstantiate();         //создаем стрелочеки
                        }
                }
            }
        }

        if (dirStationBut_1)
            dirStationBut_1.transform.position = curStation.transform.position;
        if (dirStationBut_2)
            dirStationBut_2.transform.position = curStation.transform.position;
    }


    public void RandomRespawn()
    {
        if (curTrain < maxTrain && stations.Count > 1)
        {
            curStation = stations[Random.Range(0, stations.Count - 1)];

            Quaternion ang = Quaternion.Euler(curStation.GetComponent<Station>().GetAngles());   //получаем угол ж/д на которой находиться станция

            int rand = Random.Range(0, 2);


            if (ang == Quaternion.Euler(0, 0, 0)) //выставляем стрелочкам выполненение нужных функций
            {
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


    // Функция создания стрелочек для выбора направления движения
    void DirButtonInstantiate()
    {
        Quaternion ang;
        if (curStation.GetComponent<Station>().endStation)
        {
            Vector2 dir = curStation.GetComponent<RailwayScript>().GetConect(0);


            switch (dir.x)
            {
                case -1:
                    ang = Quaternion.Euler(0, 0, 180 - 45 * dir.y);

                    switch (dir.y)
                    {
                        case -1:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownLeftArrow);
                            break;
                        case 0:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LeftArrow);
                            break;
                        case 1:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpLeftArrow);
                            break;
                    }
                    break;

                case 0:
                    ang = Quaternion.Euler(0, 0, 180 - 90 * dir.y);
                    
                    switch (dir.y)
                    {
                        case -1:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownArrow);
                            break;
                        case 1:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpArrow);
                            break;
                    }
                    break;

                case 1:
                    ang = Quaternion.Euler(0, 0, 0 + 45 * dir.y);
                    
                    switch (dir.y)
                    {
                        case -1:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownRightArrow);
                            break;
                        case 0:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(RightArrow);
                            break;
                        case 1:
                            dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелку
                            dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpRightArrow);
                            break;
                    }
                    break;
            }
        }
        else
        {
            ang = Quaternion.Euler(curStation.GetComponent<Station>().GetAngles());   //получаем угол ж/д на которой находиться станция

            

            if (ang == Quaternion.Euler(0, 0, 0)) //выставляем стрелочкам выполненение нужных функций
            {
                dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелочки
                dirStationBut_2 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform);
                dirStationBut_2.transform.localScale = new Vector3(-dirStationBut_2.transform.localScale.x, dirStationBut_2.transform.localScale.y, dirStationBut_2.transform.localScale.z);

                dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(RightArrow);
                dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LeftArrow);
            }
            else if (ang == Quaternion.Euler(0, 0, 45))
            {
                dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелочки
                dirStationBut_2 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform);
                dirStationBut_2.transform.localScale = new Vector3(-dirStationBut_2.transform.localScale.x, dirStationBut_2.transform.localScale.y, dirStationBut_2.transform.localScale.z);


                dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpRightArrow);
                dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownLeftArrow);
            }
            else if (ang == Quaternion.Euler(0, 0, 90))
            {
                dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелочки
                dirStationBut_2 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform);
                dirStationBut_2.transform.localScale = new Vector3(-dirStationBut_2.transform.localScale.x, dirStationBut_2.transform.localScale.y, dirStationBut_2.transform.localScale.z);

                dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpArrow);
                dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownArrow);
            }
            else if (ang == Quaternion.Euler(0, 0, 135))
            {
                dirStationBut_1 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform); // Создаем стрелочки
                dirStationBut_2 = Instantiate(dirStationButPref, curStation.transform.position, ang, canvas.transform);
                dirStationBut_2.transform.localScale = new Vector3(-dirStationBut_2.transform.localScale.x, dirStationBut_2.transform.localScale.y, dirStationBut_2.transform.localScale.z);

                dirStationBut_1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(UpLeftArrow);
                dirStationBut_2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DownRightArrow);
            }
        }
    }


    void LeftArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x - 0.501f, curStation.transform.position.y);

        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void RightArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x + 0.501f, curStation.transform.position.y);

        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void UpArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x, curStation.transform.position.y + 0.501f);


        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void DownArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x, curStation.transform.position.y - 0.501f);

        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }

    void UpRightArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x + 0.501f, curStation.transform.position.y + 0.501f);

        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void DownLeftArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x - 0.501f, curStation.transform.position.y - 0.501f);

        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void UpLeftArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x - 0.501f, curStation.transform.position.y + 0.501f);

        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode();
    }
    void DownRightArrow()
    {
        Vector2[] points = new Vector2[1];
        points[0] = new Vector2(curStation.transform.position.x + 0.501f, curStation.transform.position.y - 0.501f);

        RespawnTrain_(points);

        GetComponent<GameControler>().ActDeactRespawnTrainMode(); ;
    }

    
    public void DiactivateRespMode()
    {
        curStation = null;
        if (dirStationBut_1 != null)
            Destroy(dirStationBut_1.gameObject);
        if (dirStationBut_2 != null)
            Destroy(dirStationBut_2.gameObject);
    }


    void RespawnTrain(Vector2[] points)
    {
        curTrain++;

        GameObject train = Instantiate(TrainPref, curStation.transform.position, Quaternion.identity);
        train.SetActive(false);

        stations.Remove(curStation);

        int rand = Random.Range(0, stations.Count-1);
        int rand_color = Random.Range(0, colorsTrain.Count);

        stations.Add(curStation);

        stations[rand].GetComponent<Station>().SetColor(colorsTrain[rand_color]);

        train.GetComponent<TrainScript>().CreateTrain(colorsTrain[rand_color], gameObject, points, stations[rand], 0);

        stations.RemoveAt(rand);
        colorsTrain.RemoveAt(rand_color);

        train.SetActive(true);
    }


    public void RespawnTrain_Modif(GameObject respStation, Vector2[] points, GameObject arrivalStation, Color32 color, int countCarriage, out GameObject trainOut)
    {
        curTrain++;

        GameObject train = Instantiate(TrainPref, respStation.transform.position, Quaternion.identity);

        arrivalStation.GetComponent<Station>().SetColor(color);

        train.GetComponent<TrainScript>().CreateTrain(color, gameObject, points, arrivalStation, countCarriage);

        trainOut = train;
    }


    public void RespawnTrain_(Vector2[] points)
    {
        GameObject train = Instantiate(TrainPref, curStation.transform.position, Quaternion.identity);

        train.GetComponent<TrainScript>().CreateTrain(curStation, points);
    }


    public void AddStation (GameObject station)
    {
        stations.Add(station);
    }

    public int CountStation()
    {
        return stations.Count;
    }

    public void ArrivedAtStation(GameObject station, Color color)
    {
        curTrain--;
        stations.Add(station);
        colorsTrain.Add(color);

        if (miniMission)
            miniMission.TrainOnStation();
    }

    public void DeleteStation(GameObject station)
    {
        stations.Remove(station);
    }
}