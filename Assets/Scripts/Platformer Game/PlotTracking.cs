using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotTracking : MonoBehaviour
{
    [SerializeField]
    GameObject[] objects;
    public void NextPlotMoment()
    {
        switch (GamePrefs.countOfPlots)
        {
            case 1:
                if (GamePrefs.prologCrutch1)
                {
                    objects[0].SetActive(false);
                    objects[1].SetActive(false);
                    objects[2].SetActive(false);
                    objects[3].SetActive(false);
                    StartCoroutine(Wait(3));
                }
                else if (GamePrefs.prologCrutch2)
                {
                    objects[0].SetActive(false);
                    objects[1].SetActive(false);
                    objects[2].SetActive(false);
                    objects[3].SetActive(false);
                    StartCoroutine(Wait(3));
                }
                else
                {
                    if (GamePrefs.countOfPlotMoment < Plot1.delsArray.Length)
                    {
                        Plot1.delsArray[GamePrefs.countOfPlotMoment](objects);
                        GamePrefs.countOfPlotMoment++;
                    }
                    else GamePrefs.countOfPlots++;
                }
                break;
            case 2:

                break;
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (GamePrefs.prologCrutch1)
        {
            GamePrefs.prologCrutch1 = false;
            NextPlotMoment();
        }
        else if (GamePrefs.prologCrutch2)
        {
            GamePrefs.prologCrutch2 = false;
            GetComponent<TheMainMainScript>().StartDialog();
        }
    }
}
