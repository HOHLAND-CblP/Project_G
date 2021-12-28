using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{  
    [Header("Components")]
    GameObject mainCam;     // Главная камера
    

    // Параметры движения
    public float speed;
    bool reverse;           // Задний ход
    public GameObject loadingCicle;

    // Поезд находиться в процессе уничтожения
    bool destroing = false;


    // Factory
    int countResOnBoard;
    string res = "";
    bool onFactory;

    [Header("Prefabs")]
    public GameObject railwayCarriagePref;  // Префаб вагонетки
    public GameObject crashInfoPref;        // Префаб уведомления о столкновении
    
    
    // Текущее положение в сетке
    Vector2Int curPos = Vector2Int.zero;    // Текущая позиция поезда в сетке
    [SerializeField] GameObject curCell;                     // Текущая клетка, в которой находится поезд

    // Информация о станциях
    private GameObject departureStation;    // Станция отбытия
    private GameObject arrivalStation;      // Станция прибытия 
    private bool carriageOnStation;         // Вагон/головной состав попал на станцию прибытия
    private bool trainOnStaion;             // Один вагон из состава попал на станцию

    // Точки движения
    [SerializeField] Vector2[] points;   // Точки, по которым движется поезд
    int curNumPoint;    // Номер точки, к которой движется поезд в данный момент  

    GameObject curArrow;


    List<RailwayCarriage> railwayCarriages = new List<RailwayCarriage>(); // вагоны, идещии позади поезда


    Vector2 vectorError = new Vector2(0.7654f, 0.321f);


    public void CreateTrain(Color color, GameObject curCell, Vector2[] points, GameObject arrivalStation, int countCarriages)
    {
        GetComponent<SpriteRenderer>().color = color;
        this.curCell = curCell;
        this.points = points;
        this.arrivalStation = arrivalStation;


        LookAtPointOnStart(points[0]);

        for (int i = 0; i < countCarriages; i++)   // Создаем вагоны
        {
            GameObject carriage = Instantiate(railwayCarriagePref, transform.position, Quaternion.identity);
            railwayCarriages.Add(carriage.GetComponent<RailwayCarriage>());
            carriage.GetComponent<RailwayCarriage>().CreateRailwayCarriage(color, curCell, points, arrivalStation, this, i + 1);
            if (i == countCarriages - 1)
                carriage.GetComponent<RailwayCarriage>().endRailwayCarriage = true;

            if (i == 0)
                carriage.GetComponent<RailwayCarriage>().AddNextCarriage(gameObject);
            else
            {
                carriage.GetComponent<RailwayCarriage>().AddNextCarriage(railwayCarriages[i - 1].gameObject);
                railwayCarriages[i - 1].AddBackCarriage(carriage);
            }
        }
    }

    public void CreateTrain(GameObject curCell, Vector2[] points)
    {
        this.curCell = curCell;
        this.points = points;

        LookAtPointOnStart(points[0]);
    }


    void Start()
    {
        reverse = false;
        carriageOnStation = false;

        curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        mainCam = Camera.main.gameObject;
        curNumPoint = 0;
    }


    //----------------------------------Start Update

    void Update()
    {
        if (curCell == null)
        {
            curCell = mainCam.GetComponent<BuildingsGrid>().GetCellFromGrid(curPos.x, curPos.y);
            TakeNewPoints();
        }


        if (speed != 0)     // Движение
        {
            LookAtPoint(points[curNumPoint]);

            if (!reverse)
                transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], speed * Time.deltaTime);
            else
            {
                if (railwayCarriages.Count > 0)
                {
                    float distance = Vector2.Distance(transform.position, railwayCarriages[0].transform.position) - 0.7f;
                    transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], distance);
                }
                else
                    transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], speed * Time.deltaTime);
            }
        }

        if (transform.position.x == points[curNumPoint].x && transform.position.y == points[curNumPoint].y && curNumPoint != points.Length - 1)
            curNumPoint++;


        if (Mathf.Abs(transform.position.x - curPos.x) > 0.5f)
        {
            curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            curCell = mainCam.GetComponent<BuildingsGrid>().GetCellFromGrid(curPos.x, curPos.y);
        }
        if (Mathf.Abs(transform.position.y - curPos.y) > 0.5f)
        {
            curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            curCell = mainCam.GetComponent<BuildingsGrid>().GetCellFromGrid(curPos.x, curPos.y);
        }

        if (curNumPoint == points.Length - 1 && transform.position.x == points[curNumPoint].x && transform.position.y == points[curNumPoint].y)
        {
            if (carriageOnStation)
            {
                enabled = false;
                if (reverse || railwayCarriages.Count == 0)
                {
                    DestroyTrain();
                }
            }
            else if (Mathf.Abs(points[curNumPoint].x % 1) != 0.5f && Mathf.Abs(points[curNumPoint].y % 1) != 0.5f)
            {
                TakeNewPoints();
            }
            else
            {
                speed = 0;
            }

            if (onFactory)
            {
                StartCoroutine(OnFactory());
            }
        }
    }

    //-----------------------------------End Update

    void LookAtPointOnStart(Vector2 point)  // Поворот вагона при появлении в нужную сторону
    {
        point.x = point.x - transform.position.x;
        point.y = point.y - transform.position.y;

        float rot_z = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    void LookAtPoint(Vector2 point)
    {
        point.x = point.x - transform.position.x;
        point.y = point.y - transform.position.y;


        float rot_z = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;

        if (!reverse)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z), 15 * speed * Time.deltaTime);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z-180), 15 * speed * Time.deltaTime);
    }


    void TakeNewPoints()      
    {
        Vector2 dir;

        if (Mathf.Abs(transform.position.x - curPos.x) <= 0.1666f)
            dir.x = 0;
        else if (transform.position.x - curPos.x > 0)
            dir.x = 1;
        else
            dir.x = -1;


        if (Mathf.Abs(transform.position.y - curPos.y) <= 0.2f)
            dir.y = 0;
        else if (transform.position.y - curPos.y > 0)
            dir.y = 1;
        else
            dir.y = -1;

        if (curCell == null)
        {
            enabled = false;
            Debug.LogError("I Don't Have Cell");
        }
        else
        {
            if (railwayCarriages.Count == 0)
                points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, 0, points);
            else if (!reverse)
                points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, 1, points);
            else
                points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, -1, points);
        }

        if (points[0] == vectorError)
        {
            enabled = false;
            Debug.LogError("Vector Error");
        }


        curNumPoint = 0;
    }   
        
        
    public void SpeedChange(float speed)
    {
        if (speed < 0 && !reverse && !trainOnStaion)
        {
            ChangeDirection();
            this.speed = -speed;
            reverse = true;
        }
        else if (speed >= 0)
        {
            if (reverse)
            {
                if (!trainOnStaion)
                {
                    ChangeDirection();
                    reverse = false;
                    this.speed = speed;
                }
            }
            else
            {
                this.speed = speed;
            }
        }
        

        foreach(var r in railwayCarriages)
        {
            r.GetComponent<RailwayCarriage>().SpeedChange(speed);
        }
    }


    void ChangeDirection()
    {
        points = curCell.GetComponent<RailwayScript>().ChangeDirection(points, transform.position);

        if (points[0] == vectorError)
        {
            enabled = false;
            Debug.LogError("ERROR");
        }

        float minDist = 10;
        int minNumbPoint = 0;

        for (int i = 0; i < points.Length; i++)
        {
            
            float dist = Vector2.Distance(transform.position, points[i]);

            if (dist < minDist)
            {
                minNumbPoint = i;
                minDist = dist;
            }
            else
                break;
        }

        curNumPoint = minNumbPoint;

        float leftDist = Vector2.Distance(transform.InverseTransformPoint(transform.localPosition - transform.right), transform.InverseTransformPoint(points[curNumPoint]));
        float rightDist = Vector2.Distance(transform.InverseTransformPoint(transform.localPosition + transform.right), transform.InverseTransformPoint(points[curNumPoint]));


        if (reverse)
        {
            if (leftDist < rightDist)
                curNumPoint++;
        }
        else
        {
            if (leftDist > rightDist)
                curNumPoint++;
        }
    }

    void CarriageOnStation()
    {
        points = new Vector2[1];
        curNumPoint = 0;
        points[0] = curPos;
        carriageOnStation = true;
    }

    public void TrainOnStation()
    {
        trainOnStaion = true;
        foreach(var rc in railwayCarriages)
        {
            rc.GetComponent<RailwayCarriage>().TrainOnStation();
        }
    }


    public void DestroyTrain()
    {
        if (!destroing)
        {
            destroing = true;

            UnblockArrow();

            Camera.main.GetComponent<GameControler>().DeactSpeedPanel(gameObject);

            Camera.main.GetComponent<RespawnMode>().ArrivedAtStation(arrivalStation, GetComponent<SpriteRenderer>().color);
            if(arrivalStation)
                arrivalStation.GetComponent<Station>().RemoveColor();

            foreach (var r in railwayCarriages) // Уничтожаем вагоны
            {
                r.gameObject.GetComponent<RailwayCarriage>().UnblockArrow();
                Destroy(r.gameObject);
            }

            Destroy(gameObject);    // Уничтожаем поезд
        }
    }


    public bool IsThisMyCarriage(GameObject railCarr)
    {
        foreach (var car in railwayCarriages)
        {
            if (railCarr == car)
            {
                Debug.Log(true);
                return true;
            }
        }
        Debug.Log(false);
        return false;
    }


    // Снятие блокировки со стрелки при уничтожении поезда на стрелке
    public void UnblockArrow()   
    {
        if (curArrow)
        {
            curArrow.GetComponent<ArrowRailway>().UnblockArrow();
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Train")   // Столкновение с ведущего состава с другим ведущим составом
        {
            DestroyTrain();
        }

        /*if (col.transform.tag == "Carriage")  // Столкновение ведущего состава с вагоном другого поезда
        {
            if (!IsThisMyCarriage(col.gameObject))
            {
                col.gameObject.GetComponent<RailwayCarriage>().mainTrain.DestroyTrain();
                DestroyTrain();
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Station")
        {
            if (departureStation == null)
                departureStation = col.gameObject;

            if (col.GetComponent<Station>().defaultStation && res != "")
            {
                mainCam.GetComponent<GameControler>().AddResources(res, 10);
                DestroyTrain();
            }
        }

        if (col.gameObject == arrivalStation)   // Прибытие на станцию назначения 
        {
            CarriageOnStation();
            TrainOnStation();
        }

        if (col.tag == "Factory")
        {
            if (res == "")
            {
                onFactory = true;
                countResOnBoard = col.GetComponent<Factory>().TakeRespurce(10);
                res = col.GetComponent<Factory>().TypeOfRes();
                //StartCoroutine(OnFactory());
            }
        }

        if (col.tag == "Arrow")
        {
            curArrow = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Arrow" && col.gameObject == curArrow)
        { 
            curArrow = null;
        }
    }


    private void OnMouseDown()
    {
        Camera.main.GetComponent<GameControler>().ActivateSpeedPanel(gameObject);
    }

    IEnumerator OnFactory()
    {
        enabled = false;
        GameObject lc = Instantiate(loadingCicle, transform.position, Quaternion.identity, mainCam.GetComponent<GameControler>().GetCanvas().transform);
        lc.transform.SetAsFirstSibling();
        lc.GetComponent<LoadingCircle>().NewLoad(5, transform);
        

        yield return new WaitForSeconds(5f);

        switch (res)
        {
            case "Wood":
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(215, 151, 101, 255);
                break;

            case "Stone":
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(137, 164, 166, 255);
                break;

            case "Iron":
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(199, 195, 195, 255);
                break;
        }

        onFactory = false;
        enabled = true;
    }
}