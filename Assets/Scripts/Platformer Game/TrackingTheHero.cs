using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackingTheHero : MonoBehaviour
{
    public GameObject player, faded, unfaded, shining;
    public float leftV, rightV;
    public GameObject endGame, pausePanel, message;

    private void Start()
    {
        if (!GamePrefs.prologCrutch4)
        {
            transform.position = new Vector3(0, GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY, transform.position.z);
            GetComponent<TheMainMainScript>().phoneButton.SetActive(false);
            GetComponent<TheMainMainScript>().left.SetActive(false);
            GetComponent<TheMainMainScript>().right.SetActive(false);
            GetComponent<TheMainMainScript>().pauseButton.SetActive(false);
            GamePrefs.prologue = true;
            GetComponent<TrackingTheHero>().enabled = false;
            GetComponent<PlotTracking>().objects[4].GetComponent<Animator>().enabled = true;
            GetComponent<PlotTracking>().objects[5].GetComponent<Animator>().enabled = true;
        }
        else if (GamePrefs.prologCrutch4)
        {
            GamePrefs.prologCrutch4 = false;
            GetComponent<TheMainMainScript>().currentLevel = GetComponent<PlotTracking>().objects[15];
            leftV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV;
            rightV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV;
            GetComponent<PlotTracking>().NextPlotMoment();
        }    
        else if (GamePrefs.currentLevel == null)
        {
            player.transform.position = new Vector3(player.transform.position.x, GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY);
            leftV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV;
            rightV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV;
        }
        if(!GamePrefs.prologue)
            unfaded.SetActive(true);
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, leftV, rightV), player.transform.position.y + 2, transform.position.z);
        if (player.GetComponent<HeroControler>().health <= 0)
            endGame.SetActive(true);

        if (player.GetComponent<HeroControler>().headAche)
            shining.SetActive(true);
        else
            shining.SetActive(false);

        if (unfaded.activeSelf && unfaded.GetComponent<Image>().color.a == 0)
        {
            if (GamePrefs.isNeedMessage) 
            { 
                message.SetActive(true); 
                GamePrefs.isNeedMessage = false; 
            }
            unfaded.SetActive(false);
        }
        if (faded.activeSelf && faded.GetComponent<Image>().color.a == 1)
        {
            if (GamePrefs.isNeedMessage) message.SetActive(true);
            if (GamePrefs.inout == 1)
            {
                leftV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV;
                rightV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV;
                player.transform.position = GetComponent<TheMainMainScript>().darkSide.transform.position;
                player.transform.position = new Vector3(player.transform.position.x, GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY);
                if (GamePrefs.countOfHint == 5 && GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId == 2)
                {
                    GetComponent<TheMainMainScript>().OpenHint("Идите налево, к остановке.", 180);
                }
            }
            else if (GamePrefs.inout == 2)
            {
                GamePrefs.countsOfcountOfDialogs[GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId] = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().countOfDialogs;
                GetComponent<TheMainMainScript>().currentLevel = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().busStop.GetComponent<ObjectProperties>().nextLevels[GamePrefs.id];
                GetComponent<TheMainMainScript>().animMap.gameObject.transform.GetChild(1).GetComponent<Text>().text = "Текущая локация: " +
                    GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().title;
                leftV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV;
                rightV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV;
                player.transform.position = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().busStop.transform.position;
                player.transform.position = new Vector3(player.transform.position.x, GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY);
            }
            else if (GamePrefs.inout == 3)
            {
                leftV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV;
                rightV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV;
                player.transform.position = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().teleport.transform.position;
                player.transform.position = new Vector3(player.transform.position.x + 2, GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY);
            }
            else if (GamePrefs.inout == 4)
            {
                leftV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV;
                rightV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV;
                player.transform.position = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().teleport.transform.position;
                player.transform.position = new Vector3(player.transform.position.x - 2, GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY);
            }
            else if (GamePrefs.inout == 5)
            {
                leftV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().leftV;
                rightV = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().rightV;
                player.transform.position = GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().computer.transform.position;
                player.transform.position = new Vector3(player.transform.position.x, GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().groundY);
            }
            GetComponent<TheMainMainScript>().animMap.SetBool("map", false);
            unfaded.SetActive(true);
            faded.SetActive(false);
            if (GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId == 2
                || GetComponent<TheMainMainScript>().currentLevel.GetComponent<SceneProperties>().sceneId == 5)
            {
                GetComponent<TheMainMainScript>().currentLevel.GetComponent<BackGroundCarController>().Travel();
            }
        }
    }  
}
