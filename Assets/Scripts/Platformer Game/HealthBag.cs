using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthBag : MonoBehaviour
{
    GameObject player;
    int healthValue;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Heal(int id)
    {
        if (id == 0)
            healthValue = 200;
        else if (id == 1)
            healthValue = 300;
        else if (id == 2)
            GamePrefs.headAche = false;

        player.GetComponent<HeroControler>().health = Math.Min(100, player.GetComponent<HeroControler>().health + healthValue);
        GamePrefs.countsOfKindOfHealth[id]--;
        GamePrefs.amountOfHealth--;
        Destroy(gameObject);
    }
}