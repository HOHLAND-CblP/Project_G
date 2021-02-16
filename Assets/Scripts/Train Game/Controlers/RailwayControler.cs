using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwayControler : MonoBehaviour
{
    readonly List<RailwayScript> railways = new List<RailwayScript>();


    public void StartConect()
    {
        foreach (var r in railways)
        {
            r.GetComponent<RailwayScript>().SetConect();
        }

        foreach (var r in railways)
        {
            UpdateCellsAround((int)r.transform.position.x, (int)r.transform.position.y);
        }
    }


    public void UpdateCellsAround(int x, int y)
    {
        GameObject[,] aroundCell = new GameObject[3, 3];

        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
            {
                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(x + i, y + k);
                if (i != 0 || k != 0)
                {
                    if (aroundCell[i + 1, k + 1] && aroundCell[i + 1, k + 1].GetComponent<RailwayScript>())
                        aroundCell[i + 1, k + 1].GetComponent<RailwayScript>().SetConect();
                }
            }
    }


    public void AddRailway(RailwayScript railway)
    {
        railways.Add(railway);
    }
}