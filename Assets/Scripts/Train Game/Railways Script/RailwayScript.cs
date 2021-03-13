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

    public Roads roads; // выбранный тип железной дороги

 
    public enum Roads   //типы ж/д путей
    {
        Unselected = 0,
        Straight = 1,
        Turn90 = 2,
        Turn45 = 3,
        Arrow = 4,
        Station = 5
    }


    private void Awake()
    {
        Camera.main.GetComponent<RailwayControler>().AddRailway(this);
    }


    public Vector2[] ActivateRailway(Vector2 dir, int rangRailwayCarriage, Vector2[] points) //Активируем клетку железной дороги
    {
        switch (roads)  // Находим нужный тип дороги
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
        }
        return vectorError;
    }


    public Vector2[] ChangeDirection(Vector2[] points, Vector2 trainPos)
    {
        switch (roads)
        {
            case Roads.Turn90: //поворот на 90 градусов
                return GetComponent<Turn90Railway>().ChangeDirection(points);

            case Roads.Turn45: //поворот на 45 градусов
                return GetComponent<Turn45Railway>().ChangeDirection(points);

          case Roads.Arrow: // стрелка
                return GetComponent<ArrowRailway>().ChangeDirection(points, trainPos);

            case Roads.Station: //станция
                 return GetComponent<Station>().ChangeDirection(points);

            case Roads.Straight:
                 return GetComponent<StraightRailway>().ChangeDirection(points);
        }

        return vectorError;
    }


    public void SetConect() // Создаем визувльный конект
    {
        switch (roads)
        {
            case Roads.Straight:
                GetComponent<StraightRailway>().SetDeadEnd();
                break;

            case Roads.Turn90:
                GetComponent<Turn90Railway>().SetDeadEnd();
                break;

            case Roads.Turn45: 
                break;

            case Roads.Arrow: 
                break;

            case Roads.Station:
                GetComponent<Station>().SetType();
                break;
        }
    }


    public void UpdateCellsAround()
    {
        GameObject[,] aroundCell = new GameObject[3, 3];

        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
            {

                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x + i, (int)transform.position.y + k);
                if (i != 0 || k != 0)
                {
                    if (aroundCell[i + 1, k + 1] && aroundCell[i + 1, k + 1].GetComponent<RailwayScript>())
                        aroundCell[i + 1, k + 1].GetComponent<RailwayScript>().SetConect();
                } 
            }
    }



    void MakeNewPoints()
    {
        switch (roads)
        {
            case Roads.Straight:
                GetComponent<StraightRailway>().MakeNewPoints();
                break;

            case Roads.Turn90:
                GetComponent<Turn90Railway>().MakeNewPoints();
                break;

            case Roads.Turn45:
                GetComponent<Turn45Railway>().MakeNewPoints();
                break;

            case Roads.Arrow:
                GetComponent<ArrowRailway>().MakeNewPoints();
                break;
        }
    }



    public void Turn(int k) // k - направление в котором надо поворачивать объект
    {
        switch (roads)
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
        }
    }



    public void RailwayIsBuid()
    {
        MakeNewPoints();
        SetConect();
        Camera.main.GetComponent<RailwayControler>().UpdateCellsAround((int)transform.position.x, (int)transform.position.y);
        SetConect();
    }


    public void DeleteRailway()
    {
        switch (roads)
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