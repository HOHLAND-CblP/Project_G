using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void Moments(GameObject[] obj);
public class Plot1
{
    public static Moments[] delsArray = { Moment1, Moment2, Moment3, Moment4, Moment5, Moment6, Moment7,
        Moment8, Moment9, Moment10, Moment11, Moment12, Moment13, Moment14, Moment15, Moment16, Moment17, 
        Moment18, Moment19, Moment20, Moment21, Moment22, Moment23, Moment24, Moment25, Moment26, Moment27,
        Moment28, Moment29, Moment30, Moment31, Moment32, Moment33, Moment34, Moment35, Moment36, Moment37,
        Moment38, Moment39, Moment40};

    static void Moment1(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].GetComponent<Animator>().SetBool("anim2", true);
        obj[5].GetComponent<Animator>().SetBool("anim2", true);
    }

    static void Moment2(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[7].GetComponent<Animator>().enabled = true;
    }

    static void Moment3(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[20].GetComponent<PlotTracking>().StartCoroutine(obj[20].GetComponent<PlotTracking>().Wait(3));
        GamePrefs.prologCrutch1 = true;
    }

    static void Moment4(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[6].GetComponent<Animator>().enabled = true;
    }

    static void Moment5(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].GetComponent<Animator>().SetBool("anim3", true);
        obj[5].GetComponent<Animator>().SetBool("anim3", true);
    }

    static void Moment6(GameObject[] obj)
    {
        obj[6].SetActive(false);
        obj[7].SetActive(false);
        obj[12].SetActive(true);
    }

    static void Moment7(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[9].SetActive(true);
        obj[10].SetActive(false);
        obj[11].GetComponent<Animator>().enabled = true;
        obj[8].transform.localScale = new Vector3(-1, 1, 1);
    }

    static void Moment8(GameObject[] obj)
    {
        obj[4].GetComponent<Animator>().SetBool("anim4", true);
        obj[5].GetComponent<Animator>().SetBool("anim4", true);
    }

    static void Moment9(GameObject[] obj)
    {
        obj[12].GetComponent<SpriteRenderer>().sortingOrder = 7;
        obj[12].GetComponent<Animator>().enabled = true;
    }

    static void Moment10(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[12].GetComponent<Animator>().SetBool("atack", true);
        obj[4].GetComponent<Animator>().SetBool("anim5", true);
        obj[5].GetComponent<Animator>().SetBool("anim5", true);
        GamePrefs.prologCrutch2 = true;
        GamePrefs.prologCrutch3 = true;
    }

    static void Moment11(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[20].GetComponent<PlotTracking>().StartCoroutine(obj[20].GetComponent<PlotTracking>().Wait(3));
    }

    static void Moment12(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[13].GetComponent<Animator>().enabled = true;
    }

    static void Moment13(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[14].GetComponent<Animator>().enabled = true;
    }

    static void Moment14(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[14].GetComponent<Animator>().SetBool("go", true);
    }

    static void Moment15(GameObject[] obj)
    {
        obj[17].GetComponent<SpriteRenderer>().enabled = true;
        obj[17].GetComponent<Animator>().SetBool("endOfPlot", true);
        GamePrefs.prologCrutch4 = true;
    }

    static void Moment16(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[18].GetComponent<Animator>().SetBool("Sit", true);
    }

    static void Moment17(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[18].GetComponent<Animator>().SetBool("Up", true);
        obj[18].GetComponent<Animator>().SetBool("Sit", false);
    }

    static void Moment18(GameObject[] obj)
    {
        obj[18].GetComponent<Animator>().SetBool("Up", false);
        obj[20].GetComponent<TheMainMainScript>().OpenHint("Используйте стрелки, чтобы двигаться.", 0);
        GamePrefs.diplomaDialog = true;
    }

    static void Moment19(GameObject[] obj)
    {
        obj[19].GetComponent<BoxCollider2D>().enabled = true;
        obj[20].GetComponent<TheMainMainScript>().OpenHint("Подойдите к окну.", 180);
    }

    static void Moment20(GameObject[] obj)
    {
        obj[19].GetComponent<BoxCollider2D>().enabled = false;
        obj[20].GetComponent<TheMainMainScript>().OpenHint("Идите в коридор. Для этого выйдите в дверь.", 180);
    }

    static void Moment21(GameObject[] obj)
    {
        obj[20].GetComponent<TheMainMainScript>().OpenHint("Выходите на улицу", 0);
    }

    static void Moment22(GameObject[] obj)
    {
        obj[20].GetComponent<TheMainMainScript>().OpenHint("Подходите к остановке", 0);
    }

    static void Moment23(GameObject[] obj)
    {
        GamePrefs.inout = 2;
        GamePrefs.id = 0;
        obj[20].GetComponent<TheMainMainScript>().currentLevel.GetComponent<BackGroundCarController>().StopTravel();
        obj[20].GetComponent<TrackingTheHero>().faded.SetActive(true);
    }

    static void Moment24(GameObject[] obj)
    {
        obj[20].GetComponent<TheMainMainScript>().OpenHint("В своем телефоне вы можете посмотреть состояние здоровья и голода Семёна, а также много другого.", 180);
        obj[20].GetComponent<TheMainMainScript>().StartDialog();
    }

    static void Moment25(GameObject[] obj)
    {
        obj[20].GetComponent<TheMainMainScript>().OpenHint("Выходите и идите в кафе", 180);
    }

    static void Moment26(GameObject[] obj)
    {
        obj[21].transform.localScale = new Vector3(4, 4, 1);
        obj[20].GetComponent<TheMainMainScript>().StartDialog();
    }

    static void Moment27(GameObject[] obj)
    {
        obj[21].transform.localScale = new Vector3(-4, 4, 1);
        obj[20].GetComponent<TheMainMainScript>().StartDialog();
        GamePrefs.prologCrutch5 = true;
    }

    static void Moment28(GameObject[] obj)
    {
        obj[22].GetComponent<BoxCollider2D>().enabled = true;
        obj[14].transform.localPosition = new Vector3(3.6f, -44.4f, 13.425f);
        obj[14].transform.localScale = new Vector3(4, 4, 0);
        obj[14].SetActive(false);
        obj[27].SetActive(true);
    }

    static void Moment29(GameObject[] obj)
    {
        if (GamePrefs.chooseButton == 0)
        {
            obj[20].GetComponent<TheMainMainScript>().StartDialog();
            GamePrefs.prologCrutch7 = true;
        }
        else
        {
            obj[20].GetComponent<TrackingTheHero>().endGame.SetActive(true);
        }
    }

    static void Moment30(GameObject[] obj)
    {
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().callerName = "Мама";
        obj[20].GetComponent<TheMainMainScript>().phoneButton.GetComponent<Animator>().SetBool("call", true);
    }

    static void Moment31(GameObject[] obj)
    {
        GamePrefs.isCallDialog = true;
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Animator>().SetBool("phone", true);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().statMenu.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().mainMenu.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().bankMenu.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().contactsMenu.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().notesMenu.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().callMenu.SetActive(true);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().callMenu.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().callMenu.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().callMenu.transform.GetChild(0).gameObject.SetActive(true);
        obj[20].GetComponent<TheMainMainScript>().phone.GetComponent<Phone>().callMenu.transform.GetChild(1).gameObject.SetActive(false);
        obj[20].GetComponent<TheMainMainScript>().StartDialog();
    }
    
    static void Moment32(GameObject[] obj)
    {
        GamePrefs.isCallDialog = false;
        obj[24].SetActive(true);
    }

    static void Moment33(GameObject[] obj)
    {
        obj[20].GetComponent<TheMainMainScript>().StartDialog();
    }

    static void Moment34(GameObject[] obj)
    {
        obj[20].GetComponent<TheMainMainScript>().OpenHint("Возьмите красный рюкзак", 180);
        GamePrefs.prologCrutch8 = true;
    }

    static void Moment35(GameObject[] obj)
    {
        obj[18].GetComponent<Animator>().SetBool("LookAround", true);
    }

    static void Moment36(GameObject[] obj)
    {
        obj[24].GetComponent<Animator>().SetBool("LookAround", true); 
        obj[18].GetComponent<Animator>().SetBool("LookAround", false);
    }
    
    static void Moment37(GameObject[] obj)
    {
        obj[24].GetComponent<Animator>().SetBool("LookAround", false);
    }

    static void Moment38(GameObject[] obj)
    {
        obj[24].transform.localScale = new Vector3(-4, 4, 0);
        obj[24].GetComponent<Animator>().SetBool("Walk", true);
    }
    static void Moment39(GameObject[] obj)
    {
        obj[26].GetComponent<Animator>().SetBool("Open", false);
        obj[28].SetActive(true);
        obj[20].GetComponent<TheMainMainScript>().StartDialog();
    }

    static void Moment40(GameObject[] obj)
    {
        obj[20].GetComponent<TrackingTheHero>().faded.SetActive(true);
        GamePrefs.isNeedMessage = true;
        GamePrefs.countOfPlotMoment = 0;
        GamePrefs.countOfPlots++;
        GamePrefs.inout = -1;
    }
}