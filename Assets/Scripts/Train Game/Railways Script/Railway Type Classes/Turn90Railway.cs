using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn90Railway : MonoBehaviour
{
    // Массивы точек для движения по данному пути в разных направлениях
    [SerializeField]
    private Vector2[] points_1 = new Vector2[7];    // В первом направлении 
    [SerializeField]
    private Vector2[] points_2 = new Vector2[7];    // Во втором направлении

    // Направления
    Vector2 dir_1;
    Vector2 dir_2;

    // Добавочная длина
    float fDeadEnd_1;
    float fDeadEnd_2;

    [Header("Dead Ends")]
    public GameObject deadEnd_1;
    public GameObject deadEnd_2;



    // Вектор ошибки
    Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) }; // Возвращается в случае непредвиденных обстоятельств, например, попал не в тот сектор жд дороги



    public Vector2[] GetPoints(Vector2 dir)
    {
        if (dir == dir_1)
            return points_1;
        else if (dir == dir_2)
            return points_2;
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

    // Строительство дороги
    public void BuildRailway()
    {
        MakeNewPoints();
    }

    // Создание точек и конектов
    public void MakeNewPoints()
    {
        switch (transform.rotation.eulerAngles.z)
        {
            case 0:
                Turn0Points();
                Turn0Conects();
                break;
            case 90:
                Turn90Points();
                Turn90Conects();
                break;
            case 180:
                Turn180Points();
                Turn180Conects();
                break;
            case 270:
                Turn270Points();
                Turn270Conects();
                break;
        }
    }

    // Создание конектов
    public void MakeNewConects()
    {
        switch (transform.rotation.eulerAngles.z)
        {
            case 0:
                Turn0Conects();
                break;
            case 90:
                Turn90Conects();
                break;
            case 180:
                Turn180Conects();
                break;
            case 270:
                Turn270Conects();
                break;
        }
    }


    // Установка тупиков
    public void SetDeadEnd()
    {
        GameObject temp = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(transform.position.x + dir_1.x), Mathf.RoundToInt(transform.position.y + dir_1.y));

        if (temp == null || temp.GetComponent<RailwayScript>() == null || !temp.GetComponent<RailwayScript>().IsConect(dir_1))
        {
            deadEnd_1.SetActive(true);
            fDeadEnd_1 = 0;
        }
        else
        {
            deadEnd_1.SetActive(false);
            fDeadEnd_1 = 0.001f;
        }


        temp = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(transform.position.x + dir_2.x), Mathf.RoundToInt(transform.position.y + dir_2.y));

        if (temp == null || temp.GetComponent<RailwayScript>() == null || !temp.GetComponent<RailwayScript>().IsConect(dir_2))
        {
            deadEnd_2.SetActive(true);
            fDeadEnd_2 = 0;
        }
        else
        {
            deadEnd_2.SetActive(false);
            fDeadEnd_2 = 0.001f;
        }
    }


    

    void Turn0Points()
    {
        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf =  84 - i * 14; //вычисляем текущий угол
            points_1[i] = new Vector2(0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x - 0.5f, 0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y - 0.5f);

            if (i == 6)
            {
                points_1[i] = new Vector2(points_1[i].x , points_1[i].y - fDeadEnd_2);
            }
        }

        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf = i*14 + 6; //вычисляем текущий угол
            points_2[i] = new Vector2( 0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x -  0.5f, 0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y - 0.5f);

            if (i == 6)
            {
                points_2[i] = new Vector2(points_2[i].x - fDeadEnd_1, points_2[i].y);
            }
        }
    }
    void Turn90Points()
    {
        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf = i * 14 + 6; //вычисляем текущий угол
            points_1[i] = new Vector2(-0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x + 0.5f, 0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y - 0.5f);

            if (i == 6)
            {
                points_1[i] = new Vector2(points_1[i].x + fDeadEnd_2, points_1[i].y);
            }
        }

        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf = 84 - i * 14; //вычисляем текущий угол
            points_2[i] = new Vector2(-0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x + 0.5f, 0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y - 0.5f);

            if (i == 6)
            {
                points_2[i] = new Vector2(points_2[i].x, points_2[i].y - fDeadEnd_1);
            }
        }        
    }
    void Turn180Points()
    {
        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf = 84 - i * 14; //вычисляем текущий угол
            points_1[i] = new Vector2(-0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x + 0.5f, -0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y + 0.5f);
            
            if (i == 6)
            {
                points_1[i] = new Vector2(points_1[i].x, points_1[i].y + fDeadEnd_2);
            }
        }

        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf = i * 14 + 6; //вычисляем текущий угол
            points_2[i] = new Vector2(-0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x + 0.5f, - 0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y + 0.5f);

            if (i == 6)
            {
                points_2[i] = new Vector2(points_2[i].x + fDeadEnd_1, points_2[i].y);
            }
        }
    }
    void Turn270Points()
    {
        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf = i * 14 + 6; //вычисляем текущий угол
            points_1[i] = new Vector2(0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x - 0.5f, -0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y + 0.5f);

            if (i == 6)
            {
                points_1[i] = new Vector2(points_1[i].x - fDeadEnd_2, points_1[i].y);
            }
        }

        for (int i = 0; i < 7; i++) //создаем точки по которым двигается поезд
        {
            int alf = 84 - i * 14; //вычисляем текущий угол
            points_2[i] = new Vector2(0.5f * Mathf.Cos((alf) * Mathf.PI / 180) + transform.position.x - 0.5f, -0.5f * Mathf.Sin((alf) * Mathf.PI / 180) + transform.position.y + 0.5f);

            if (i == 6)
            {
                points_2[i] = new Vector2(points_2[i].x, points_2[i].y + fDeadEnd_1);
            }
        }
    }


    void Turn0Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(-1, 0);
        dir_2 = new Vector2(0, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Turn90Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(0, -1);
        dir_2 = new Vector2(1, 0);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Turn180Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(1, 0);
        dir_2 = new Vector2(0, 1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Turn270Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(0, 1);
        dir_2 = new Vector2(-1, 0);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }



    // Поворот объекта по оси Z (доступно при строительстве/модификации)
    public void Turn(int k)
    {
        transform.Rotate(0, 0, 90 * -k);
    }

}