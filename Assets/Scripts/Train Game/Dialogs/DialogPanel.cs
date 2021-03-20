using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LolilopGamesLibrary;


public class DialogPanel : MonoBehaviour
{
    [Header("Elements Dialog Panel")]
    // Элементы диалогового окна
    [SerializeField]
    Text nameCharacter;
    [SerializeField]
    Image face;
    [SerializeField]
    Text dialog;

    [Header("Character Elements")]
    public string[] names; // Массив имен
    public Sprite[] faces; // Массив лиц

    [Header("Components")]
    public float outputDelay;   // Задержка для постепнного вывода текста

    [HideInInspector]
    public NextSay nextSay;

    string remainingText = "";  // Остаток от деления текста на 2 части

    public string debugText;

    bool skipDialog;    // Скипнули ли диалог 


    void Start()
    {
        // Защита от дурака
        if (outputDelay < 0)
            outputDelay = 0;

        //NewDialog(0, debugText);
    }

    // Запуск нового диалога: выводится ими персонажа, и его картинка, затем запускается вывод диалога
    public void NewDialog(int idCharacter, string textDialog) 
    {
        nameCharacter.text = names[idCharacter];
        face.sprite = faces[idCharacter];
        gameObject.SetActive(true);

        //StartCoroutine(OutputDialog(textDialog));
    }


    // Режет строчку на две части: первая часть - часть, которая влезает в поле диалога (выводится через return), вторая часть - остаток для вывода впоследствии (выводится через out)
    string CutString(string line, out string outLine)
    {
        string cutLine;
        outLine = line;

        int maxLenth = UsefulFunctions.GetMaxSymbolsCount(dialog, line);

        // Если текст больше максимальной длины влезающей строчки
        if (outLine.Length > maxLenth) 
        {
            cutLine = outLine.Substring(0, maxLenth);
            outLine = outLine.Substring(maxLenth+1);
            return cutLine + "...";
        }
        else // Если меньше
        {
            cutLine = outLine;
            outLine = "";       // Если текст полнстью влезает, то возвращется пустая строчка. Она сигнализирует о полностью выведенном тексте
            return cutLine;
        }
    }


    public void NextSkipDialog()
    {
        if (!skipDialog)
        { 
            skipDialog = true;  // Скипаем постепенный вывод тескта
        } 
        else if (remainingText != "") // Проверка на оставшийся после обрезки текст
        {
            StartCoroutine(OutputDialog(remainingText));
        }
        else  // Выключаем диалогове окно, если текст кончился
        {
            nextSay();
            gameObject.SetActive(false);
        }
    }


    IEnumerator OutputDialog(string textDialog)
    {
        remainingText = "";     // Чистим оставшийся текст и диалоговое поле
        dialog.text = "";

        skipDialog = false;

        string cutLine = CutString(textDialog, out textDialog);

        foreach (char letter in cutLine)
        {
            if (!skipDialog)
            {
                dialog.text += letter;
                yield return new WaitForSeconds(outputDelay);
            }
            else
            {
                dialog.text = cutLine;
                break;
            }       
        }

        if (textDialog != "")
        {
            remainingText = textDialog;
        }
    }
}