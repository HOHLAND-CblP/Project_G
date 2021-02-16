using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField]
    GameObject player, pausePanel;
    [SerializeField]
    float leftV, rightV;

    void Start()
    {
        GamePrefs.currentRunnerLevel = GameObject.Find("Level" + GamePrefs.runnerLevel.ToString());
        GamePrefs.currentRunnerLevel = GameObject.Find("Level1");
        leftV = GamePrefs.currentRunnerLevel.GetComponent<SceneProperties>().leftV;
        rightV = GamePrefs.currentRunnerLevel.GetComponent<SceneProperties>().rightV;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, leftV, rightV), player.transform.position.y + 2, transform.position.z);
    }

    public void Paused()
    {
        pausePanel.SetActive(true);
        player.SetActive(false);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        player.SetActive(true);
    }
}
