using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightRailway : MonoBehaviour
{
    [SerializeField]
    private  Vector2[] points_1 = new Vector2[1];
    [SerializeField]
    private Vector2[] points_2 = new Vector2[1];

    public float horizontalScale;
    public float diagonalScale;

    Vector2 dir_1;
    Vector2 dir_2;

    public GameObject deadEnd_1;
    public GameObject deadEnd_2;
    public GameObject railwaySprite;


    Quaternion t0 = Quaternion.Euler(0, 0, 0);
    Quaternion t45 = Quaternion.Euler(0, 0, 45);
    Quaternion t90 = Quaternion.Euler(0, 0, 90);
    Quaternion t135 = Quaternion.Euler(0, 0, 135);


    Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) };


    void Awake()
    {
        BuildRailway();
    }


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
        ResizeRotation();
        MakeNewPoints();
        ResizeRailway();
    }


    // Изменение поворота к нормальному виду
    void ResizeRotation()
    {
        if (transform.rotation.eulerAngles.z >= 180)
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 180);

        if (transform.rotation.eulerAngles.z <= -180)
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 180);
    }

    // Создание точек и конектов
    void MakeNewPoints()
    {
        switch (Mathf.RoundToInt(transform.rotation.eulerAngles.z))
        {
            case 0:
                HorizontalPoints();
                HorizontalConects();
                break;
            case 45:
                FirstDiagonalPoints();
                FirstDiagonalConects();
                break;
            case 90:
                VerticalPoints();
                VerticalConects();
                break;
            case 135:
                SecondDiagonalPoints();
                SecondDiagonalConects();
                break;
        }
    }

    // Создание конектов
    public void MakeNewConects()
    {
        ResizeRotation();

        switch (Mathf.RoundToInt(transform.rotation.eulerAngles.z))
        {
            case 0:
                HorizontalConects();
                break;
            case 45:
                FirstDiagonalConects();
                break;
            case 90:
                VerticalConects();
                break;
            case 135:
                SecondDiagonalConects();
                break;
        }
    }

    // Изменение длины дороги 
    void ResizeRailway()
    {
        switch (Mathf.RoundToInt(transform.rotation.eulerAngles.z))
        {
            case 0:
                railwaySprite.transform.localScale = new Vector3(horizontalScale, railwaySprite.transform.localScale.y, railwaySprite.transform.localScale.z);
                break;
            case 45:
                railwaySprite.transform.localScale = new Vector3(diagonalScale, railwaySprite.transform.localScale.y, railwaySprite.transform.localScale.z);
                break;
            case 90:
                railwaySprite.transform.localScale = new Vector3(horizontalScale, railwaySprite.transform.localScale.y, railwaySprite.transform.localScale.z);
                break;
            case 135:
                railwaySprite.transform.localScale = new Vector3(diagonalScale, railwaySprite.transform.localScale.y, railwaySprite.transform.localScale.z);
                break;
        }
    }

    
    // Установка тупиков
    public void SetDeadEnd()
    {
        GameObject temp = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(transform.position.x + dir_1.x), Mathf.RoundToInt(transform.position.y + dir_1.y));

        if (temp == null || temp.GetComponent<RailwayScript>() == null || !temp.GetComponent<RailwayScript>().IsConect(dir_1))
        {
            if (Mathf.Round(transform.rotation.eulerAngles.z) % 10 == 5)
                deadEnd_1.transform.localPosition = new Vector3(0.21f, 0, 0);
            else
                deadEnd_1.transform.localPosition = new Vector3(0, 0, 0);

            deadEnd_1.SetActive(true);
        }
        else
        {
            deadEnd_1.SetActive(false);
        }


        temp = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(transform.position.x + dir_2.x), Mathf.RoundToInt(transform.position.y + dir_2.y));

        if (temp == null || temp.GetComponent<RailwayScript>() == null || !temp.GetComponent<RailwayScript>().IsConect(dir_2))
        {
            if (Mathf.Round(transform.rotation.eulerAngles.z) % 10 == 5)
                deadEnd_2.transform.localPosition = new Vector3(-0.21f, 0, 0);
            else
                deadEnd_2.transform.localPosition = new Vector3(0, 0, 0);

            deadEnd_2.SetActive(true);
        }
        else
        {
            deadEnd_2.SetActive(false);
        }
    }



    void HorizontalPoints()
    {
        GetComponent<RailwayScript>().DeleteConects();

        points_1[0] = new Vector2(-0.501f + transform.position.x, transform.position.y);
        points_2[0] = new Vector2(0.501f + transform.position.x, transform.position.y);

        dir_1 = new Vector2(1, 0);
        dir_2 = new Vector2(-1, 0);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void VerticalPoints()
    {
        GetComponent<RailwayScript>().DeleteConects();

        points_1[0] = new Vector2(transform.position.x, -0.501f + transform.position.y);
        points_2[0] = new Vector2(transform.position.x, 0.501f + transform.position.y);

        dir_1 = new Vector2(0, 1);
        dir_2 = new Vector2(0, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void FirstDiagonalPoints()
    {
        GetComponent<RailwayScript>().DeleteConects();

        points_1[0] = new Vector2(-0.501f + transform.position.x, -0.501f + transform.position.y);
        points_2[0] = new Vector2(0.501f + transform.position.x, 0.501f + transform.position.y);

        dir_1 = new Vector2(1, 1);
        dir_2 = new Vector2(-1, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void SecondDiagonalPoints()
    {
        GetComponent<RailwayScript>().DeleteConects();

        points_1[0] = new Vector2(0.501f + transform.position.x, -0.501f + transform.position.y);
        points_2[0] = new Vector2(-0.501f + transform.position.x, 0.501f + transform.position.y);

        dir_1 = new Vector2(-1, 1);
        dir_2 = new Vector2(1, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }


    void HorizontalConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(1, 0);
        dir_2 = new Vector2(-1, 0);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void VerticalConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(0, 1);
        dir_2 = new Vector2(0, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void FirstDiagonalConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(1, 1);
        dir_2 = new Vector2(-1, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }
    void SecondDiagonalConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        dir_1 = new Vector2(-1, 1);
        dir_2 = new Vector2(1, -1);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    public void Turn(int k)
    {
        transform.Rotate(0, 0, 45 * -k);

        if (Mathf.Round(transform.rotation.eulerAngles.z) % 10 == 5)
            railwaySprite.transform.localScale = new Vector3(diagonalScale, transform.localScale.y, transform.localScale.z);
        else
            railwaySprite.transform.localScale = new Vector3(horizontalScale, transform.localScale.y, transform.localScale.z);
    }
}
