using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiseChapterPanel : MonoBehaviour
{
    GameObject cam;
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    public void ChapterDay(int chapterDay)
    {
        gameObject.SetActive(false);
        GamePrefs.day = chapterDay % 10;
        GamePrefs.chapter = chapterDay / 10;
        cam.GetComponent<TheMainMainScript>().fade.SetActive(true);

        //if (GamePrefs.chapter == 1 && GamePrefs.day == 1)
        //{
        //    GamePrefs.day = 2;
        //    GamePrefs.firstSceneFirstDialog = true;
        //    GamePrefs.countOfDialog = 0;
        //    cam.GetComponent<TheMainMainScript>().StartDialog();
        //}
        //else if (GamePrefs.chapter == 1 && GamePrefs.day == 2)
        //{
        //    GamePrefs.chapter++;
        //    GamePrefs.day = 1;
        //    GamePrefs.countOfDialog = 13;
        //    cam.GetComponent<House>().animBedButton.gameObject.SetActive(false);
        //}
        //else if (GamePrefs.chapter == 2 && GamePrefs.day == 1)
        //{
        //    GamePrefs.day = 2;
        //    GamePrefs.countOfDialog = 26;
        //    cam.GetComponent<House>().animBedButton.gameObject.SetActive(false);
        //}
        //else if (GamePrefs.chapter == 2 && GamePrefs.day == 2)
        //{
        //    GamePrefs.chapter++;
        //    GamePrefs.day = 1;
        //    GamePrefs.countOfDialog = 38;
        //    cam.GetComponent<House>().animBedButton.gameObject.SetActive(false);
        //}
        //else if (GamePrefs.chapter == 3 && GamePrefs.day == 1)
        //{
        //    GamePrefs.day = 2;
        //    GamePrefs.countOfDialog = 54;
        //    GamePrefs.headAche = true;
        //    GamePrefs.eleventhSceneFirstDialog = true;
        //}
        //else if (GamePrefs.chapter == 3 && GamePrefs.day == 2)
        //{
        //    GamePrefs.day = 1;
        //    GamePrefs.chapter++;
        //    GamePrefs.countOfDialog = 70;
        //}
        //else if (GamePrefs.chapter == 4 && GamePrefs.day == 1)
        //{
        //    GamePrefs.day = 2;
        //    GamePrefs.countOfDialog = 78;
        //}
        //cam.GetComponent<TheMainMainScript>().levelText.GetComponent<Text>().text = $"Глава {GamePrefs.chapter} День {GamePrefs.day}";
    }
}
