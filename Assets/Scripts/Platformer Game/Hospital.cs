using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Hospital : MonoBehaviour
{
    GameObject cam;
    public Animator animMessage;
    public GameObject player, choosePanel, healthBag;
    int _id = -1, healthValue;
    bool outputMessage, maxHealthBag = false;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void FixedUpdate()
    {
        if (GamePrefs.amountOfHealth == 10)
            maxHealthBag = true;
    }

    public void Buy(int id)
    {
        if ((_id = id) == 0 && player.GetComponent<HeroControler>().cash >= 200)
        {
            choosePanel.SetActive(true);
            healthValue = 50;
        }
        else if ((_id = id) == 1 && player.GetComponent<HeroControler>().cash >= 300)
        {
            choosePanel.SetActive(true);
            healthValue = 75;
        }
        else if ((_id = id) == 2 && player.GetComponent<HeroControler>().cash >= 100)
        {
            choosePanel.SetActive(true);
        }
        else
        {
            animMessage.gameObject.transform.GetChild(0).transform.GetComponent<Text>().text = "Недостаточно денег!";
            outputMessage = true;
            animMessage.SetBool("isMessage", true);
            StartCoroutine(Messages());
        }
    }

    public void Eat()
    {
        choosePanel.SetActive(false);
        if (_id == 0)
            player.GetComponent<HeroControler>().cash -= 200;
        else if (_id == 1)
            player.GetComponent<HeroControler>().cash -= 300;
        else if (_id == 2)
            player.GetComponent<HeroControler>().cash -= 100;
        if (_id != 2) player.GetComponent<HeroControler>().health = Math.Min(100, player.GetComponent<HeroControler>().health + healthValue);
        else GamePrefs.headAche = false;
    }

    public void InHealthBag()
    {
        choosePanel.SetActive(false);
        if (!maxHealthBag)
        {
            if (_id == 0)
            {
                player.GetComponent<HeroControler>().cash -= 200;
                healthBag.GetComponent<ObjectProperties>().AddHealthBag(_id);
            }
            else if (_id == 1)
            {
                player.GetComponent<HeroControler>().cash -= 300;
                healthBag.GetComponent<ObjectProperties>().AddHealthBag(_id);
            }
            else if (_id == 2)
            {
                player.GetComponent<HeroControler>().cash -= 100;
                healthBag.GetComponent<ObjectProperties>().AddHealthBag(_id);
            }
            GamePrefs.amountOfHealth++;
            GamePrefs.countsOfKindOfHealth[_id]++;
        }
        else
        {
            animMessage.gameObject.transform.GetChild(0).transform.GetComponent<Text>().text = "Коробка заполнена!";
            outputMessage = true;
            animMessage.SetBool("isMessage", true);
            StartCoroutine(Messages());
        }
    }

    IEnumerator Messages()
    {
        while (outputMessage)
        {
            yield return new WaitForSeconds(2);
            animMessage.SetBool("isMessage", false);
            outputMessage = false;
        }
    }
}
