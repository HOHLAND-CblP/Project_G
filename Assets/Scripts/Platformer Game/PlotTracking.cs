using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotTracking : MonoBehaviour
{
    public GameObject[] objects;
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
                    Debug.Log("Костыль 1");
                }
                else if (GamePrefs.prologCrutch2)
                {
                    GetComponent<TheMainMainScript>().currentLevel = objects[16];
                    transform.position = new Vector3(GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV
                        + (GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV - GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV) / 2,
                        GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY + 3, transform.position.z);
                    objects[17].GetComponent<Animator>().enabled = true;
                    GamePrefs.prologCrutch2 = false;
                    Debug.Log("Костыль 2");
                }
                else if (GamePrefs.prologCrutch3)
                {
                    objects[0].SetActive(false);
                    objects[1].SetActive(false);
                    objects[2].SetActive(false);
                    objects[3].SetActive(false);
                    StartCoroutine(Wait(3)); 
                    Debug.Log("Костыль 3");
                }
                else if (GamePrefs.prologCrutch4)
                {
                    //GetComponent<TheMainMainScript>().currentLevel = null;
                    GetComponent<TheMainMainScript>().currentLevel = objects[15];
                    transform.position = new Vector3(GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV
                        + (GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV - GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV) / 2,
                        GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY, transform.position.z);
                    //GamePrefs.currentLevel = objects[15];
                    //SceneManager.LoadScene(3);
                    GetComponent<TheMainMainScript>().left.SetActive(true);
                    GetComponent<TheMainMainScript>().right.SetActive(true);
                    GetComponent<TheMainMainScript>().phoneButton.SetActive(true);
                    GetComponent<TheMainMainScript>().pauseButton.SetActive(true);
                    GamePrefs.prologCrutch4 = false;
                    Debug.Log("Костыль 4");
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
        else if (GamePrefs.prologCrutch3)
        {
            GamePrefs.prologCrutch3 = false;
            GetComponent<TheMainMainScript>().StartDialog();
        }
    }
}
