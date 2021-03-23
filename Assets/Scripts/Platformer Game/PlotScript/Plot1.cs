using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void Moments(GameObject[] obj);
public static class Plot1
{
    public static Moments[] delsArray = { Moment1, Moment2, Moment3, Moment4, Moment5, Moment6, Moment7, Moment8, Moment9, Moment10, Moment11, Moment12, Moment13};
    static void Moment1(GameObject[] obj)
    {
        Debug.Log("Момент 1");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].GetComponent<Animator>().SetBool("anim2", true);
        obj[5].GetComponent<Animator>().SetBool("anim2", true);
    }

    static void Moment2(GameObject[] obj)
    {
        Debug.Log("Момент 2");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[7].GetComponent<Animator>().enabled = true;
        GamePrefs.prologCrutch1 = true;
    }

    static void Moment3(GameObject[] obj)
    {
        Debug.Log("Момент 3");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[6].GetComponent<Animator>().enabled = true;
    }

    static void Moment4(GameObject[] obj)
    {
        Debug.Log("Момент 4");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].GetComponent<Animator>().SetBool("anim3", true);
        obj[5].GetComponent<Animator>().SetBool("anim3", true);
    }

    static void Moment5(GameObject[] obj)
    {
        Debug.Log("Момент 5");
        obj[6].SetActive(false);
        obj[7].SetActive(false);
        obj[12].SetActive(true);
    }

    static void Moment6(GameObject[] obj)
    {
        Debug.Log("Момент 6");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[9].SetActive(true);
        obj[10].SetActive(false);
        obj[11].GetComponent<Animator>().enabled = true;
        obj[8].transform.localScale = new Vector3(-1, 1, 1);
    }

    static void Moment7(GameObject[] obj)
    {
        Debug.Log("Момент 7");
        obj[4].GetComponent<Animator>().SetBool("anim4", true);
        obj[5].GetComponent<Animator>().SetBool("anim4", true);
    }

    static void Moment8(GameObject[] obj)
    {
        Debug.Log("Момент 8");
        obj[12].GetComponent<SpriteRenderer>().sortingOrder = obj[8].GetComponent<SpriteRenderer>().sortingOrder;
        obj[12].GetComponent<Animator>().enabled = true;
    }

    static void Moment9(GameObject[] obj)
    {
        Debug.Log("Момент 9");
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

    static void Moment10(GameObject[] obj)
    {
        Debug.Log("Момент 10");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[13].GetComponent<Animator>().enabled = true;
    }

    static void Moment11(GameObject[] obj)
    {
        Debug.Log("Момент 11");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[14].GetComponent<Animator>().enabled = true;
    }

    static void Moment12(GameObject[] obj)
    {
        Debug.Log("Момент 12");
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[14].GetComponent<Animator>().SetBool("go", true);
    }

    static void Moment13(GameObject[] obj)
    {
        Debug.Log("Момент 13");
        obj[17].GetComponent<SpriteRenderer>().enabled = true;
        obj[17].GetComponent<Animator>().SetBool("endOfPlot", true);
        GamePrefs.prologCrutch4 = true;
    }
}
