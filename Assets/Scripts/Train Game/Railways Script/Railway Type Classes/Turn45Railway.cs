using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn45Railway : MonoBehaviour
{
    private Vector2[] points_1 = new Vector2[6];
    private Vector2[] points_2 = new Vector2[6];

    Vector2 dir_1;
    Vector2 dir_2;

    Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) };

    void Awake()
    {
        MakeNewPoints();
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


    public void MakeNewPoints()
    {
        GetComponent<RailwayScript>().DeleteConects();


        switch (transform.rotation.eulerAngles.z)
        {
            case 0:
                Turn0();
                break;
            case 90:
                Turn90();
                break;
            case 180:
                Turn180();
                break;
            case 270:
                Turn270();
                break;
        }
    }

    void Turn0()
    {
        points_1[0] = new Vector2(transform.position.x - 0.1f, transform.position.y);
        points_1[1] = new Vector2(transform.position.x - 0.05f, transform.position.y + transform.localScale.y * 0.005f);
        points_1[2] = new Vector2(transform.position.x + 0.01f, transform.position.y + transform.localScale.y * 0.033f);
        points_1[3] = new Vector2(transform.position.x + 0.05f, transform.position.y + transform.localScale.y * 0.06f);
        points_1[4] = new Vector2(transform.position.x + 0.092f, transform.position.y + transform.localScale.y * 0.092f);
        points_1[5] = new Vector2(transform.position.x + 0.501f, transform.position.y + transform.localScale.y * 0.501f);

        points_2[0] = new Vector2(transform.position.x + 0.092f, transform.position.y + transform.localScale.y * 0.092f);
        points_2[1] = new Vector2(transform.position.x + 0.05f, transform.position.y + transform.localScale.y * 0.06f);
        points_2[2] = new Vector2(transform.position.x + 0.01f, transform.position.y + transform.localScale.y * 0.033f);
        points_2[3] = new Vector2(transform.position.x - 0.05f, transform.position.y + transform.localScale.y * 0.005f);
        points_2[4] = new Vector2(transform.position.x - 0.1f, transform.position.y);
        points_2[5] = new Vector2(transform.position.x - 0.501f, transform.position.y);

        dir_1 = new Vector2(-1, 0);
        dir_2 = new Vector2(1 , 1 * transform.localScale.y);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    void Turn90()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y - 0.1f);
        points_1[1] = new Vector2(transform.position.x - transform.localScale.y * 0.005f, transform.position.y - 0.05f);
        points_1[2] = new Vector2(transform.position.x - transform.localScale.y * 0.033f, transform.position.y + 0.01f);
        points_1[3] = new Vector2(transform.position.x - transform.localScale.y * 0.06f, transform.position.y + 0.05f);
        points_1[4] = new Vector2(transform.position.x - transform.localScale.y * 0.092f, transform.position.y + 0.092f);
        points_1[5] = new Vector2(transform.position.x - transform.localScale.y * 0.501f, transform.position.y + 0.501f);

        points_2[0] = new Vector2(transform.position.x - transform.localScale.y * 0.092f, transform.position.y + 0.092f);
        points_2[1] = new Vector2(transform.position.x - transform.localScale.y * 0.06f, transform.position.y + 0.05f);
        points_2[2] = new Vector2(transform.position.x - transform.localScale.y * 0.033f, transform.position.y + 0.01f);
        points_2[3] = new Vector2(transform.position.x - transform.localScale.y * 0.005f, transform.position.y - 0.05f);
        points_2[4] = new Vector2(transform.position.x, transform.position.y - 0.1f);
        points_2[5] = new Vector2(transform.position.x, transform.position.y - 0.501f);

        dir_1 = new Vector2(0, -1);
        dir_2 = new Vector2(-1 * transform.localScale.y, 1 );

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    void Turn180()
    {
        points_1[0] = new Vector2(transform.position.x + 0.1f, transform.position.y);
        points_1[1] = new Vector2(transform.position.x + 0.05f, transform.position.y - transform.localScale.y * 0.005f);
        points_1[2] = new Vector2(transform.position.x - 0.01f, transform.position.y - transform.localScale.y * 0.033f);
        points_1[3] = new Vector2(transform.position.x - 0.05f, transform.position.y - transform.localScale.y * 0.06f);
        points_1[4] = new Vector2(transform.position.x - 0.092f, transform.position.y - transform.localScale.y * 0.092f);
        points_1[5] = new Vector2(transform.position.x - 0.501f, transform.position.y - transform.localScale.y * 0.501f);

        points_2[0] = new Vector2(transform.position.x - 0.092f, transform.position.y - transform.localScale.y * 0.092f);
        points_2[1] = new Vector2(transform.position.x - 0.05f, transform.position.y - transform.localScale.y * 0.06f);
        points_2[2] = new Vector2(transform.position.x - 0.01f, transform.position.y - transform.localScale.y * 0.033f);
        points_2[3] = new Vector2(transform.position.x + 0.05f, transform.position.y - transform.localScale.y * 0.005f);
        points_2[4] = new Vector2(transform.position.x + 0.1f, transform.position.y);
        points_2[5] = new Vector2(transform.position.x + 0.501f, transform.position.y);

        dir_1 = new Vector2(1, 0);
        dir_2 = new Vector2(-1, -1 * transform.localScale.y);

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
    }

    void Turn270()
    {
        points_1[0] = new Vector2(transform.position.x, transform.position.y + 0.1f);
        points_1[1] = new Vector2(transform.position.x + transform.localScale.y * 0.005f, transform.position.y + 0.05f);
        points_1[2] = new Vector2(transform.position.x + transform.localScale.y * 0.033f, transform.position.y - 0.01f);
        points_1[3] = new Vector2(transform.position.x + transform.localScale.y * 0.06f, transform.position.y - 0.05f);
        points_1[4] = new Vector2(transform.position.x + transform.localScale.y * 0.092f, transform.position.y - 0.092f);
        points_1[5] = new Vector2(transform.position.x + transform.localScale.y * 0.501f, transform.position.y - 0.501f);

        points_2[0] = new Vector2(transform.position.x + transform.localScale.y * 0.092f, transform.position.y - 0.092f);
        points_2[1] = new Vector2(transform.position.x + transform.localScale.y * 0.06f, transform.position.y - 0.05f);
        points_2[2] = new Vector2(transform.position.x + transform.localScale.y * 0.033f, transform.position.y - 0.01f);
        points_2[3] = new Vector2(transform.position.x + transform.localScale.y * 0.005f, transform.position.y + 0.05f);
        points_2[4] = new Vector2(transform.position.x, transform.position.y + 0.1f);
        points_2[5] = new Vector2(transform.position.x, transform.position.y + 0.501f);

        dir_1 = new Vector2(0, 1);
        dir_2 = new Vector2(1 * transform.localScale.y, -1 );

        GetComponent<RailwayScript>().AddConect(dir_1);
        GetComponent<RailwayScript>().AddConect(dir_2);
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