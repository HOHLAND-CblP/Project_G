using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Moments(GameObject[] obj);
public static class Plot1
{
    public static Moments[] delsArray = { Moment1, Moment2, Moment3, Moment4, Moment5, Moment6, Moment7, Moment8, Moment9};
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
        GamePrefs.prologCrutch1 = true;
    }

    static void Moment3(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[6].GetComponent<Animator>().enabled = true;
    }

    static void Moment4(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].GetComponent<Animator>().SetBool("anim3", true);
        obj[5].GetComponent<Animator>().SetBool("anim3", true);
    }

    static void Moment5(GameObject[] obj)
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

    static void Moment6(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[12].GetComponent<Animator>().SetBool("atack", true); 
        obj[4].GetComponent<Animator>().SetBool("anim5", true);
        obj[5].GetComponent<Animator>().SetBool("anim5", true);
        GamePrefs.prologCrutch2 = true;
    }

    static void Moment7(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[13].GetComponent<Animator>().enabled = true;
    }

    static void Moment8(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[14].GetComponent<Animator>().enabled = true;
    }

    static void Moment9(GameObject[] obj)
    {
        obj[0].SetActive(false);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[14].GetComponent<Animator>().SetBool("go", true);
    }
}
