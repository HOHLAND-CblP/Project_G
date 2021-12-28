using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LolilopGamesLibrary;

namespace NewPlatformer
{
    public class NovellaDialog : MonoBehaviour
    {
        [Header("Parametrs")]
        [Min(0)]
        public float outputDelay;
        public Color32 wasntChosenCol;
        public Color32 wasChosenCol;

        int curSay;
        int lastSay;
        bool skipDialog;
        string remainingText;

        [Header("Components")]
        public NovellaDialogXml curDialog;
        public List<GameObject> dialogueParticipants;

        [Header("UI Components")]
        public GameObject dialogBlock;
        public GameObject answersBlock;
        public GameObject answerButtonPref;
        public GameObject contentBoxForAnswers;
        public GameObject leftCamPers;
        public GameObject rightCamPers;

        //Mods
        bool dialogMode;
        bool answerMode;

        void Awake()
        {
            contentBoxForAnswers = answersBlock.transform.GetChild(0).GetChild(0).gameObject;
        }

        void Update()
        {
            if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                if (dialogMode)
                {
                    if (!skipDialog)
                    {
                        skipDialog = true;
                    }
                    else if (remainingText != "") // Проверка на оставшийся после обрезки текст
                    {
                        StartCoroutine(OutputDialog(remainingText));
                    }
                    else
                    {
                        lastSay = curSay;
                        curSay = curDialog.says[curSay].nextSay;
                        NextSay();
                    }
                }
            }
        }

        public void NewDialog(TextAsset dia)
        {
            curDialog = NovellaDialogXml.Load(dia);
            gameObject.SetActive(true);

            curSay = 0;
            lastSay = 0;
            FirstSay();
        }
        public void NewDialog(string dia)
        {
            curDialog = NovellaDialogXml.Load(dia);
            gameObject.SetActive(true);

            curSay = 0;
            lastSay = 0;
            FirstSay();
        }


        void FirstSay()
        {
            if (dialogueParticipants[curDialog.says[0].idCharacter].transform.position.x < dialogueParticipants[curDialog.says[1].idCharacter].transform.position.x)
            {
                RightCamPers(curDialog.says[1].idCharacter);
                LeftCamPers(curDialog.says[0].idCharacter);
            }
            else
            {
                LeftCamPers(curDialog.says[1].idCharacter);
                RightCamPers(curDialog.says[0].idCharacter);
            }

            if (curDialog.says[0].text != null)
            {
                DialogMode();
            }
            else
            {
                AnswerMode();
            }
        }
            
        public void NextSay()
        {
            int idCharacter = curDialog.says[curSay].idCharacter;
            if (curDialog.says[curSay].answers == null)
            {
                if (dialogueParticipants[curDialog.says[curSay].idCharacter].transform.position.x < dialogueParticipants[curDialog.says[lastSay].idCharacter].transform.position.x)
                    LeftCamPers(idCharacter);
                else  
                    RightCamPers(idCharacter);
                
                DialogMode();
            }
            else
            {
                if (dialogueParticipants[curDialog.says[curSay].idCharacter].transform.position.x < dialogueParticipants[curDialog.says[lastSay].idCharacter].transform.position.x)
                    LeftCamPers(idCharacter);
                else
                    RightCamPers(idCharacter);

                AnswerMode();
            }
        }

        public void Answer(int answerNumber)
        {
            curDialog.says[curSay].answers[answerNumber].wasChosen = true;

            if (curDialog.says[curSay].answers[answerNumber].dialEnd)
            {
                answerMode = false;
                gameObject.SetActive(false);
                if (curDialog.idCharacters[0] != 0)
                    dialogueParticipants[curDialog.idCharacters[0]].GetComponent<NPC>().dialogFileName = NovellaDialogXml.Save(curDialog);
                Camera.main.GetComponent<NovellaDialContoler>().EndDialog();
                return;
            }

            lastSay = curSay;
            curSay = curDialog.says[curSay].answers[answerNumber].nextSay;
            NextSay();
        }


        void LeftCamPers(int idCharacter)
        {
            if (idCharacter == 0)
            {
                leftCamPers.transform.GetChild(0).GetComponent<Text>().text = dialogueParticipants[0].GetComponent<Hero>().nameHero;
                leftCamPers.transform.GetChild(1).GetComponent<Image>().sprite = dialogueParticipants[0].GetComponent<Hero>().iconFace;   
            }
            else
            {
                leftCamPers.transform.GetChild(0).GetComponent<Text>().text = dialogueParticipants[curDialog.says[0].idCharacter].GetComponent<NPC>().nameNPC;
                leftCamPers.transform.GetChild(1).GetComponent<Image>().sprite = dialogueParticipants[curDialog.says[0].idCharacter].GetComponent<NPC>().iconFace;
            }

            leftCamPers.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            leftCamPers.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            rightCamPers.transform.GetChild(0).GetComponent<Text>().color = new Color32(140, 140, 140, 255);
            rightCamPers.transform.GetChild(1).GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        }
        void RightCamPers(int idCharacter)
        { 

            if (idCharacter == 0)
            {
                rightCamPers.transform.GetChild(0).GetComponent<Text>().text = dialogueParticipants[0].GetComponent<Hero>().nameHero;
                rightCamPers.transform.GetChild(1).GetComponent<Image>().sprite = dialogueParticipants[0].GetComponent<Hero>().iconFace;
            }
            else
            {
                rightCamPers.transform.GetChild(0).GetComponent<Text>().text = dialogueParticipants[curDialog.says[0].idCharacter].GetComponent<NPC>().nameNPC;
                rightCamPers.transform.GetChild(1).GetComponent<Image>().sprite = dialogueParticipants[curDialog.says[0].idCharacter].GetComponent<NPC>().iconFace;
            }

            rightCamPers.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            rightCamPers.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            leftCamPers.transform.GetChild(0).GetComponent<Text>().color = new Color32(140, 140, 140, 255);
            leftCamPers.transform.GetChild(1).GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        }

        void DialogMode()
        {
            dialogMode = true;
            answerMode = false;
            dialogBlock.SetActive(true);
            answersBlock.SetActive(false);

            StartCoroutine(OutputDialog(curDialog.says[curSay].text));
        }
        void AnswerMode()
        {
            dialogMode = false;
            answerMode = true;
            dialogBlock.SetActive(false);
            answersBlock.SetActive(true);

            UsefulFunctions.DestroyAllChild(contentBoxForAnswers.transform);

            for (int i = 0; i < curDialog.says[curSay].answers.Length; i++)
            {
                GameObject but = Instantiate(answerButtonPref, contentBoxForAnswers.transform);
                int temp = i;
                but.GetComponent<Button>().onClick.AddListener(() => Answer(temp));
                but.GetComponent<Text>().text = curDialog.says[curSay].answers[i].text;
                if (curDialog.says[curSay].answers[i].wasChosen)
                    but.GetComponent<Text>().color = wasChosenCol;
                else
                    but.GetComponent<Text>().color = wasntChosenCol;
            }
        }

        string CutString(string line, out string outLine)
        {
            string cutLine;
            outLine = line;

            int maxLenth = UsefulFunctions.GetMaxSymbolsCount(dialogBlock.GetComponent<Text>(), line);

            // Если текст больше максимальной длины влезающей строчки
            if (outLine.Length > maxLenth)
            {
                cutLine = outLine.Substring(0, maxLenth);
                outLine = outLine.Substring(maxLenth + 1);
                return cutLine + "...";
            }
            else // Если меньше
            {
                cutLine = outLine;
                outLine = "";       // Если текст полнстью влезает, то возвращется пустая строчка. Она сигнализирует о полностью выведенном тексте
                return cutLine;
            }
        }


        IEnumerator OutputDialog(string textDialog)
        {
            Text diaText = dialogBlock.GetComponent<Text>();
            remainingText = "";     // Чистим оставшийся текст и диалоговое поле
            diaText.text = "";

            skipDialog = false;

            string cutLine = CutString(textDialog, out textDialog);
            

            foreach (char letter in cutLine)
            {
                if (!skipDialog)
                {
                    diaText.text += letter;
                    yield return new WaitForSeconds(outputDelay);
                }
                else
                {
                    diaText.text = cutLine;
                    break;
                }
            }
            skipDialog = true;

            if (textDialog != "")
            {
                remainingText = textDialog;
            }
            yield return new WaitForSeconds(outputDelay);
        }
    }
}