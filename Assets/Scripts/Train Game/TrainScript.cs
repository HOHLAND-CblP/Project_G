using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{  
    [Header("Components")]
    [SerializeField]
    private float speed;
    bool reverse;       // Задний ход
    bool canGo;         // Можно ли ехать
    bool destroing = false;

    GameObject mainCam;

    [Space]
    public GameObject railwayCarriagePref;  // Префаб вагонетки
    public GameObject crashInfoPref;        // Префаб уведомления о столкновении

    // Текущее положение в сетке
    Vector2Int curPos = Vector2Int.zero;    // Текущая позиция поезда по клеточкам
    GameObject curCell;                     // Текущая клеточка, в которой находится поезд

    // Станция прибытия
    private GameObject arrivalStation;      // Станция прибытия 
    private bool carriageOnStation;         // Вагон/головной состав попал на станцию прибытия
    private bool trainOnStaion;             // Один вагон из состава попал на станцию

    // Точки движения
    [SerializeField]
    Vector2[] points;   // Точки, по которым движется поезд
    [SerializeField]
    int curNumPoint;    // Номер точки, к которой движется поезд в данный момент            


    List<RailwayCarriage> railwayCarriages = new List<RailwayCarriage>(); // вагоны, идещии позади поезда


    Vector2 vectorError = new Vector2(0.7654f, 0.321f);


    public void CreateTrain(Color color, GameObject curCell, Vector2[] points, GameObject arrivalStation, int countCarriages)
    {
        GetComponent<SpriteRenderer>().color = color;
        this.curCell = curCell;
        this.points = points;
        this.arrivalStation = arrivalStation;

        float point_x = points[0].x - transform.position.x;
        float point_y = points[0].y - transform.position.y;
        float rot_z = Mathf.Atan2(point_y, point_x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        for (int i = 0; i < countCarriages; i++)
        {
            GameObject carriage = Instantiate(railwayCarriagePref, transform.position, Quaternion.identity);
            railwayCarriages.Add(carriage.GetComponent<RailwayCarriage>());
            carriage.GetComponent<RailwayCarriage>().CreateRailwayCarriage(color, curCell, points, arrivalStation, this, i+1);
            if (i == countCarriages - 1)
                carriage.GetComponent<RailwayCarriage>().endRailwayCarriage = true;
        }
    }


    void Start()
    {
        canGo = true;
        reverse = false;
        carriageOnStation = false;

        curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        mainCam = Camera.main.gameObject;
        curNumPoint = 0;
    }


    void Update()
    {
        if (curCell == null)
        {
            curCell = mainCam.GetComponent<BuildingsGrid>().GetCellFromGrid(curPos.x, curPos.y);
            TakeNewPoints(); 
        }


        if (canGo)
        {
            LookAtPoint(points[curNumPoint]);
            transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], speed * Time.deltaTime); // движение
        }
            

        if (transform.position.x == points[curNumPoint].x && transform.position.y == points[curNumPoint].y)
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

        if (curNumPoint == points.Length)
        {
            if (carriageOnStation)
            {
                enabled = false;
                if (reverse)
                {
                    DestroyTrain();
                }
            }
            else
                TakeNewPoints();
        }
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
            Debug.LogError("ERROR");
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
            {
                Debug.Log(1);
                Debug.Log(transform.InverseTransformPoint(points[curNumPoint]));
                Debug.Log(leftDist);
                Debug.Log(rightDist);
                curNumPoint++;
            }
        }
        else
        {
            if (leftDist > rightDist)
            {
                Debug.Log(2);
                Debug.Log(transform.InverseTransformPoint(points[curNumPoint]));
                Debug.Log(leftDist);
                Debug.Log(rightDist);
                curNumPoint++;
            }
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

            Camera.main.GetComponent<GameControler>().DeactSpeedPanel(gameObject);

            Camera.main.GetComponent<RespawnMode>().ArrivedAtStation(arrivalStation, GetComponent<SpriteRenderer>().color);
            arrivalStation.GetComponent<Station>().SetColor(Color.white);

            foreach (var r in railwayCarriages)
            {
                Destroy(r.gameObject);
            }

            Debug.Log(name);

            Destroy(gameObject);
        }
    }


    public void TrainCantGo()
    {
        canGo = false;

        foreach(var rc in railwayCarriages)
        {
            rc.GetComponent<RailwayCarriage>().CarriageCantGo();
        }

        SpeedChange(1);
    }

    public void TrainCanGo()
    {
        canGo = true;

        foreach (var rc in railwayCarriages)
        {
            rc.GetComponent<RailwayCarriage>().CarriageCanGo();
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


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Train")
        {
            DestroyTrain();
        }

        /*if (col.transform.tag == "Carriage")
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
        if (col.gameObject == arrivalStation)
        {
            CarriageOnStation();
            TrainOnStation();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Arrow")
        {
            TrainCantGo();
        }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Arrow")
        {
            TrainCanGo();
        }
    }


    private void OnMouseDown()
    {
        Camera.main.GetComponent<GameControler>().ActivateSpeedPanel(gameObject);
    }
}