using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Station : MonoBehaviour
{
    bool endStation;
    bool horizontal = false;    //тип дороги для отрисовки
    bool vertical = false;
    bool diagonal_1 = false;    // Вертикаль от снизу слева до сверху справа
    bool diagonal_2 = false;    // Вертикаль от снизу справа до сверху слева
    
    [SerializeField]
    int curType;

    GameObject[,] aroundCell = new GameObject[3, 3];

    public float horizontalScale; //размеры дорог
    public float digonalScale;

    Vector2[] points_1 = new Vector2[1];    //точки для движения
    Vector2[] points_2 = new Vector2[1];

    Vector2 dir_1;  //сектора входа поезда
    Vector2 dir_2;

    Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) };


    //    Порядок создания станций:
    //      -> определение возможных типов соединения
    //      -> проверка не выбран ли тип - не выбран, ставим горизонтальный по умолчанию
    //      -> создаем конекты по выбраному типу
    //      -> проверяем на конечность/проточность станции
    //      -> отрисовываем станцию по выбраному типу



    void Start()
    {
        endStation = false;
        Camera.main.GetComponent<RespawnMode>().AddStation(gameObject);
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

    public Vector3 GetAngles()
    {
        if (horizontal)
            return new Vector3(0, 0, 0);
        else if (vertical)
            return new Vector3(0, 0, 90);
        else if (diagonal_1)
            return new Vector3(0, 0, 45);
        else
            return new Vector3(0, 0, 135);
    }


    public void SetType()       // определение возможных типов соединений
    {
        horizontal = false;     // обнуляем все существующие соединенеия
        vertical = false;
        diagonal_1 = false;
        diagonal_2 = false;


        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x + i, (int)transform.position.y + k);


        if (aroundCell[2, 1] || aroundCell[0, 1])   // Горизонтальное соединение
        {
            if (aroundCell[2, 1] && aroundCell[2, 1].GetComponent<RailwayScript>() && aroundCell[2, 1].GetComponent<RailwayScript>().IsConect(new Vector2(1, 0)))
                horizontal = true;
            else if (aroundCell[0, 1] && aroundCell[0, 1].GetComponent<RailwayScript>() && aroundCell[0, 1].GetComponent<RailwayScript>().IsConect(new Vector2(-1,0)))
                horizontal = true;
        }

        if (aroundCell[1, 2] || aroundCell[1, 0])   // Вертикальное соединение
        {
            if (aroundCell[1, 2] && aroundCell[1, 2].GetComponent<RailwayScript>() && aroundCell[1, 2].GetComponent<RailwayScript>().IsConect(new Vector2(0,1)))
                vertical = true;
            else if (aroundCell[1, 0] && aroundCell[1, 0].GetComponent<RailwayScript>() && aroundCell[1, 0].GetComponent<RailwayScript>().IsConect(new Vector2(0, -1)))
                vertical = true;
        }

        if (aroundCell[2, 2] || aroundCell[0, 0])   // Вертикаль первая
        {
            if (aroundCell[2, 2] && aroundCell[2, 2].GetComponent<RailwayScript>() && aroundCell[2, 2].GetComponent<RailwayScript>().IsConect(new Vector2(1, 1)))
                diagonal_1 = true;
            else if (aroundCell[0, 0] && aroundCell[0, 0].GetComponent<RailwayScript>() && aroundCell[0, 0].GetComponent<RailwayScript>().IsConect(new Vector2(-1, -1)))
                diagonal_1 = true;
        }

        if (aroundCell[0, 2] || aroundCell[2, 0])   // Вертикаль вторая
        {
            if (aroundCell[0, 2] && aroundCell[0, 2].GetComponent<RailwayScript>() && aroundCell[0, 2].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 1)))
                diagonal_2 = true;
            else if (aroundCell[2, 0] && aroundCell[2, 0].GetComponent<RailwayScript>() && aroundCell[2, 0].GetComponent<RailwayScript>().IsConect(new Vector2(1, -1)))
                diagonal_2 = true;
        }

        
        SelectCurType();
    }

    void SelectCurType()        // Задаем текущий тип
    {
        switch (curType)      // Если тип выбран, но он не соответствует одному из возможных в данный момент типов    
        {
            case 1:
                if (!horizontal)
                    curType = 0;
                break;

            case 2:
                if (!vertical)
                    curType = 0;
                break;

            case 3:
                if (!diagonal_1)
                    curType = 0;
                break;

            case 4:
                if (!diagonal_2)
                    curType = 0;
                break;
        }

        

        if (curType == 0)         // Если тип не выбран
        {
            if (horizontal)
                curType = 1;
            else if (vertical)
                curType = 2;
            else if (diagonal_1)
                curType = 3;
            else if (diagonal_2)
                curType = 4;
        }

        

        SetConect();
    }

    public void SetConect()     // создание конекта с соседними дорогами
    {
        endStation = false;
        GetComponent<RailwayScript>().DeleteConects();

        switch (curType)
        {
            case 1:
                if (aroundCell[2, 1] && aroundCell[2, 1].GetComponent<RailwayScript>()) // Соединение справа
                {
                    if (aroundCell[2, 1].GetComponent<RailwayScript>().IsConect(new Vector2(1, 0)))
                    {
                        endStation = true;
                        horizontal = true;
                    }
                }
                if (aroundCell[0, 1] && aroundCell[0, 1].GetComponent<RailwayScript>()) // Соединение слева
                {
                    if (aroundCell[0, 1].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 0)))
                    {
                        endStation = !endStation;
                        horizontal = true;
                    }
                }
                break;

            case 2:
                if (aroundCell[1, 2] && aroundCell[1, 2].GetComponent<RailwayScript>())
                {
                    if (aroundCell[1, 2].GetComponent<RailwayScript>().IsConect(new Vector2(0, 1)))
                    {
                        endStation = true;
                        vertical = true;
                    }
                }
                if (aroundCell[1, 0] && aroundCell[1, 0].GetComponent<RailwayScript>())
                {
                    if (aroundCell[1, 0].GetComponent<RailwayScript>().IsConect(new Vector2(0, -1)))
                    {
                        endStation = !endStation;
                        vertical = true;
                    }
                }
                break;

            case 3:
                if (aroundCell[2, 2] && aroundCell[2, 2].GetComponent<RailwayScript>())
                {
                    if (aroundCell[2, 2].GetComponent<RailwayScript>().IsConect(new Vector2(1, 1)))
                    {
                        endStation = true;
                        diagonal_1 = true;
                    }
                }
                if (aroundCell[0, 0] && aroundCell[0, 0].GetComponent<RailwayScript>())
                {
                    if (aroundCell[0, 0].GetComponent<RailwayScript>().IsConect(new Vector2(-1, -1)))
                    {
                        endStation = !endStation;
                        diagonal_1 = true;
                    }
                }
                break;

            case 4:
                if (aroundCell[0, 2] && aroundCell[0, 2].GetComponent<RailwayScript>())
                {
                    if (aroundCell[0, 2].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 1)))
                    {
                        endStation = true;
                        diagonal_2 = true;
                    }
                }
                if (aroundCell[2, 0] && aroundCell[2, 0].GetComponent<RailwayScript>())
                {
                    if (aroundCell[2, 0].GetComponent<RailwayScript>().IsConect(new Vector2(1, -1)))
                    {
                        endStation = !endStation;
                        diagonal_2 = true;
                    }
                }
                break;
        }

        DrawRailway();
    }

    void DrawRailway()          // отрисовка дороги
    {
        switch (curType)
        {
            case 0:
                GameObject temp_ = transform.GetChild(0).gameObject;
                temp_.transform.localScale = new Vector3(0, temp_.transform.localScale.y, temp_.transform.localScale.z);
                break;

            case 1:
                if (endStation)
                {
                    GameObject leftCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x - 1, (int)transform.position.y);

                    if (leftCell && leftCell.GetComponent<RailwayScript>() && leftCell.GetComponent<RailwayScript>().IsConect(new Vector2(-1, 0)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
                        temp.transform.localScale = new Vector3(horizontalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(-horizontalScale / 4, 0, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
                        temp.transform.localScale = new Vector3(horizontalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(horizontalScale / 4, 0, 0);
                    }
                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 0);
                    temp.transform.localScale = new Vector3(horizontalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                PointsHorizontal();
                break;

            case 2:
                if (endStation)
                {
                    GameObject downCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x, (int)transform.position.y - 1);

                    if (downCell && downCell.GetComponent<RailwayScript>() && downCell.GetComponent<RailwayScript>().IsConect(new Vector2(0, -1)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 90);
                        temp.transform.localScale = new Vector3(horizontalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(0, -horizontalScale / 4, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 90);
                        temp.transform.localScale = new Vector3(horizontalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(0, horizontalScale / 4, 0);
                    }
                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 90);
                    temp.transform.localScale = new Vector3(horizontalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                PointsVertical();
                break;

            case 3:
                if (endStation)
                {
                    GameObject downLedtCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x - 1, (int)transform.position.y - 1);

                    if (downLedtCell && downLedtCell.GetComponent<RailwayScript>() && downLedtCell.GetComponent<RailwayScript>().IsConect(new Vector2(-1, -1)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 45);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(-horizontalScale / 4, -horizontalScale / 4, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 45);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(horizontalScale / 4, +horizontalScale / 4, 0);
                    }
                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 45);
                    temp.transform.localScale = new Vector3(digonalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                PointsDiagonal_1();
                break;

            case 4:
                if (endStation)
                {
                    GameObject upLedtCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x - 1, (int)transform.position.y + 1);

                    if (upLedtCell && upLedtCell.GetComponent<RailwayScript>() && upLedtCell.GetComponent<RailwayScript>().IsConect(new Vector2(-1, 1)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 135);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(-horizontalScale / 4, horizontalScale / 4, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 135);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(horizontalScale / 4, -horizontalScale / 4, 0);
                    }

                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 135);
                    temp.transform.localScale = new Vector3(digonalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                PointsDiagonal_2();
                break;
        }
    }


    void PointsHorizontal()
    {
        points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y);
        points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y);

        dir_1 = new Vector2(1, 0);
        dir_2 = new Vector2(-1, 0);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    void PointsVertical()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y - 0.501f);
        points_2[0] = new Vector2(transform.position.x, transform.position.y + 0.501f);

        dir_1 = new Vector2(0, 1);
        dir_2 = new Vector2(0, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    void PointsDiagonal_1()
    {
        points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y - 0.501f);
        points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y + 0.501f);

        dir_1 = new Vector2(1, 1);
        dir_2 = new Vector2(-1, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    void PointsDiagonal_2()
    {
        points_1[0] = new Vector2(transform.position.x + 0.501f, transform.position.y - 0.501f);
        points_2[0] = new Vector2(transform.position.x - 0.501f, transform.position.y + 0.501f);

        dir_1 = new Vector2(-1, 1);
        dir_2 = new Vector2(1, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }


    public void SetColor(Color color)
    {
        transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
    }
}