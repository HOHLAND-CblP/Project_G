using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RailwayScript : MonoBehaviour
{
    [SerializeField]
    List<Vector2> conects;

    Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) }; // вектор который выдается в случае ошибки
    /*
    система конектов: каждая клеточка разбивается на 9 одинковых секторов, каждому из которых присваивается координата Vector2, где отсчет начинается от центрального 
    сектора с координатой (0,0). С помощью этих секторов станции конетктятся с соседними ж/д путями, а также определяют местоположение поезда, запрашивающего точки. Например, сектор с коодинатами (-1,0) 
    конектится с левым от него ж/д путем, а сектор (1,1) - с верхним правым, идущем по диагонали. По сути координаты показывают направление, по которому идет ж/д путь, к которому можно приконектиться.


            (0,1)     
            _ _ _
     (-1,1)|_|_|_|(1,1)
     (-1,0)|_|_|_|(1,0)
    (-1,-1)|_|_|_|(1,-1)
            (0,-1)

    */

    public Roads road; // выбранный тип железной дороги


    public enum Roads   //типы ж/д путей
    {
        Unselected = 0,
        Straight = 1,
        Turn90 = 2,
        Turn45 = 3,
        Arrow = 4,
        Station = 5,
        Factory = 6
    }


    private void Awake()
    {
        Camera.main.GetComponent<RailwayControler>().AddRailway(this);
    }


    public Vector2[] ActivateRailway(Vector2 dir, int rangRailwayCarriage, Vector2[] points) //Активируем клетку железной дороги
    {
        switch (road)  // Находим нужный тип дороги
        {
            case Roads.Turn90: //поворот на 90 градусов
                return GetComponent<Turn90Railway>().GetPoints(dir);

            case Roads.Turn45: //поворот на 45 градусов
                return GetComponent<Turn45Railway>().GetPoints(dir);

            case Roads.Arrow: // стрелка
                return GetComponent<ArrowRailway>().GetPoints(dir, rangRailwayCarriage, points);
                
            case Roads.Station: //станция
                return GetComponent<Station>().GetPoints(dir);

            case Roads.Straight:
                return GetComponent<StraightRailway>().GetPoints(dir);

            case Roads.Factory:
                return GetComponent<FactoryRailway>().GetPoints(dir);
        }
        return vectorError;
    }

    public Vector2[] ChangeDirection(Vector2[] points, Vector2 trainPos)
    {
        switch (road)
        {
            case Roads.Turn90:  //поворот на 90 градусов
                return GetComponent<Turn90Railway>().ChangeDirection(points);

            case Roads.Turn45:  //поворот на 45 градусов
                return GetComponent<Turn45Railway>().ChangeDirection(points);

            case Roads.Arrow:   // стрелка
                return GetComponent<ArrowRailway>().ChangeDirection(points, trainPos);

            case Roads.Station: //станция
                 return GetComponent<Station>().ChangeDirection(points);

            case Roads.Straight:
                 return GetComponent<StraightRailway>().ChangeDirection(points);

            case Roads.Factory:
                return GetComponent<FactoryRailway>().ChangeDirection(points);
        }

        return vectorError;
    }



    public void BuildRailway()  // Строительство дороги
    {
        _BuildRailway();

        BuildRailwaysAround();
    }

    private void _BuildRailway()
    {
        switch (road)
        {
            case Roads.Straight:
                GetComponent<StraightRailway>().BuildRailway();
                break;

            case Roads.Turn90:
                GetComponent<Turn90Railway>().BuildRailway();
                break;

            case Roads.Turn45:
                GetComponent<Turn45Railway>().BuildRailway();
                break;

            case Roads.Arrow:
                GetComponent<ArrowRailway>().BuildRailway();
                break;

            case Roads.Station:
                GetComponent<Station>().BuildRailway();
                break;

            case Roads.Factory:
                GetComponent<FactoryRailway>().Build();
                GetComponent<Factory>().BuildFactory();
                break;
        }
    }


    public void PreBuildVizualization()     // Предварительное отображение дороги
    {
        MakeNewConects();
        DrawCellsAround();
        Draw();
    }

    public void CancelPreBuildVizual()  // Отмена превизуализации
    {
        DeleteConects();
        DrawCellsAround();
        DrawDefault();
    }


    public void MakeNewConects()
    {
        switch (road)
        {
            case Roads.Straight:
                GetComponent<StraightRailway>().MakeNewConects();
                break;

            case Roads.Turn90:
                GetComponent<Turn90Railway>().MakeNewConects();
                break;

            case Roads.Turn45:
                GetComponent<Turn45Railway>().MakeNewConects();
                break;

            case Roads.Arrow:
                GetComponent<ArrowRailway>().MakeNewConects();
                break;

            case Roads.Station:
                GetComponent<Station>().MakeVisualConects();
                break;

            case Roads.Factory:
                GetComponent<FactoryRailway>().MakeVisualConects();
                break;
        }
    }


    public void Draw() // Обработака визульного вида дороги
    {
        switch (road)
        {
            case Roads.Straight:
                GetComponent<StraightRailway>().SetDeadEnd();
                break;

            case Roads.Turn90:
                GetComponent<Turn90Railway>().SetDeadEnd();
                break;

            case Roads.Turn45:
                GetComponent<Turn45Railway>().SetDeadEnd();
                break;

            case Roads.Arrow:
                GetComponent<ArrowRailway>().SetDeadEnd();
                break;

            case Roads.Station:
                GetComponent<Station>().MakeVisualConects();
                GetComponent<Station>().DrawStation();
                break;

            case Roads.Factory:
                GetComponent<FactoryRailway>().MakeVisualConects();
                GetComponent<FactoryRailway>().Draw();
                break;
        }
    }

    void DrawDefault()
    {
        switch (road)
        {
            case Roads.Straight:
                
                break;

            case Roads.Turn90:
                
                break;

            case Roads.Turn45:

                break;

            case Roads.Arrow:
                break;

            case Roads.Station:
                GetComponent<Station>().DrawDefault();
                break;
        }
    }

    void DrawCellsAround()
    {
        GameObject[,] aroundCell = new GameObject[3, 3];

        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
            {

                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x + i, (int)transform.position.y + k);
                if (i != 0 || k != 0)
                {
                    if (aroundCell[i + 1, k + 1] && aroundCell[i + 1, k + 1].GetComponent<RailwayScript>())
                        aroundCell[i + 1, k + 1].GetComponent<RailwayScript>().Draw();
                }
            }
    }



    void BuildRailwaysAround()
    {
        GameObject[,] aroundCell = new GameObject[3, 3];

        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
            {

                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x + i, (int)transform.position.y + k);
                if (i != 0 || k != 0)
                {
                    if (aroundCell[i + 1, k + 1] && aroundCell[i + 1, k + 1].GetComponent<RailwayScript>())
                        aroundCell[i + 1, k + 1].GetComponent<RailwayScript>()._BuildRailway();
                }
            }
    }

    void UpdateRailwaysAround()
    {
        GameObject[,] aroundCell = new GameObject[3, 3];

        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
            {

                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x + i, (int)transform.position.y + k);
                if (i != 0 || k != 0)
                {
                    if (aroundCell[i + 1, k + 1] && aroundCell[i + 1, k + 1].GetComponent<RailwayScript>())
                    {
                        aroundCell[i + 1, k + 1].GetComponent<RailwayScript>().MakeNewConects();
                        aroundCell[i + 1, k + 1].GetComponent<RailwayScript>().Draw();
                        aroundCell[i + 1, k + 1].GetComponent<RailwayScript>()._BuildRailway();
                    }
                }
            }
    }


    public void Turn(int k) // k - направление в котором надо поворачивать объект
    {
        switch (road)
        {
            case Roads.Straight:
                GetComponent<StraightRailway>().Turn(k);
                break;

            case Roads.Turn90:
                GetComponent<Turn90Railway>().Turn(k);
                break;

            case Roads.Turn45:
                GetComponent<Turn45Railway>().Turn(k);
                break;

            case Roads.Arrow:
                GetComponent<ArrowRailway>().Turn(k);
                break;

            case Roads.Station:
                GetComponent<Station>().Turn(k);
                break;
        }
    }

   



    public void DestroyRailway()
    {
        DrawCellsAround();
        BuildRailwaysAround();
        
        switch (road)
        {
            case Roads.Station:
                Camera.main.GetComponent<RespawnMode>().DeleteStation(gameObject);
                break;
        }
    }


    public void DeleteConects()
    {
        conects.Clear();
    }

    public void AddConect(Vector2 conect)
    {
        conects.Add(conect);
    }

    public Vector2 GetConect(int num)
    {
        return conects[num];
    }

    public bool IsConect(Vector2 conect)
    {
        conect = new Vector2(-conect.x, -conect.y);
        foreach (var c in conects)
        {
            if (c == conect)
                return true;
        }
        return false;
    }
}