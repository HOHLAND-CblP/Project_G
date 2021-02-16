using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwayCarriage : MonoBehaviour
{
    public TrainScript mainTrain;

    public bool endRailwayCarriage;

    public float speed;
    bool reverse;       // Задний ход
    bool canGo;         // Можно ли ехать

    [SerializeField]
    Vector2Int curPos = Vector2Int.zero;    // текущая позиция вагона по клеточкам
    GameObject curCell;                     // текущая клеточка, в которой находится вагон

    // Станция прибытия
    private GameObject arrivalStation;      // Станция прибытия 
    private bool carriageOnStation;         // Вагон/головной состав попал на станцию прибытия
    private bool trainOnStation;             // Один вагон из состава попал на станцию

    [SerializeField]
    Vector2[] points;   //точки, по которым движется вагон
    int curNumPoint;    //номер точки, к которой движется вагон          
    

    int numberOfRailwayCarriage;

    Vector2 vectorError = new Vector2(0.7654f, 0.321f);


    public void CreateRailwayCarriage(Color color, GameObject curCell, Vector2[] points, GameObject arrivalStation, TrainScript mainTrain, int numberOfRailwayCarriage)
    {
        GetComponent<SpriteRenderer>().color = color;
        this.curCell = curCell;
        this.points = points;
        this.arrivalStation = arrivalStation;
        this.mainTrain = mainTrain;
        this.numberOfRailwayCarriage = numberOfRailwayCarriage;
        
        LookAtPointOnStart(points[0]);

        StartCoroutine(WaitMainTrain(numberOfRailwayCarriage));
    }



    void Start()
    {
        canGo = true;

        carriageOnStation = false;

        curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }


    void Update()
    {
/*        if (curCell == null)
        {
            curCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(curPos.x, curPos.y);
            TakeNewPoints();
        }*/

        

        if (canGo)
        {
            LookAtPoint(points[curNumPoint]);
            transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], speed * Time.deltaTime); // движение
        }

        if (transform.position.x == points[curNumPoint].x && transform.position.y == points[curNumPoint].y)
        {
            curNumPoint++;
        }



        if (Mathf.Abs(transform.position.x - curPos.x) > 0.5f)
        {
            curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            curCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(curPos.x, curPos.y);
        }
        if (Mathf.Abs(transform.position.y - curPos.y) > 0.5f)
        {
            curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            curCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(curPos.x, curPos.y);
        }


        if (curNumPoint == points.Length)
        {
            if (carriageOnStation)
            {
                enabled = false;
                if (endRailwayCarriage && !reverse)
                {
                    mainTrain.GetComponent<TrainScript>().DestroyTrain();
                }
            }
            else
                TakeNewPoints();
        }
    }


    void LookAtPointOnStart(Vector2 point)
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 180), 15 * speed * Time.deltaTime);
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
            if (endRailwayCarriage)
                if (!reverse)
                    points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, -1, points);
                else
                    points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, 1, points);
            else
                points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, 0, points);

            if (points[0] == vectorError)
            {
                enabled = false;
                Debug.LogError("ERROR");
            }
        }

        curNumPoint = 0;
    }

    public void SpeedChange(float speed)
    {
        if (speed < 0 && !reverse && !trainOnStation)
        {
            ChangeDirection();
            this.speed = -speed;
            reverse = true;
        }
        else if (speed >= 0)
        {
            if (reverse)
            {
                if (!trainOnStation)
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
        trainOnStation = true;
    }


    public void CarriageCantGo()
    {
        canGo = false;
    }

    public void CarriageCanGo()
    {
        canGo = true;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == arrivalStation)
        {
            CarriageOnStation();
            mainTrain.GetComponent<TrainScript>().TrainOnStation();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Arrow")
        {
            mainTrain.GetComponent<TrainScript>().TrainCantGo();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Arrow")
        {
            mainTrain.GetComponent<TrainScript>().TrainCanGo();
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Train")
        {
            if (col.gameObject != mainTrain.gameObject)
            {
                col.transform.GetComponent<TrainScript>().DestroyTrain();
                mainTrain.GetComponent<TrainScript>().DestroyTrain();
            }
        }

        /*if (col.tag == "Railway Carriage")
             {
                 if (!mainTrain.IsThisMyCarriage(col.gameObject))
                 {
                     col.GetComponent<RailwayCarriage>().mainTrain.DestroyTrain();
                     mainTrain.DestroyTrain();
                 }
             }*/
    }


    private void OnMouseDown()
    {
        Camera.main.GetComponent<GameControler>().ActivateSpeedPanel(mainTrain.gameObject);
    }


    IEnumerator WaitMainTrain(int count)
    {
        yield return new WaitForSeconds(0.35f * count);
        enabled = true;
    }
}