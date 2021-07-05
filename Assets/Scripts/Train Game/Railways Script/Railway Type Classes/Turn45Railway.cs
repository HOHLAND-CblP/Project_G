using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn45Railway : MonoBehaviour
{
    // Массивы точек для движения по данному пути в разных направлениях
    private Vector2[] points_1 = new Vector2[6];
    private Vector2[] points_2 = new Vector2[6];

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
    Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) };



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



    public void BuildRailway()
    {
        MakeNewPoints();
    }


    void MakeNewPoints()
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


    public void SetDeadEnd()
    {
        GameObject temp = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(transform.position.x + dir_1.x), Mathf.RoundToInt(transform.position.y + dir_1.y));

        if (temp == null || temp.GetComponent<RailwayScript>() == null || !temp.GetComponent<RailwayScript>().IsConect(dir_1))
        {
            deadEnd_1.SetActive(true);
            fDeadEnd_1 = 0.5f;
        }
        else
        {
            deadEnd_1.SetActive(false);
            fDeadEnd_1 = 0.501f;
        }


        temp = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(transform.position.x + dir_2.x), Mathf.RoundToInt(transform.position.y + dir_2.y));

        if (temp == null || temp.GetComponent<RailwayScript>() == null || !temp.GetComponent<RailwayScript>().IsConect(dir_2))
        {
            deadEnd_2.SetActive(true);
            fDeadEnd_2 = 0.5f;
        }
        else
        {
            deadEnd_2.SetActive(false);
            fDeadEnd_2 = 0.501f;
        }
    }


    void Turn0Points()
    {
        points_1[0] = new Vector2(transform.position.x - 0.1f, transform.position.y);
        points_1[1] = new Vector2(transform.position.x - 0.05f, transform.position.y + transform.localScale.y * 0.005f);
        points_1[2] = new Vector2(transform.position.x + 0.01f, transform.position.y + transform.localScale.y * 0.033f);
        points_1[3] = new Vector2(transform.position.x + 0.05f, transform.position.y + transform.localScale.y * 0.06f);
        points_1[4] = new Vector2(transform.position.x + 0.092f, transform.position.y + transform.localScale.y * 0.092f);
        points_1[5] = new Vector2(transform.position.x + fDeadEnd_2, transform.position.y + transform.localScale.y * fDeadEnd_2);

        points_2[0] = new Vector2(transform.position.x + 0.092f, transform.position.y + transform.localScale.y * 0.092f);
        points_2[1] = new Vector2(transform.position.x + 0.05f, transform.position.y + transform.localScale.y * 0.06f);
        points_2[2] = new Vector2(transform.position.x + 0.01f, transform.position.y + transform.localScale.y * 0.033f);
        points_2[3] = new Vector2(transform.position.x - 0.05f, transform.position.y + transform.localScale.y * 0.005f);
        points_2[4] = new Vector2(transform.position.x - 0.1f, transform.position.y);
        points_2[5] = new Vector2(transform.position.x - fDeadEnd_1, transform.position.y);
    }
    void Turn90Points()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y - 0.1f);
        points_1[1] = new Vector2(transform.position.x - transform.localScale.y * 0.005f, transform.position.y - 0.05f);
        points_1[2] = new Vector2(transform.position.x - transform.localScale.y * 0.033f, transform.position.y + 0.01f);
        points_1[3] = new Vector2(transform.position.x - transform.localScale.y * 0.06f, transform.position.y + 0.05f);
        points_1[4] = new Vector2(transform.position.x - transform.localScale.y * 0.092f, transform.position.y + 0.092f);
        points_1[5] = new Vector2(transform.position.x - transform.localScale.y * fDeadEnd_2, transform.position.y + fDeadEnd_2);

        points_2[0] = new Vector2(transform.position.x - transform.localScale.y * 0.092f, transform.position.y + 0.092f);
        points_2[1] = new Vector2(transform.position.x - transform.localScale.y * 0.06f, transform.position.y + 0.05f);
        points_2[2] = new Vector2(transform.position.x - transform.localScale.y * 0.033f, transform.position.y + 0.01f);
        points_2[3] = new Vector2(transform.position.x - transform.localScale.y * 0.005f, transform.position.y - 0.05f);
        points_2[4] = new Vector2(transform.position.x, transform.position.y - 0.1f);
        points_2[5] = new Vector2(transform.position.x, transform.position.y - fDeadEnd_1);
    }
    void Turn180Points()
    {
        points_1[0] = new Vector2(transform.position.x + 0.1f, transform.position.y);
        points_1[1] = new Vector2(transform.position.x + 0.05f, transform.position.y - transform.localScale.y * 0.005f);
        points_1[2] = new Vector2(transform.position.x - 0.01f, transform.position.y - transform.localScale.y * 0.033f);
        points_1[3] = new Vector2(transform.position.x - 0.05f, transform.position.y - transform.localScale.y * 0.06f);
        points_1[4] = new Vector2(transform.position.x - 0.092f, transform.position.y - transform.localScale.y * 0.092f);
        points_1[5] = new Vector2(transform.position.x - fDeadEnd_2, transform.position.y - transform.localScale.y * fDeadEnd_2);

        points_2[0] = new Vector2(transform.position.x - 0.092f, transform.position.y - transform.localScale.y * 0.092f);
        points_2[1] = new Vector2(transform.position.x - 0.05f, transform.position.y - transform.localScale.y * 0.06f);
        points_2[2] = new Vector2(transform.position.x - 0.01f, transform.position.y - transform.localScale.y * 0.033f);
        points_2[3] = new Vector2(transform.position.x + 0.05f, transform.position.y - transform.localScale.y * 0.005f);
        points_2[4] = new Vector2(transform.position.x + 0.1f, transform.position.y);
        points_2[5] = new Vector2(transform.position.x + fDeadEnd_1, transform.position.y);
    }
    void Turn270Points()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y + 0.1f);
        points_1[1] = new Vector2(transform.position.x + transform.localScale.y * 0.005f, transform.position.y + 0.05f);
        points_1[2] = new Vector2(transform.position.x + transform.localScale.y * 0.033f, transform.position.y - 0.01f);
        points_1[3] = new Vector2(transform.position.x + transform.localScale.y * 0.06f, transform.position.y - 0.05f);
        points_1[4] = new Vector2(transform.position.x + transform.localScale.y * 0.092f, transform.position.y - 0.092f);
        points_1[5] = new Vector2(transform.position.x + transform.localScale.y * fDeadEnd_2, transform.position.y - fDeadEnd_2);

        points_2[0] = new Vector2(transform.position.x + transform.localScale.y * 0.092f, transform.position.y - 0.092f);
        points_2[1] = new Vector2(transform.position.x + transform.localScale.y * 0.06f, transform.position.y - 0.05f);
        points_2[2] = new Vector2(transform.position.x + transform.localScale.y * 0.033f, transform.position.y - 0.01f);
        points_2[3] = new Vector2(transform.position.x + transform.localScale.y * 0.005f, transform.position.y + 0.05f);
        points_2[4] = new Vector2(transform.position.x, transform.position.y + 0.1f);
        points_2[5] = new Vector2(transform.position.x, transform.position.y + fDeadEnd_1);
    }


    void Turn0Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(-1, 0);
        dir_2 = new Vector2(1, 1 * transform.localScale.y);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Turn90Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(0, -1);
        dir_2 = new Vector2(-1 * transform.localScale.y, 1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Turn180Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(1, 0);
        dir_2 = new Vector2(-1, -1 * transform.localScale.y);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void Turn270Conects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(0, 1);
        dir_2 = new Vector2(1 * transform.localScale.y, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    public void Turn(int k)
    {
        if (k == 1)
            if (transform.localScale.y == -1)
            {
                transform.Rotate(0, 0, 90 * -k);
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
            else
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        else
        if (transform.localScale.y == 1)
        {
            transform.Rotate(0, 0, 90 * -k);
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
        else
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

    }
}