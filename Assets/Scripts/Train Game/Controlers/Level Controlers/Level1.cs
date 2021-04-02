using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public delegate void DialogEnded();
public delegate void AnimationEnded();
public delegate void AddStation();

public class Level1 : MonoBehaviour
{
    [Header("Dialog Components")]
    public GameObject dialogPanel;
    public Vector2 dialogPanelPos;
    public TextAsset[] dialogs;

    [Header("Speed Panel")]
    public GameObject speedPanel;

    private DialogTrains curDialog;
    private int curSay;
    bool dialogEnded;


    [Header("Mission Script Components")]
    public Animator mainCamAnim;
    public GameObject redPointer_1;
    public GameObject redPointer_2;
    public GameObject trainPref;

    [Header("First Action Components")]
    public GameObject arrowFirstAct;
    public Vector2 trainFA_pos;
    private GameObject trainFA;
    private int countColFA;
    private int curWay;
    private bool colCompl = false;

    [Header("Second Action Components")]
    public Vector2 trainSA_pos_1;
    public Vector2 trainSA_pos_2;
    private GameObject trainSA;
    private int countColSA = 0;

    [Header("Third Action Components")]
    public Vector2 trainTA_pos;
    private GameObject trainTA;
    public Vector2 dialogPanelTA_Pos;
    private bool speedPanelIsActive = false;

    [Header("Fourth Action Components")]
    public AudioClip normalClip;
    public AudioClip randRespClip;
    public GameObject stationFA_1;
    public GameObject stationFA_2;
    public GameObject stationFA_3;
    public GameObject stationFA_4;
    public GameObject stationFA_5;
    private GameObject curTrain;
    private int countTrain;
    private bool misAct;


    // Список bool переменых контролирующих уровень в порядке следования
    bool firstDialog = true;
    bool firstAnimation = true;
    bool secondDialog = true;
    bool secondAnimation = true;
    bool thirdDialog = true;
    bool thirdAnimation = true;
    bool fourthDialog = true;
    bool fifthDialog = true;
    bool firstAction = true;
    bool sixthDialog = true;
    bool fourthAnimation = true;
    bool seventhDialog = true;
    bool secondAction = true;
    bool eighthDialog = true;
    bool fifthAnimation = true;
    bool thirdAction = true;
    bool ninthDialog = true;
    bool sixthAnimation = true;
    bool tenthDialog = true;
    bool seventhAnimation = true;
    bool eleventhDialog = true;
    bool eighthAnimation = true;
    bool fourthAction = true;
    bool fifthAction = true;
    bool twelfthDialog = true;



    void Start()
    {
        mainCamAnim.enabled = false;
        Camera.main.transform.position = new Vector3(-7, 0.2f, -10);
        Camera.main.orthographicSize = 5;
        enabled = false;
        dialogPanel.GetComponent<DialogPanel>().dialogEnded = DialogEnded;
        Camera.main.GetComponent<GameControler>().animEnd = AnimationEnded;
        Camera.main.GetComponent<GameControler>().enabled = false;
        Camera.main.GetComponent<GameControler>().blockSpeedPanel = true;
        curSay = 0;
        LevelControler();
    }


    private void Update()
    {
        if (firstAction && !sixthDialog)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                GameObject build = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

                if (build == arrowFirstAct)
                {
                    build.GetComponent<Building>().ClickOnTheCell();
                }
            }
        }

        if (thirdAction && !ninthDialog)
        {
            trainTA.transform.position = trainTA_pos;

            if (speedPanel.activeSelf && !speedPanelIsActive)
            {
                speedPanelIsActive = true;
                speedPanel.transform.GetChild(6).gameObject.SetActive(false);

                dialogPanel.GetComponent<RectTransform>().anchoredPosition = dialogPanelTA_Pos;
                dialogEnded = false;
                curDialog = DialogTrains.Load(dialogs[10]);
                StartCoroutine(TimeAfterDialog());
            }
            else if (speedPanel.activeSelf && dialogEnded)
            {
                speedPanel.SetActive(false);
                Destroy(trainTA.gameObject);
                LevelControler();
            }
        }

        if (fourthAction)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                GameObject build = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

                if (build != null)
                {
                    build.GetComponent<Building>().ClickOnTheCell();
                }
            }

            if (curTrain == null && misAct)
            {
                countTrain++;
                misAct = false;

                if (countTrain < 4)
                    FourthActionDialog();
                else
                    LevelControler();
            }
        }
    }


    void LevelControler()
    {
        if (!firstDialog)
        {
            curDialog = DialogTrains.Load(dialogs[0]);
            StartCoroutine(TimeAfterDialog());
            firstDialog = true;
        }
        else if (!firstAnimation)
        {
            mainCamAnim.enabled = true;
            mainCamAnim.SetBool("First Animation", true);
            firstAnimation = true;
        }
        else if (!secondDialog)
        {
            mainCamAnim.enabled = false;
            curDialog = DialogTrains.Load(dialogs[1]);
            StartCoroutine(TimeAfterDialog());
            secondDialog = true;
        }
        else if (!secondAnimation)
        {
            redPointer_1.SetActive(true);
            redPointer_2.SetActive(true);
            redPointer_1.GetComponent<Animator>().SetBool("Second Animation", true);
            redPointer_2.GetComponent<Animator>().SetBool("Second Animation", true);
            StartCoroutine(RedPointerAnim_1());
            secondAnimation = true;
        }
        else if (!thirdDialog)
        {
            curDialog = DialogTrains.Load(dialogs[2]);
            StartCoroutine(TimeAfterDialog());
            thirdDialog = true;
        }
        else if (!thirdAnimation)
        {
            redPointer_2.transform.eulerAngles = new Vector3(0, 0, 225);
            redPointer_2.transform.position = new Vector3(-10.635f, 2.367f, redPointer_2.transform.position.z);
            redPointer_1.GetComponent<Animator>().SetBool("Second Animation", true);
            redPointer_2.GetComponent<Animator>().SetBool("Second Animation", true);
            StartCoroutine(RedPointerAnim_1());
            thirdAnimation = true;
        }
        else if (!fourthDialog)
        {
            curDialog = DialogTrains.Load(dialogs[3]);
            StartCoroutine(TimeAfterDialog());
            fourthDialog = true;
        }
        else if (!fifthDialog)
        {
            redPointer_1.SetActive(false);
            redPointer_2.SetActive(false);
            curDialog = DialogTrains.Load(dialogs[4]);
            StartCoroutine(TimeAfterDialog());
            fifthDialog = true;
        }
        else if (!firstAction)
        {
            DialogFirstAction();
            enabled = true;
            firstAction = true;
        }
        else if (!sixthDialog)
        {
            enabled = false;
            curDialog = DialogTrains.Load(dialogs[5]);
            StartCoroutine(TimeAfterDialog());
            sixthDialog = true;
        }
        else if (!fourthAnimation)
        {
            redPointer_1.SetActive(true);
            redPointer_2.SetActive(true);
            redPointer_1.transform.eulerAngles = new Vector3(0, 0, 225);
            redPointer_1.transform.position = new Vector3(-10.635f, 2.367f, redPointer_1.transform.position.z);
            redPointer_2.transform.eulerAngles = new Vector3(0, 0, 180);
            redPointer_2.transform.position = new Vector3(-10.481f, 2, redPointer_2.transform.position.z);
            redPointer_1.GetComponent<Animator>().SetBool("Second Animation", true);
            redPointer_2.GetComponent<Animator>().SetBool("Second Animation", true);
            StartCoroutine(RedPointerAnim_1());
            fourthAnimation = true;
        }
        else if (!seventhDialog)
        {
            curDialog = DialogTrains.Load(dialogs[6]);
            StartCoroutine(TimeAfterDialog());
            seventhDialog = true;
        }
        else if (!secondAction)
        {
            redPointer_1.SetActive(false);
            redPointer_2.SetActive(false);
            SecondAction();
            secondAction = true;

        }
        else if (!eighthDialog)
        {
            curDialog = DialogTrains.Load(dialogs[8]);
            StartCoroutine(TimeAfterDialog());
            eighthDialog = true;
        }
        else if (!fifthAnimation)
        {
            mainCamAnim.enabled = true;
            mainCamAnim.SetBool("Fifth Animation", true);
            fifthAnimation = true;
        }
        else if (!thirdAction)
        {
            mainCamAnim.enabled = false;
            ThirdAction();
            thirdAction = true;
        }
        else if (!ninthDialog)
        {
            dialogPanel.GetComponent<RectTransform>().anchoredPosition = dialogPanelPos;
            curDialog = DialogTrains.Load(dialogs[11]);
            StartCoroutine(TimeAfterDialog());
            ninthDialog = true;
        }
        else if (!sixthAnimation)
        {
            mainCamAnim.enabled = true;
            mainCamAnim.SetBool("Sixth Animation", true);
            sixthAnimation = true;
        }
        else if (!tenthDialog)
        {
            mainCamAnim.enabled = false;
            curDialog = DialogTrains.Load(dialogs[12]);
            StartCoroutine(TimeAfterDialog());
            tenthDialog = true;
        }
        else if (!seventhAnimation)
        {
            stationFA_1.GetComponent<Station>().SetColor(new Color32(70, 108, 42, 255));
            seventhAnimation = true;
            LevelControler();
        }
        else if (!eleventhDialog)
        {
            curDialog = DialogTrains.Load(dialogs[13]);
            StartCoroutine(TimeAfterDialog());
            eleventhDialog = true;
        }
        else if (!eighthAnimation)
        {
            mainCamAnim.enabled = true;
            mainCamAnim.SetBool("Eighth Animation", true);
            eighthAnimation = true;
        }
        else if (!fourthAction)
        {
            Camera.main.GetComponent<GameControler>().blockSpeedPanel = false;
            countTrain = 0;
            enabled = true;
            FourthAction();
            fourthAction = true;
        }
        else if (!fifthAction)
        {
            GetComponent<AudioSource>().clip = randRespClip;
            GetComponent<AudioSource>().Play();
            StartCoroutine(FifthAction());
            StartCoroutine(FifthActionPause());
            fifthAction = true;
        }
        else if (!twelfthDialog)
        {
            curDialog = DialogTrains.Load(dialogs[14]);
            StartCoroutine(TimeAfterDialog());
            twelfthDialog = true;
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    void NextSay()
    {
        if (!curDialog.says[curSay].dialEnd)
        {
            curSay++;
            dialogPanel.GetComponent<DialogPanel>().NewDialog(curDialog.says[curSay].id, curDialog.says[curSay].text);
        }
        else
        {
            curSay = 0;
            LevelControler();
        }
    }



    void DialogFirstAction()
    {
        colCompl = false;

        string d0 = "Главный путь.";
        string d1 = "Второстепенный путь.";

        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                dialogPanel.GetComponent<DialogPanel>().NewDialog(1, d0);
                curWay = 0;
                break;
            case 1:
                dialogPanel.GetComponent<DialogPanel>().NewDialog(1, d1);
                curWay = 1;
                break;
        }
    }

    void TrainFAStart()
    {
        trainFA = Instantiate(trainPref, trainFA_pos, Quaternion.identity);
        trainFA.GetComponent<TrainScript>().speed = 0.5f;
        trainFA.SetActive(true);
    }

    public void ColFirstAction(int numberCol)
    {
        string dialogCompl = "Хорошо";
        string dialogFail = "Нет, нет, давайте еще раз.";

        if (firstAction && !sixthDialog)
        {
            Destroy(trainFA.gameObject);

            if (numberCol == curWay)
            {
                colCompl = true;
                countColFA++;
                if (countColFA == 5)
                {
                    LevelControler();
                }
                else
                    dialogPanel.GetComponent<DialogPanel>().NewDialog(1, dialogCompl);
            }
            else
            {
                dialogPanel.GetComponent<DialogPanel>().NewDialog(1, dialogFail);
            }
        }
    }



    void SecondAction()
    {
        switch (countColSA)
        {
            case 0:
                trainSA = Instantiate(trainPref, trainSA_pos_1, Quaternion.Euler(0, 0, 180));
                break;
            case 1:
                trainSA = Instantiate(trainPref, trainSA_pos_2, Quaternion.Euler(0, 0, 180));
                break;
        }
    }

    void DialogSecondAction()
    {
        curDialog = DialogTrains.Load(dialogs[7]);
        StartCoroutine(TimeAfterDialog());
    }

    public void ColSecondAction()
    {
        if (secondAction && !eighthDialog)
        {
            Destroy(trainSA.gameObject);

            countColSA++;
            switch (countColSA)
            {
                case 1:
                    DialogSecondAction();
                    break;
                case 2:
                    LevelControler();
                    break;
            }
        }
    }



    void ThirdAction()
    {
        enabled = true;
        trainTA = Instantiate(trainPref, trainTA_pos, Quaternion.identity);
        trainTA.GetComponent<TrainScript>().speed = 0;

        curDialog = DialogTrains.Load(dialogs[9]);
        StartCoroutine(TimeAfterDialog());
    }



    void FourthAction()
    {
        misAct = true;

        Vector2[] tempVec = new Vector2[1];

        switch (countTrain)
        {
            case 0:
                tempVec[0] = new Vector2(stationFA_2.transform.position.x + 0.501f, stationFA_2.transform.position.y);
                Camera.main.GetComponent<RespawnMode>().RespawnTrain_Modif(stationFA_2, tempVec, stationFA_1, new Color32(70, 108, 42, 255), 0, out curTrain);
                break;
            case 1:
                tempVec[0] = new Vector2(stationFA_2.transform.position.x + 0.501f, stationFA_2.transform.position.y);
                Camera.main.GetComponent<RespawnMode>().RespawnTrain_Modif(stationFA_2, tempVec, stationFA_3, Color.red, 0, out curTrain);
                break;
            case 2:
                tempVec[0] = new Vector2(stationFA_4.transform.position.x - 0.501f, stationFA_4.transform.position.y);
                Camera.main.GetComponent<RespawnMode>().RespawnTrain_Modif(stationFA_4, tempVec, stationFA_5, Color.yellow, 0, out curTrain);
                break;
            case 3:
                tempVec[0] = new Vector2(stationFA_1.transform.position.x + 0.501f, stationFA_1.transform.position.y);
                Camera.main.GetComponent<RespawnMode>().RespawnTrain_Modif(stationFA_1, tempVec, stationFA_3, new Color32(255,157,0,255), 0, out curTrain);
                break;
        }

        curTrain.GetComponent<TrainScript>().speed = 1;
    }

    void FourthActionDialog()
    {
        string dialog = "Хорошо, теперь ещё один";

        dialogPanel.GetComponent<DialogPanel>().NewDialog(1, dialog);
    }




    IEnumerator FifthAction()
    {
        while (true)
        {
            Camera.main.GetComponent<RespawnMode>().RandomRespawn();
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator FifthActionPause()
    {
        yield return new WaitForSeconds(5f);

        LevelControler();
    }


    IEnumerator TimeAfterDialog()
    {
        yield return new WaitForSeconds(0.5f);
        dialogPanel.GetComponent<DialogPanel>().NewDialog(curDialog.says[curSay].id, curDialog.says[curSay].text);
    }

    IEnumerator RedPointerAnim_1()
    {
        yield return new WaitForSeconds(1.5f);
        redPointer_1.GetComponent<Animator>().SetBool("Second Animation", false);
        redPointer_2.GetComponent<Animator>().SetBool("Second Animation", false);
        LevelControler();
    }

    void DialogEnded()
    {
        dialogEnded = true;

        if (firstAction && !sixthDialog)
            if (colCompl)
                DialogFirstAction();
            else
                TrainFAStart();
        else if (secondAction && !eighthDialog)
            SecondAction();
        else if (thirdAction && !ninthDialog)
            Camera.main.GetComponent<GameControler>().blockSpeedPanel = false;
        else if (fourthAction && !fifthAction)
            FourthAction();
        else
            NextSay();
    }

    void AnimationEnded()
    {
        if (firstAnimation && !secondDialog)
        {
            mainCamAnim.SetBool("First Animation", false);
            mainCamAnim.enabled = false;
        }
        if (fifthAnimation && !thirdAction)
        {
            mainCamAnim.SetBool("Fifth Animation", false);
            mainCamAnim.enabled = false;
        }

        if (sixthAnimation && !tenthDialog)
        {
            mainCamAnim.SetBool("Sixth Animation", false);
            mainCamAnim.enabled = false;
        }
        if (eighthAnimation && !fourthAction)
        {
            mainCamAnim.SetBool("Eighth Animation", false);
            mainCamAnim.enabled = false;
        }

        LevelControler();
    }
}