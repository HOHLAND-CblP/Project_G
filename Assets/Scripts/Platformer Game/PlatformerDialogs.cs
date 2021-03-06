﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class PlatformerDialogs : MonoBehaviour
{
    public string line;
    bool k = true, waitngAnswer, overSpace=false; 
    public bool dialogueEnded = false;
    public string words = "";
    string a;
    [SerializeField]
    Text dialogText, nameText;
    float outputDelay=0.1f;
    public Dialogue dialogue;
    public GameObject[] answers;
    public Button nextDialogButton;
    public int currentNode = 0;
    public Sprite[] faces;
    public Image face;
    public GameObject arrow, cam, canvas;
    public GameObject[] partipicipants;

    private void Start()
    {
        answers[0].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(But1);
        answers[1].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(But2);
        answers[2].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(But3);
        nextDialogButton.onClick.AddListener(ButtonAhead);
    }
    public void OutputDialogs()
    {
        dialogText.text = "";
        if (!dialogueEnded)
        {
            if (dialogue.nodes[currentNode].text == "" && answers.Length!=0)
            {
                transform.position = new Vector3(partipicipants[0].transform.position.x, transform.position.y, transform.position.z);
                DialogPosition();
                arrow.transform.position = new Vector3(partipicipants[0].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                waitngAnswer = true;
                face.sprite = faces[0];
                nameText.text = "Семен";
                for (int i = 0; i < dialogue.nodes[currentNode].answers.Length; i++)
                {
                    answers[i].transform.GetChild(0).GetComponent<Text>().text = dialogue.nodes[currentNode].answers[i].text;
                    answers[i].SetActive(true);
                }
            }
            else
            {
                transform.position = new Vector3(partipicipants[dialogue.nodes[currentNode].participant].transform.position.x, transform.position.y, transform.position.z);
                DialogPosition();
                arrow.transform.position = new Vector3(partipicipants[dialogue.nodes[currentNode].participant].transform.position.x, arrow.transform.position.y);
                face.sprite = faces[dialogue.nodes[currentNode].face];
                line = dialogue.nodes[currentNode].name;
                nameText.text = line;
                line = dialogue.nodes[currentNode].text;
                StartCoroutine(OutputDelay());
            }
        }
    }

    void DialogPosition()
    {
        if (GetComponent<RectTransform>().localPosition.x + GetComponent<RectTransform>().rect.width / 2 > canvas.GetComponent<RectTransform>().rect.width / 2 - 10)
            GetComponent<RectTransform>().localPosition = new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2 - GetComponent<RectTransform>().rect.width / 2 - 10,
                GetComponent<RectTransform>().localPosition.y, GetComponent<RectTransform>().localPosition.z);

        else if (GetComponent<RectTransform>().localPosition.x - GetComponent<RectTransform>().rect.width / 2 < -canvas.GetComponent<RectTransform>().rect.width / 2 + 10)
            GetComponent<RectTransform>().localPosition = new Vector3(-canvas.GetComponent<RectTransform>().rect.width / 2 + GetComponent<RectTransform>().rect.width / 2 + 10,
                GetComponent<RectTransform>().localPosition.y, GetComponent<RectTransform>().localPosition.z);

    }

    string CutString(string line)
    {
        a = "";
        string[] b = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        words = "";
        int j = 0;
        if (line.Length > 171)
        {
            overSpace = true;
            while (a.Length + b[j].Length < 160)
            {
                a += b[j]+" ";
                j++;
            }
            for (; j < b.Length; j++)
            {
                words += b[j]+" ";
            }
            a += "...";
            return a;
        }
        else return line;
    }

    public void But1() { Answer(0); }
    public void But2() { Answer(1); }
    public void But3() { Answer(2); }

    public void ButtonAhead()
    {
        if (!waitngAnswer)
        {

            if (!k)
            {
                k = true;
                StopCoroutine(OutputDelay());
                outputDelay = 0;
                if (overSpace)
                {
                    dialogText.text = "";
                    dialogText.text = a;
                    overSpace = false;
                }
                else
                {
                    dialogText.text = "";
                    dialogText.text = line;
                }
            }
            else
            {
                if (words.Length != 0)
                {
                    line = words;
                    words = "";
                    dialogText.text = "";
                    StartCoroutine(OutputDelay());
                }
                else if (dialogue.nodes[currentNode].end == "true") dialogueEnded = true;
                else if (dialogue.nodes[currentNode].answers.Length > 0)
                {
                    waitngAnswer = true;
                    dialogText.text = "";
                    face.sprite = faces[0];
                    name = "Семен";
                    for (int i = 0; i < dialogue.nodes[currentNode].answers.Length; i++)
                    {
                        answers[i].transform.GetChild(0).GetComponent<Text>().text = dialogue.nodes[currentNode].answers[i].text;
                        answers[i].SetActive(true);
                    }
                }
                else if (!dialogueEnded)
                {
                    line = dialogue.nodes[currentNode].text;
                    currentNode = System.Convert.ToInt32(dialogue.nodes[currentNode].nextNode);
                    OutputDialogs();
                }
                if (dialogueEnded)
                {
                    dialogText.text = "";
                    cam.GetComponent<TheMainMainScript>().CloseDialog();
                }
            }
        }
    }

    public void Answer(int id)
    {
        waitngAnswer = false;
        GamePrefs.chooseButton = id;
        if ((dialogue.nodes[currentNode].end == "true")) 
        {
            for (int i = 0; i < dialogue.nodes[currentNode].answers.Length; i++)
                answers[i].SetActive(false);
            cam.GetComponent<TheMainMainScript>().CloseDialog(); 
            return; 
        }
        dialogText.text = "";
        for (int i = 0; i < dialogue.nodes[currentNode].answers.Length; i++)
            answers[i].SetActive(false);
        currentNode = dialogue.nodes[currentNode].answers[id].nextNode;
        line = dialogue.nodes[currentNode].text;
        OutputDialogs();
    }

    IEnumerator OutputDelay()
    {
        float temp = outputDelay;
        k = false;
        foreach (char letter in CutString(line).ToCharArray())
        {
            if (!k)
            {
                dialogText.text += letter;
                yield return new WaitForSeconds(outputDelay);
            }
        }
        outputDelay = temp;
        k = true;
    }
}
