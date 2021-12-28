using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Сrutch : MonoBehaviour
{
    GameObject cam;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    public void StartDialog()
    {
        cam.GetComponent<TheMainMainScript>().StartDialog();
    }

    public void NextMoment()
    {
        cam.GetComponent<PlotTracking>().NextPlotMoment();
    }
}