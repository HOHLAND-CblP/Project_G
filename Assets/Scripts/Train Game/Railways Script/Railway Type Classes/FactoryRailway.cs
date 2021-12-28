using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryRailway : MonoBehaviour
{
    [Header("Railway")]
    [SerializeField]
    GameObject railway;

    private RailwayScript railwayScript;

    // points
    Vector2[] points_1 = new Vector2[1];
    Vector2[] points_2 = new Vector2[1];

    Vector2 dir_1;  // сектора входа поезда
    Vector2 dir_2;

    //possible types
    bool horizontalRigth;
    bool horizontalLeft;
    bool verticalUp;
    bool verticalDown;
    bool diagonal00;
    bool diagonal01;
    bool diagonal10;
    bool diagonal11;


    GameObject[,] aroundCell = new GameObject[3, 3];

    int curTypeOfRoad = 0;


    readonly Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) };


    private void Start()
    {
        railwayScript = GetComponent<RailwayScript>();
    }


    public Vector2[] GetPoints(Vector2 dir)
    {
        if (dir == dir_1)
            return points_1;
        else if (dir == dir_2)
            return points_2;
        else
            return vectorError;
    }

    public Vector2[] ChangeDirection(Vector2[] points)
    {
        if (points == points_1)
            return points_2;
        else if (points == points_2)
            return points_1;
        return vectorError;
    }




    public void MakeVisualConects()
    {
        PossibleTypes();
        SelectCurType();
        

        switch (curTypeOfRoad)
        {
            case 0:
                railwayScript.DeleteConects();
                break;

            case 1:
                HorizontalRigthVisualConects();
                break;

            case 2:
                HorizontalLeftVisualConects();
                break;

            case 3:
                VerticalUpVisualConects();
                break;

            case 4:
                VerticalDownVisualConects();
                break;

            case 5:
                Diagonal00VisualConects();
                break;

            case 6:
                Diagonal01VisualConects();
                break;

            case 7:
                Diagonal10VisualConects();
                break;

            case 8:
                Diagonal11VisualConects();
                break;
        }
    }

    void PossibleTypes()
    {
        horizontalRigth = false;
        horizontalLeft = false;
        verticalUp = false;
        verticalDown = false;
        diagonal00 = false;
        diagonal01 = false;
        diagonal10 = false;
        diagonal11 = false;

        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x + i, (int)transform.position.y + k);


        if (aroundCell[2, 1])
            if (aroundCell[2, 1].GetComponent<RailwayScript>() && aroundCell[2, 1].GetComponent<RailwayScript>().IsConect(new Vector2(1, 0)))
                horizontalRigth = true;

        if (aroundCell[0, 1])
            if (aroundCell[0, 1].GetComponent<RailwayScript>() && aroundCell[0, 1].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 0)))
                horizontalLeft = true;

        if (aroundCell[1, 2])
            if (aroundCell[1, 2].GetComponent<RailwayScript>() && aroundCell[1, 2].GetComponent<RailwayScript>().IsConect(new Vector2(0, 1)))
                verticalUp = true;

        if (aroundCell[1, 0])
            if (aroundCell[1, 0].GetComponent<RailwayScript>() && aroundCell[1, 0].GetComponent<RailwayScript>().IsConect(new Vector2(0, -1)))
                verticalDown = true;

        if (aroundCell[0, 0])
            if (aroundCell[0, 0].GetComponent<RailwayScript>() && aroundCell[0, 0].GetComponent<RailwayScript>().IsConect(new Vector2(-1, -1)))
                diagonal00 = true;

        if (aroundCell[0, 2])
            if (aroundCell[0, 2].GetComponent<RailwayScript>() && aroundCell[0, 2].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 1)))
                diagonal01 = true;

        if (aroundCell[2, 0])
            if (aroundCell[2, 0].GetComponent<RailwayScript>() && aroundCell[2, 0].GetComponent<RailwayScript>().IsConect(new Vector2(1, -1)))
                diagonal10 = true;

        if (aroundCell[2, 2])
            if (aroundCell[2, 2].GetComponent<RailwayScript>() && aroundCell[2, 2].GetComponent<RailwayScript>().IsConect(new Vector2(1, 1)))
                    diagonal11 = true;
    }

    void SelectCurType()
    {
        if (curTypeOfRoad != 0)
            switch (curTypeOfRoad)      // Если тип выбран, но он не соответствует одному из возможных в данный момент типов, то тип сбрасывается   
            {
                case 1:
                    if (!horizontalRigth)
                        curTypeOfRoad = 0;
                    break;

                case 2:
                    if (!horizontalLeft)
                        curTypeOfRoad = 0;
                    break;

                case 3:
                    if (!verticalUp)
                        curTypeOfRoad = 0;
                    break;

                case 4:
                    if (!verticalDown)
                        curTypeOfRoad = 0;
                    break;

                case 5:
                    if (!diagonal00)
                        curTypeOfRoad = 0;
                    break;

                case 6:
                    if (!diagonal01)
                        curTypeOfRoad = 0;
                    break;

                case 7:
                    if (!diagonal10)
                        curTypeOfRoad = 0;
                    break;

                case 8:
                    if (!diagonal11)
                        curTypeOfRoad = 0;
                    break;
            }
        else
        {
            if (horizontalRigth)
                curTypeOfRoad = 1;
            else if (horizontalLeft)
                curTypeOfRoad = 2;
            else if (verticalUp)
                curTypeOfRoad = 3;
            else if (verticalDown)
                curTypeOfRoad = 4;
            else if (diagonal00)
                curTypeOfRoad = 5;
            else if (diagonal01)
                curTypeOfRoad = 6;
            else if (diagonal10)
                curTypeOfRoad = 7;
            else if (diagonal11)
                curTypeOfRoad = 8;
        }
    }


    public void Draw()
    {
        switch (curTypeOfRoad)
        {
            case 0:
                railway.transform.localScale = new Vector3(0, 0, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 0);
                railway.transform.position = new Vector3(0, 0, 0);
                break;

            case 1:
                railway.transform.localScale = new Vector3(0.5f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 0);
                railway.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y, 0);
                break;

            case 2:
                railway.transform.localScale = new Vector3(0.5f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 0);
                railway.transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y , 0);
                break;

            case 3:
                railway.transform.localScale = new Vector3(0.5f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 90);
                railway.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, 0);
                break;

            case 4:
                railway.transform.localScale = new Vector3(0.5f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 90);
                railway.transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, 0);
                break;

            case 5:
                railway.transform.localScale = new Vector3(0.5f * 1.42f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 45);
                railway.transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y - 0.25f, 0);
                break;

            case 6:
                railway.transform.localScale = new Vector3(0.5f * 1.42f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 135);
                railway.transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y + 0.25f, 0);
                break;

            case 7:
                railway.transform.localScale = new Vector3(0.5f * 1.42f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 135);
                railway.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y - 0.25f, 0);
                break;

            case 8:
                railway.transform.localScale = new Vector3(0.5f * 1.42f, 1, 1);
                railway.transform.eulerAngles = new Vector3(0, 0, 45);
                railway.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 0.25f, 0);
                break;

        }
    }


    public void Build()
    {
        switch (curTypeOfRoad)
        {
            case 0:
                GetComponent<RailwayScript>().DeleteConects();
                points_1[0] = new Vector2();
                points_2[0] = new Vector2();
                dir_1 = new Vector2();
                dir_2 = new Vector2();
                break;

            case 1:
                HorizontalRigthPoints();
                HorizontalRigthConects();
                break;

            case 2:
                HorizontalLeftPoints();
                HorizontalLeftConects();
                break;

            case 3:
                VerticalUpPoints();
                VerticalUpConects();
                break;

            case 4:
                VerticalDownPoints();
                VerticalDownConects();
                break;

            case 5:
                Diagonal00Points();
                Diagonal00Conects();
                break;

            case 6:
                Diagonal01Points();
                Diagonal01Conects();
                break;

            case 7:
                Diagonal10Points();
                Diagonal10Conects();
                break;

            case 8:
                Diagonal11Points();
                Diagonal11Conects();
                break;
        }
    }

    void HorizontalRigthVisualConects()
    {
        railwayScript.DeleteConects();
        
        railwayScript.AddConect(new Vector2(1, 0));
    }
    void HorizontalLeftVisualConects()
    {
        railwayScript.DeleteConects();

        railwayScript.AddConect(new Vector2(-1, 0));
    }
    void VerticalUpVisualConects()
    {
        railwayScript.DeleteConects();

        railwayScript.AddConect(new Vector2(0, 1));
    }
    void VerticalDownVisualConects()
    {
        railwayScript.DeleteConects();

        railwayScript.AddConect(new Vector2(0, -1));
    }
    void Diagonal00VisualConects()
    {
        railwayScript.DeleteConects();

        railwayScript.AddConect(new Vector2(-1, -1));
    }
    void Diagonal01VisualConects()
    {
        railwayScript.DeleteConects();

        railwayScript.AddConect(new Vector2(-1, 1));
    }
    void Diagonal10VisualConects()
    {
        railwayScript.DeleteConects();

        railwayScript.AddConect(new Vector2(1, -1));
    }
    void Diagonal11VisualConects()
    {
        railwayScript.DeleteConects();

        railwayScript.AddConect(new Vector2(1, 1));
    }


    void HorizontalRigthPoints()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y);
        points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y);
          
    }
    void HorizontalLeftPoints()
    {
        points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y);
        points_2[0] = new Vector2(transform.position.x, transform.position.y);
    }
    void VerticalUpPoints()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y);
        points_2[0] = new Vector2(transform.position.x, transform.position.y + 0.501f);
    }
    void VerticalDownPoints()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y - 0.501f);
        points_2[0] = new Vector2(transform.position.x, transform.position.y);   
    }
    void Diagonal00Points()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y);
        points_2[0] = new Vector2(transform.position.x - 0.501f, transform.position.y - 0.501f);
    }
    void Diagonal01Points()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y);
        points_2[0] = new Vector2(transform.position.x - 0.501f, transform.position.y + 0.501f);
    }
    void Diagonal10Points()
    {
        points_1[0] = new Vector2(transform.position.x + 0.501f, transform.position.y - 0.501f);
        points_2[0] = new Vector2(transform.position.x, transform.position.y);
    }
    void Diagonal11Points()
    {
        points_1[0] = new Vector2(transform.position.x + 0.501f, transform.position.y + 0.501f);
        points_2[0] = new Vector2(transform.position.x, transform.position.y);
    }

    void HorizontalRigthConects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2(1, 0);
        dir_2 = new Vector2();

        GetComponent<RailwayScript>().AddConect(dir_1);
    }
    void HorizontalLeftConects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2();
        dir_2 = new Vector2(-1, 0);

        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void VerticalUpConects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2(0, 1);
        dir_2 = new Vector2();

        GetComponent<RailwayScript>().AddConect(dir_1);
    }
    void VerticalDownConects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2();
        dir_2 = new Vector2(0, -1);

        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Diagonal00Conects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2(-1, -1);
        dir_2 = new Vector2();

        GetComponent<RailwayScript>().AddConect(dir_1);
    }
    void Diagonal01Conects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2(-1, 1);
        dir_2 = new Vector2();

        GetComponent<RailwayScript>().AddConect(dir_1);
    }
    void Diagonal10Conects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2();
        dir_2 = new Vector2(1, -1);

        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Diagonal11Conects()
    {
        railwayScript.DeleteConects();

        dir_1 = new Vector2();
        dir_2 = new Vector2(1, 1);

        GetComponent<RailwayScript>().AddConect(dir_2);
    }
}