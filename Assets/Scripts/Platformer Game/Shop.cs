using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Animator animMessage;
    public GameObject choosePanel, player, fridge;
    int _id=-1, calorificValue;
    bool outputMessage, maxFridgeFood=false;
    GameObject cam;

    [SerializeField]
    Text costOfItem1, costOfItem2, costOfItem3, costOfItem4, costOfItem5, costOfItem6;
    int _costOfItem1 = 100, _costOfItem2 = 50, _costOfItem3 = 300, _costOfItem4 = 500, _costOfItem5 = 250, _costOfItem6 = 800;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    public void FixedUpdate()
    {
        if (GamePrefs.amountOfFood == 10)
            maxFridgeFood = true;
        if (GamePrefs.discount>0)
        {
            costOfItem1.text = $"Стоит: {_costOfItem1 * (1 - GamePrefs.discount)} рублей. Скидка {GamePrefs.discount * 100}%.";
            costOfItem2.text = $"Стоит: {_costOfItem2 * (1 - GamePrefs.discount)} рублей. Скидка {GamePrefs.discount * 100}%.";
            costOfItem3.text = $"Стоит: {_costOfItem3 * (1 - GamePrefs.discount)} рублей. Скидка {GamePrefs.discount * 100}%.";
            costOfItem4.text = $"Стоит: {_costOfItem4 * (1 - GamePrefs.discount)} рублей. Скидка {GamePrefs.discount * 100}%.";
            costOfItem5.text = $"Стоит: {_costOfItem5 * (1 - GamePrefs.discount)} рублей. Скидка {GamePrefs.discount * 100}%.";
            costOfItem6.text = $"Стоит: {_costOfItem6 * (1 - GamePrefs.discount)} рублей. Скидка {GamePrefs.discount * 100}%.";
        }
        else
        {
            costOfItem1.text = $"Стоит: {_costOfItem1 * (1 - GamePrefs.discount)} рублей.";
            costOfItem2.text = $"Стоит: {_costOfItem2 * (1 - GamePrefs.discount)} рублей.";
            costOfItem3.text = $"Стоит: {_costOfItem3 * (1 - GamePrefs.discount)} рублей.";
            costOfItem4.text = $"Стоит: {_costOfItem4 * (1 - GamePrefs.discount)} рублей.";
            costOfItem5.text = $"Стоит: {_costOfItem5 * (1 - GamePrefs.discount)} рублей.";
            costOfItem6.text = $"Стоит: {_costOfItem6 * (1 - GamePrefs.discount)} рублей.";
        }
    }

    public void Buy(int id)
    {
        if ((_id = id) == 0 && player.GetComponent<HeroControler>().cash>=(100*(1-GamePrefs.discount)))
        {
            choosePanel.SetActive(true);
            calorificValue = 20;
        }
        else if ((_id = id) == 1 && player.GetComponent<HeroControler>().cash >= (50 * (1 - GamePrefs.discount)))
        {
            choosePanel.SetActive(true);
            calorificValue = 10;
        }
        else if ((_id = id) == 2 && player.GetComponent<HeroControler>().cash >= (300 * (1 - GamePrefs.discount)))
        {
            choosePanel.SetActive(true);
            calorificValue = 50;
        }
        else if ((_id = id) == 3 && player.GetComponent<HeroControler>().cash >= (500 * (1 - GamePrefs.discount)))
        {
            choosePanel.SetActive(true);
            calorificValue = 70;
        }
        else if ((_id = id) == 4 && player.GetComponent<HeroControler>().cash >= (250 * (1 - GamePrefs.discount)))
        {
            choosePanel.SetActive(true);
            calorificValue = 40;
        }
        else if ((_id = id) == 5 && player.GetComponent<HeroControler>().cash >= (800 * (1 - GamePrefs.discount)))
        {
            choosePanel.SetActive(true);
            calorificValue = 90;
        }
        else
        {
            animMessage.gameObject.transform.GetChild(0).transform.GetComponent<Text>().text = "Недостаточно денег!";
            outputMessage = true;
            animMessage.SetBool("isMessage",true);
            StartCoroutine(Messages());
        }
    }

    public void Eat()
    {
        choosePanel.SetActive(false);
        if (_id == 0)
            player.GetComponent<HeroControler>().cash -= (100*(1-GamePrefs.discount));
        else if (_id == 1)
            player.GetComponent<HeroControler>().cash -= (50 * (1 - GamePrefs.discount));
        else if (_id == 2)
            player.GetComponent<HeroControler>().cash -= (300 * (1 - GamePrefs.discount));
        else if (_id == 3)
            player.GetComponent<HeroControler>().cash -= (500 * (1 - GamePrefs.discount));
        else if (_id == 4)
            player.GetComponent<HeroControler>().cash -= (250 * (1 - GamePrefs.discount));
        else if (_id == 5)
            player.GetComponent<HeroControler>().cash -= (800 * (1 - GamePrefs.discount));
        player.GetComponent<HeroControler>().satiety = Math.Min(100, player.GetComponent<HeroControler>().satiety + calorificValue);
    }

    public void InFridge()
    {
        choosePanel.SetActive(false);
        if (!maxFridgeFood)
        {
            if (_id == 0)
            {
                player.GetComponent<HeroControler>().cash -= (100 * (1 - GamePrefs.discount));
                fridge.GetComponent<ObjectProperties>().AddFridge(_id);
            }
            else if (_id == 1)
            {
                player.GetComponent<HeroControler>().cash -= (50 * (1 - GamePrefs.discount)); 
                fridge.GetComponent<ObjectProperties>().AddFridge(_id);
            }
            else if (_id == 2)
            {
                player.GetComponent<HeroControler>().cash -= (300 * (1 - GamePrefs.discount));
                fridge.GetComponent<ObjectProperties>().AddFridge(_id);
            }
            else if (_id == 3)
            {
                player.GetComponent<HeroControler>().cash -= (500 * (1 - GamePrefs.discount));
                fridge.GetComponent<ObjectProperties>().AddFridge(_id);
            }
            else if (_id == 4)
            {
                player.GetComponent<HeroControler>().cash -= (250 * (1 - GamePrefs.discount));
                fridge.GetComponent<ObjectProperties>().AddFridge(_id);
            }
            else if (_id == 5)
            {
                player.GetComponent<HeroControler>().cash -= (800 * (1 - GamePrefs.discount));
                fridge.GetComponent<ObjectProperties>().AddFridge(_id);
            }
            GamePrefs.amountOfFood++;
            GamePrefs.countsOfKindOfFood[_id]++;
        }
        else
        {
            animMessage.gameObject.transform.GetChild(0).transform.GetComponent<Text>().text = "Холодильник полон!";
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
