using UnityEngine;

public class ArrowRailway : MonoBehaviour
{
    [SerializeField]
    bool canGo;     //Может ли ехать поезд на стрелке в нужном ему направлении

    bool isTurnBlocked;     // блокировка кнопки поворота

    [SerializeField]    
    bool turn;      //стрелка стоит на поворот или на прямой участок

    public Sprite arrowTurn;
    public Sprite arrowForward;

    [Space(20)]
    public PointerButton pointer;

    [Space(20)]
    public GameObject turnColl;
    public GameObject forwardColl;

    Vector2 dirToArrow;                     // направлнение с которого приедит поезд на стрелку
    Vector2 dirAgainstArrowFromForward;     // направление с котороо приедит поезд против стрелки при движении по прямой
    Vector2 dirAgainstArrowFromTurn;        // направление с котороо приедит поезд против стрелки при движении с поворота

    Vector2[] pointsBeforeArrow = new Vector2[1];       // точки до стрелки при движении на стрелку
    Vector2[] pointsAfterArrowTurn = new Vector2[5];    // точки после стрелки при повороте 
    Vector2[] pointsAfterArrowForward = new Vector2[1]; // точки после стрелки при движении вперед

    Vector2[] pointsTurn = new Vector2[5];              // точки при двежении провтив стрелки на повороте до стрелки
    Vector2[] pointsForward_1 = new Vector2[1];         // точки при движении против стрелки по прямой до стрелки
    Vector2[] pointsForward_2 = new Vector2[1];         // точки для движения против стрелки по прямой/на повороте после стрелки


    Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) };



    void Awake()
    {
        pointer = transform.GetChild(0).GetComponent<PointerButton>();


        MakeNewPoints();
    }


    [ContextMenu(itemName: "Change Of Direction")]
    public void ChangeOfDirectionArrow()    // смена направления стрелки
    {
        if (!isTurnBlocked)
        {
            turn = !turn;
            if (turn)
            {
                turnColl.SetActive(false);
                forwardColl.SetActive(true);
                pointer.transform.localRotation = Quaternion.Euler(0, 0, 45);
                GetComponent<SpriteRenderer>().sprite = arrowTurn;
            }
            else
            {
                turnColl.SetActive(true);
                forwardColl.SetActive(false);
                pointer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                GetComponent<SpriteRenderer>().sprite = arrowForward;
            } 
        }
    }


    public Vector2[] GetPoints(Vector2 dir, int rangRailwayCarriage, Vector2[] points) // каждый состав имеет ранговую систему, движущий состав 1, среднии вагоны 0, конечный вагон -1 и наоборот, если поезд едит задним ходом
    {
        if (dir == dirToArrow)
        {
            return pointsBeforeArrow;
        }
        else if (dir == dirAgainstArrowFromForward)
        {
            return pointsForward_1;
        }
        else if (dir == dirAgainstArrowFromTurn)
        {
            return pointsTurn;
        }
        else if (dir == new Vector2(0, 0))
        {
            switch (rangRailwayCarriage)
            {
                case 1:
                    isTurnBlocked = true;
                    break;
                case -1:
                    isTurnBlocked = false;
                    break;
            }
            if (points == pointsBeforeArrow)
            {
                if (turn)
                    return pointsAfterArrowTurn;
                else
                    return pointsAfterArrowForward;
            }
            else
            {
                return pointsForward_2;
            }
        }
        else
            return vectorError;
    }

    public Vector2[] ChangeDirection(Vector2[] points, Vector2 trainPos)
    {
        if (points == pointsBeforeArrow)
        {
            return pointsForward_2;
        }
        else if (points == pointsAfterArrowForward)
        {
            return pointsForward_1;
        }
        else if (points == pointsAfterArrowTurn)
        {
            return pointsTurn;
        }
        else if (points == pointsForward_1)
            return pointsAfterArrowForward;
        else if (points == pointsTurn)
            return pointsAfterArrowTurn;
        else if (points == pointsForward_2)
            return pointsBeforeArrow;
        return vectorError;
    }

    public void MakeNewPoints()
    {
        GetComponent<RailwayScript>().DeleteConects();


        switch (transform.rotation.eulerAngles.z)
        {
            case 0:
                PointsFor0Grad();
                break;

            case 90:
                PointsFor90Grad();
                break;

            case 180:
                PointsFor180Grad();
                break;

            case 270:
                PointsFor270Grad();
                break;
        }
    }

    void PointsFor0Grad()
    {
        pointsBeforeArrow[0] = new Vector2(transform.position.x - 0.1f, transform.position.y);

        pointsAfterArrowTurn[0] = new Vector2(transform.position.x - 0.05f, transform.position.y + transform.localScale.y * 0.005f);
        pointsAfterArrowTurn[1] = new Vector2(transform.position.x + 0.01f, transform.position.y + transform.localScale.y * 0.033f);
        pointsAfterArrowTurn[2] = new Vector2(transform.position.x + 0.05f, transform.position.y + transform.localScale.y * 0.06f);
        pointsAfterArrowTurn[3] = new Vector2(transform.position.x + 0.092f, transform.position.y + transform.localScale.y * 0.092f);
        pointsAfterArrowTurn[4] = new Vector2(transform.position.x + 0.501f, transform.position.y + transform.localScale.y * 0.501f);

        pointsAfterArrowForward[0] = new Vector2(transform.position.x + 0.501f, transform.position.y);

        pointsTurn[0] = new Vector2(transform.position.x + 0.092f, transform.position.y + transform.localScale.y * 0.092f);
        pointsTurn[1] = new Vector2(transform.position.x + 0.05f, transform.position.y + transform.localScale.y * 0.06f);
        pointsTurn[2] = new Vector2(transform.position.x + 0.01f, transform.position.y + transform.localScale.y * 0.033f);
        pointsTurn[3] = new Vector2(transform.position.x - 0.05f, transform.position.y + transform.localScale.y * 0.005f);
        pointsTurn[4] = new Vector2(transform.position.x - 0.1f, transform.position.y);

        pointsForward_1[0] = new Vector2(transform.position.x - 0.1f, transform.position.y);

        pointsForward_2[0] = new Vector2(transform.position.x - 0.501f, transform.position.y);

        dirToArrow = new Vector2(-1, 0);
        dirAgainstArrowFromForward = new Vector2(1, 0);
        dirAgainstArrowFromTurn = new Vector2(1, transform.localScale.y);

        GetComponent<RailwayScript>().AddConect(dirToArrow);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromForward);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromTurn);
    }

    void PointsFor90Grad()
    {
        pointsBeforeArrow[0] = new Vector2(transform.position.x, transform.position.y - 0.1f);

        pointsAfterArrowTurn[0] = new Vector2(transform.position.x - transform.localScale.y * 0.005f, transform.position.y - 0.05f);
        pointsAfterArrowTurn[1] = new Vector2(transform.position.x - transform.localScale.y * 0.033f, transform.position.y + 0.01f);
        pointsAfterArrowTurn[2] = new Vector2(transform.position.x - transform.localScale.y * 0.06f, transform.position.y + 0.05f);
        pointsAfterArrowTurn[3] = new Vector2(transform.position.x - transform.localScale.y * 0.092f, transform.position.y + 0.092f);
        pointsAfterArrowTurn[4] = new Vector2(transform.position.x - transform.localScale.y * 0.501f, transform.position.y + 0.501f);

        pointsAfterArrowForward[0] = new Vector2(transform.position.x, transform.position.y+ 0.501f);

        pointsTurn[0] = new Vector2(transform.position.x - transform.localScale.y * 0.092f, transform.position.y + 0.092f);
        pointsTurn[1] = new Vector2(transform.position.x - transform.localScale.y * 0.06f, transform.position.y + 0.05f);
        pointsTurn[2] = new Vector2(transform.position.x - transform.localScale.y * 0.033f, transform.position.y + 0.01f);
        pointsTurn[3] = new Vector2(transform.position.x - transform.localScale.y * 0.005f, transform.position.y - 0.05f);
        pointsTurn[4] = new Vector2(transform.position.x, transform.position.y - 0.1f);

        pointsForward_1[0] = new Vector2(transform.position.x, transform.position.y - 0.1f);

        pointsForward_2[0] = new Vector2(transform.position.x, transform.position.y - 0.501f);

        dirToArrow = new Vector2(0, -1);
        dirAgainstArrowFromForward = new Vector2(0, 1);
        dirAgainstArrowFromTurn = new Vector2(-transform.localScale.y, 1);

        GetComponent<RailwayScript>().AddConect(dirToArrow);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromForward);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromTurn);
    }

    void PointsFor180Grad()
    {
        pointsBeforeArrow[0] = new Vector2(transform.position.x + 0.1f, transform.position.y);

        pointsAfterArrowTurn[0] = new Vector2(transform.position.x + 0.05f, transform.position.y - transform.localScale.y * 0.005f);
        pointsAfterArrowTurn[1] = new Vector2(transform.position.x - 0.01f, transform.position.y - transform.localScale.y * 0.033f);
        pointsAfterArrowTurn[2] = new Vector2(transform.position.x - 0.05f, transform.position.y - transform.localScale.y * 0.06f);
        pointsAfterArrowTurn[3] = new Vector2(transform.position.x - 0.092f, transform.position.y - transform.localScale.y * 0.092f);
        pointsAfterArrowTurn[4] = new Vector2(transform.position.x - 0.501f, transform.position.y - transform.localScale.y * 0.501f);

        pointsAfterArrowForward[0] = new Vector2(transform.position.x - 0.501f, transform.position.y);

        pointsTurn[0] = new Vector2(transform.position.x - 0.092f, transform.position.y - transform.localScale.y * 0.092f);
        pointsTurn[1] = new Vector2(transform.position.x - 0.05f, transform.position.y - transform.localScale.y * 0.06f);
        pointsTurn[2] = new Vector2(transform.position.x - 0.01f, transform.position.y - transform.localScale.y * 0.033f);
        pointsTurn[3] = new Vector2(transform.position.x + 0.05f, transform.position.y - transform.localScale.y * 0.005f);
        pointsTurn[4] = new Vector2(transform.position.x + 0.1f, transform.position.y);

        pointsForward_1[0] = new Vector2(transform.position.x + 0.1f, transform.position.y);

        pointsForward_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y);

        dirToArrow = new Vector2(1, 0);
        dirAgainstArrowFromForward = new Vector2(-1, 0);
        dirAgainstArrowFromTurn = new Vector2(-1, -transform.localScale.y);

        GetComponent<RailwayScript>().AddConect(dirToArrow);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromForward);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromTurn);
    }

    void PointsFor270Grad()
    {
        pointsBeforeArrow[0] = new Vector2(transform.position.x, transform.position.y + 0.1f);

        pointsAfterArrowTurn[0] = new Vector2(transform.position.x + transform.localScale.y * 0.005f, transform.position.y + 0.05f);
        pointsAfterArrowTurn[1] = new Vector2(transform.position.x + transform.localScale.y * 0.033f, transform.position.y - 0.01f);
        pointsAfterArrowTurn[2] = new Vector2(transform.position.x + transform.localScale.y * 0.06f, transform.position.y - 0.05f);
        pointsAfterArrowTurn[3] = new Vector2(transform.position.x + transform.localScale.y * 0.092f, transform.position.y - 0.092f);
        pointsAfterArrowTurn[4] = new Vector2(transform.position.x + transform.localScale.y * 0.501f, transform.position.y - 0.501f);

        pointsAfterArrowForward[0] = new Vector2(transform.position.x, transform.position.y - 0.501f);

        pointsTurn[0] = new Vector2(transform.position.x + transform.localScale.y * 0.092f, transform.position.y - 0.092f);
        pointsTurn[1] = new Vector2(transform.position.x + transform.localScale.y * 0.06f, transform.position.y - 0.05f);
        pointsTurn[2] = new Vector2(transform.position.x + transform.localScale.y * 0.033f, transform.position.y - 0.01f);
        pointsTurn[3] = new Vector2(transform.position.x + transform.localScale.y * 0.005f, transform.position.y + 0.05f);
        pointsTurn[4] = new Vector2(transform.position.x, transform.position.y + 0.1f);

        pointsForward_1[0] = new Vector2(transform.position.x, transform.position.y + 0.1f);

        pointsForward_2[0] = new Vector2(transform.position.x, transform.position.y + 0.501f);

        dirToArrow = new Vector2(0, 1);
        dirAgainstArrowFromForward = new Vector2(0, -1);
        dirAgainstArrowFromTurn = new Vector2(transform.localScale.y, -1);

        GetComponent<RailwayScript>().AddConect(dirToArrow);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromForward);
        GetComponent<RailwayScript>().AddConect(dirAgainstArrowFromTurn);
    }


 
    public void Turn(int k)
    {
        if (k == 1)
            if (transform.localScale.y == 1)
            {
                transform.Rotate(0, 0, 90 * k);
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
            else
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        else
        if (transform.localScale.y == -1)
        {
            transform.Rotate(0, 0, 90 * k);
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
        else
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

    }

}