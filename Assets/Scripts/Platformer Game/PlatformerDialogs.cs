using System.Collections;
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
    float outputDelay;
    public Dialogue dialogue;
    public GameObject[] answers;
    public Button nextDialogButton;
    public int currentNode = 0;
    public Sprite[] faces;
    public Image face;

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
                line = dialogue.nodes[currentNode].face;
                face.sprite = faces[System.Convert.ToInt32(line)];
                line = dialogue.nodes[currentNode].name;
                nameText.text = line;
                line = dialogue.nodes[currentNode].text;
                StartCoroutine(OutputDelay());
            }
        }
    }

    string CutString(string line)
    {
        a = "";
        string[] b = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        words = "";
        int j = 0;
        if (line.Length > 410)
        {
            overSpace = true;
            while (a.Length + b[j].Length < 380)
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
                    //GetComponent<TheMainMainScript>().CloseDialog();
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
            //GetComponent<TheMainMainScript>().CloseDialog(); 
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
