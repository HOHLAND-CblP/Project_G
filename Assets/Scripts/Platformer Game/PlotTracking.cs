using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotTracking : MonoBehaviour
{
    public GameObject[] objects;

    GameObject cur;

    private void FixedUpdate()
    {
        if (!GamePrefs.inDialog && GamePrefs.countOfPlots == 1 && GamePrefs.countOfPlotMoment == 20 &&
            GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId == 3 &&
            objects[18].transform.position.x > 48.44)
        {
            GetComponent<TheMainMainScript>().StartDialog();
        }
        if (!GamePrefs.inDialog && GamePrefs.countOfPlots == 1 && GamePrefs.countOfPlotMoment == 23 &&
            GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId == 7 &&
            objects[18].transform.position.x > 62.5)
        {
            GetComponent<TheMainMainScript>().StartDialog();
        }
        if (!GamePrefs.inDialog && GamePrefs.countOfPlots == 1 && GamePrefs.countOfPlotMoment == 25 &&
            GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId == 6 &&
            GetComponent<TrackingTheHero>().leftV == -8 &&
            objects[18].transform.position.x < -14.2)
        {
            GetComponent<TheMainMainScript>().StartDialog();
        }
        if (GetComponent<TheMainMainScript>().rejected)
        {
            GetComponent<TheMainMainScript>().rejected = false;
            StartCoroutine(GetComponent<TheMainMainScript>().Call());
        }
        if (!GetComponent<TheMainMainScript>().rejected && !GamePrefs.inDialog && GamePrefs.countOfPlots == 1 &&
            GamePrefs.countOfPlotMoment == 21 && GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId==2
            && objects[18].transform.position.x < -114)
        {
            GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().callerName = "Мама";
            GetComponent<TheMainMainScript>().phoneButton.GetComponent<Animator>().SetBool("call", true);
        }
    }

    public void NextPlotMoment()
    {
        switch (GamePrefs.countOfPlots)
        {
            case 1:
                if (GamePrefs.prologCrutch3)
                {
                    GamePrefs.countsOfcountOfDialogs[GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId] =
                           GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().countOfDialogs;
                    cur = GetComponent<TheMainMainScript>().currentLevel = objects[16];
                    transform.position = new Vector3(cur.GetComponent<SceneProperties>().leftV
                        + (cur.GetComponent<SceneProperties>().rightV - cur.GetComponent<SceneProperties>().leftV) / 2,
                        cur.GetComponent<SceneProperties>().groundY + 3, transform.position.z);
                    objects[17].GetComponent<Animator>().enabled = true;
                    GamePrefs.prologCrutch3 = false;
                }
                else if (GamePrefs.prologCrutch4)
                {
                    GamePrefs.prologue = false;
                    //cur = GetComponent<TheMainMainScript>().currentLevel = objects[15];
                    GamePrefs.countsOfcountOfDialogs[GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId] =
                        GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().countOfDialogs;
                    GamePrefs.currentLevel = objects[15];/*
                    transform.position = new Vector3(cur.GetComponent<SceneProperties>().leftV
                        + (cur.GetComponent<SceneProperties>().rightV - cur.GetComponent<SceneProperties>().leftV) / 2,
                        cur.GetComponent<SceneProperties>().groundY + 2, transform.position.z);
                    GetComponent<TrackingTheHero>().leftV = cur.GetComponent<SceneProperties>().leftV;
                    GetComponent<TrackingTheHero>().rightV = cur.GetComponent<SceneProperties>().rightV;*/
                    SceneManager.LoadScene(3);
                }
                else if (GamePrefs.prologCrutch6)
                {
                    objects[14].GetComponent<Animator>().SetBool("goToTable", false);
                    GamePrefs.prologCrutch6 = false;
                }
                else if (GamePrefs.prologCrutch7)
                {
                    GamePrefs.prologCrutch7 = false;
                    cur = GetComponent<TheMainMainScript>().currentLevel = objects[23];
                    objects[18].GetComponent<Animator>().SetBool("Table", false);
                    objects[18].GetComponent<BoxCollider2D>().size = new Vector2(objects[18].GetComponent<BoxCollider2D>().size.x, 1.102759f);
                    objects[18].transform.position = new Vector3(45.13f, cur.GetComponent<SceneProperties>().groundY, objects[18].transform.position.z); 
                    objects[18].transform.localScale = new Vector3(-4, 4, 0);
                    objects[18].GetComponent<HeroControler>().facingRight = true;
                    GamePrefs.countsOfcountOfDialogs[GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId] = 
                        GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().countOfDialogs;
                    transform.position = new Vector3(cur.GetComponent<SceneProperties>().leftV
                        + (cur.GetComponent<SceneProperties>().rightV - cur.GetComponent<SceneProperties>().leftV) / 2,
                        cur.GetComponent<SceneProperties>().groundY + 2, transform.position.z);
                    GetComponent<TrackingTheHero>().leftV = cur.GetComponent<SceneProperties>().leftV;
                    GetComponent<TrackingTheHero>().rightV = cur.GetComponent<SceneProperties>().rightV;
                    GetComponent<TrackingTheHero>().unfaded.SetActive(true);
                    objects[0].SetActive(false);
                    objects[1].SetActive(false);
                    objects[2].SetActive(false);
                    NextPlotMoment();
                }
                else if (GamePrefs.prologCrutch8)
                {
                    GetComponent<AudioSource>().clip = GetComponent<TheMainMainScript>().endOfDemo;
                    GetComponent<AudioSource>().Play();
                    GamePrefs.prologCrutch8 = false;
                    objects[26].GetComponent<Animator>().SetBool("Open", true); 
                    GamePrefs.countsOfcountOfDialogs[GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId] =
                         GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().countOfDialogs;
                    cur = GetComponent<TheMainMainScript>().currentLevel = objects[25];
                    objects[24].transform.position = new Vector3(5.0f, -80.88f, 0);
                    objects[24].transform.localScale = new Vector3(4, 4, 0);
                    objects[18].transform.position = new Vector3(-2.15f, cur.GetComponent<SceneProperties>().groundY, objects[18].transform.position.z);
                    objects[18].transform.localScale = new Vector3(-4, 4, 0);
                    objects[18].GetComponent<HeroControler>().facingRight = true;
                    /*transform.position = new Vector3(cur.GetComponent<SceneProperties>().leftV
                        + (cur.GetComponent<SceneProperties>().rightV - cur.GetComponent<SceneProperties>().leftV) / 2,
                        cur.GetComponent<SceneProperties>().groundY + 2, transform.position.z);*/
                    GetComponent<TrackingTheHero>().leftV = cur.GetComponent<SceneProperties>().leftV;
                    GetComponent<TrackingTheHero>().rightV = cur.GetComponent<SceneProperties>().rightV;
                    GetComponent<TrackingTheHero>().unfaded.SetActive(true);
                    objects[0].SetActive(false);
                    objects[1].SetActive(false);
                    objects[2].SetActive(false);
                    NextPlotMoment();
                }
                else
                {
                    if (GamePrefs.countOfPlotMoment < Plot1.delsArray.Length)
                    {
                        Plot1.delsArray[GamePrefs.countOfPlotMoment](objects);
                        GamePrefs.countOfPlotMoment++;
                    }
                    else
                    {
                        GamePrefs.countOfPlots++;
                        GamePrefs.countOfPlotMoment = 0;
                    }
                }
                break;
            case 2:

                break;
        }
    }

    public IEnumerator Wait(float seconds)
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
        else if (GamePrefs.prologCrutch5)
        {
            GamePrefs.prologCrutch5 = false;
            objects[14].SetActive(true); 
            objects[14].GetComponent<Animator>().enabled = true;
            objects[14].GetComponent<Animator>().SetBool("goToTable", true);
            GamePrefs.prologCrutch6 = true;
        }
    }
}
