using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFromFridge : MonoBehaviour
{
    GameObject player;
    int calorificValue;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Eat(int id)
    {
        if (id == 0)
            calorificValue = 20;
        else if (id == 1)
            calorificValue = 10;
        else if (id == 2)
            calorificValue = 50;
        else if (id == 3)
            calorificValue = 70;
        else if (id == 4)
            calorificValue = 40;
        else if (id == 5)
            calorificValue = 90;
        player.GetComponent<HeroControler>().satiety = Math.Min(100, player.GetComponent<HeroControler>().satiety + calorificValue);
        GamePrefs.countsOfKindOfFood[id]--;
        GamePrefs.amountOfFood--;
        Destroy(gameObject);
    }
}
