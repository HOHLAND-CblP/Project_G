using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void NextSay();

public class Level1 : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextAsset[] dialogs;
    private DialogTrains curDialog;
    private int curSay;


    void Start()
    {
        dialogPanel.GetComponent<DialogPanel>().nextSay = NextSay;
        curSay = 0;
        curDialog = DialogTrains.Load(dialogs[0]);
        dialogPanel.GetComponent<DialogPanel>().NewDialog(curDialog.says[curSay].id, curDialog.says[curSay].text);
    }

    void Update()
    {
        
    }

    public void NextSay()
    {
        
        if (!curDialog.says[curSay].dialEnd)
        {
            curSay++;
            dialogPanel.GetComponent<DialogPanel>().NewDialog(curDialog.says[curSay].id, curDialog.says[curSay].text);
        }
    }
}
