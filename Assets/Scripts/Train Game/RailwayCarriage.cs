using System;
using System.Collections;
using System.CodeDom;
using System.Collections.Generic;
using UnityEngine;

public class RailwayCarriage : MonoBehaviour
{
    [Header("Components")]
    public TrainScript mainTrain;   // Ссылка на головной вагон

    // Параметры движения
    public float speed;
    bool reverse;       // Задний ход

    // Ссылки на предыдущий и следующий вагоны
    GameObject nextCarriage;                // Ссылка на следующий вагон
    GameObject backCarriage = null;         // Ссылка на предыдущий вагон

    // Этот вагон последний?
    public bool endRailwayCarriage;

    // Позиция вагона
    Vector2Int curPos = Vector2Int.zero;    // Текущая позиция вагона в сетке
    GameObject curCell;                     // Текущая клетка, в которой находится вагон

    // Информация о станциях
    private GameObject departureStation;    // Станция отбытия
    private GameObject arrivalStation;      // Станция прибытия
    
    // Информация о прибытии 
    private bool carriageOnStation;         // Этот вагон попал на станцию прибытия
    private bool trainOnStation;            // Один вагон из соства попал на станцию прибытия

    // Маршрут движения
    Vector2[] points;   // Точки, по которым движется вагон
    int curNumPoint;    // Номер точки, к которой движется вагон          

    // Стрелка, на которой находится вагон в текущий момент
    GameObject curArrow;        // Нужена, чтобы поезд, если его уничтожат на стрелке, отключал блокиратор стрелки

    // Номер вагона 
    int numberOfRailwayCarriage;
    
    // Вектор ошибки
    Vector2 vectorError = new Vector2(0.7654f, 0.321f); // Возвращается в случае непредвиденных обстоятельств, например, попал не в тот сектор жд дороги


    public void CreateRailwayCarriage(Color color, GameObject curCell, Vector2[] points, GameObject arrivalStation, TrainScript mainTrain, int numberOfRailwayCarriage) // Задаются основные параметры вагона
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
        carriageOnStation = false;

        curPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }


    void Update()
    {
        if (speed != 0)  // Движение
        {
            LookAtPoint(points[curNumPoint]); // Поворот вагона по направлению движения

            float distance; // Вагоны держуться на дистанции 0.7 от следующего вагона.
                            // В случае первого вагона от головного соства.
                            // Если поезда движуться задним ходом, то дистанцию 0.7
                            // они держат от предыдущего вагоны
                            // Управляющий - это вагон который движется со скоростью, остальные в это время двигаются на расстояние,
                            // которое проехал управляющий вагон. Управляющим вагоном в случае движения вперед будет головной вагон, назад - последний вагон
                            
            if (!reverse)
            {
                distance = Vector2.Distance(transform.position, nextCarriage.transform.position) - 0.7f;
                transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], distance);
            }
            else
            {
                if (endRailwayCarriage) // Если вагон последний, то при заднем ходе он становиться управляющим
                    transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], speed * Time.deltaTime);
                else
                {
                    distance = Vector2.Distance(transform.position, backCarriage.transform.position) - 0.7f;
                    transform.position = Vector2.MoveTowards(transform.position, points[curNumPoint], distance);
                }
            }           
        }


        // Переход к следующей в массиве точке
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


        // Если в массиве не осталось точек
        if (curNumPoint == points.Length) 
        {
            // Проверка на достижение этим вагоном станции назначения
            if (carriageOnStation)  // Если достиг
            {
                enabled = false;    // отключаем вагон

                // Если вагон был последним и поезд двигался вперед
                if (endRailwayCarriage && !reverse) // То значит, что весь состав находится на станции назначения, и можно уничтожать поезд
                {
                    mainTrain.DestroyTrain();
                }
            }
            else // Если не достиг, берем новые точки
                TakeNewPoints(); 
        }
    }


    void LookAtPointOnStart(Vector2 point)  // Поворот вагона в нужную сторону при создании 
    {
        point.x = point.x - transform.position.x;
        point.y = point.y - transform.position.y;

        float rot_z = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    void LookAtPoint(Vector2 point) // Поворот вагона по направлению движения
    {
        point.x = point.x - transform.position.x;
        point.y = point.y - transform.position.y;

        float rot_z = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;

        if (!reverse)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z), 15 * speed * Time.deltaTime);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 180), 15 * speed * Time.deltaTime);
    }

    void TakeNewPoints() // Получение новых координат движения
    {
        // Начало определения сектора, к отором назодиться вагон
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
        // Конец определения


        // Проверка на наличие клетки 
        if (curCell == null) // Если клетки нет - выключаем вагон и выдаем ошибку
        {
            enabled = false;
            Debug.LogError("I Don't Have Cell"); 
        }
        else // Если клетка есть получаем точки движения для жд дороги в этой клетке
        {
            // Передаем сектор, в котором находится поезд, данные о блокировке/разблокировке/бездействии (1/-1/0) стрелки, текущие точки (нужны для стрелки, чтобы выдать нужные следующие точки)
            if (endRailwayCarriage)  
                if (!reverse)
                    points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, -1, points); 
                else
                    points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, 1, points);
            else
                points = curCell.GetComponent<RailwayScript>().ActivateRailway(dir, 0, points);


            // Если возникла ошибка при выдаче точек
            if (points[0] == vectorError) 
            {
                enabled = false;
                Debug.LogError("ERROR");
            }
        }

        curNumPoint = 0;
    }

    public void SpeedChange(float speed) // Изменение скорости
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

    void ChangeDirection() // Смена направления 
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
        trainOnStation = true;
    }

    // Снятие блокировки со стрелки при уничтожении поезда на стрелке
    public void UnblockArrow()  
    {
        if (curArrow)
        {
            curArrow.GetComponent<ArrowRailway>().UnblockArrow();
        }
    }

    public void AddNextCarriage(GameObject nextCarriage)
    {
        this.nextCarriage = nextCarriage;
    }

    public void AddBackCarriage(GameObject backCarriage)
    {
        this.backCarriage = backCarriage;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Station" && departureStation == null)
        {
            departureStation = col.gameObject;
        }

        if (col.gameObject == arrivalStation)   // Вагон попал на конечную станцию
        {
            CarriageOnStation();
            mainTrain.GetComponent<TrainScript>().TrainOnStation();
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


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Train")   // Вагон сталкивается с головным вагоном
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
        Camera.main.GetComponent<GameControler>().ActivateSpeedPanel(mainTrain.gameObject); // Активация понели скоростей
    }


    IEnumerator WaitMainTrain(int count) 
    {
        yield return new WaitForSeconds(0.35f * count );
        enabled = true;
    }
}