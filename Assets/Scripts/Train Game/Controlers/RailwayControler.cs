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
            r.MakeNewConects();
        }

        foreach (var r in railways)
        {
            r.Draw();
            r.BuildRailway();
        }

    }


    public void AddRailway(RailwayScript railway)
    {
        railways.Add(railway);
    }
}