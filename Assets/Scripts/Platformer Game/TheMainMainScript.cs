using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TheMainMainScript : MonoBehaviour
{
    public GameObject fade, levelText, phoneButton, currentLevel, pausePanel, player, darkSide, left, right, currentLevelTemp, dialogWindow;

    [Header("Animator")]
    public Animator animMap;
    public Animator animFridge;
    public Animator animHealthBag;
    public Animator phone;
    public Animator animShop; 
    public Animator animApteka;

    [Header("Inventories")]
    public GameObject fridge;
    public GameObject healthBag;
    public GameObject fridgeObject;
    public GameObject healthBagObject;

    //public bool rejected = false;
    //public bool cameraTranslate=false;
    //public int modeTranslate = 0;
    //public bool playerGoRight = false;

    [Header("Buttons")]
    public GameObject fridgeButton;
    public GameObject bedButton;
    public GameObject healthButton;
    public GameObject goInOutButton;
    public GameObject talkButton;
    public GameObject TPButton;
    public GameObject shopButton;
    public GameObject safeButton;
    public GameObject compButton;
    public GameObject seeButton;

    void Start()
    {
        if (GamePrefs.currentLevel != null)
            currentLevel = GamePrefs.currentLevel;
        GamePrefs.inDialog = false;
        StartDialog();
        if (GamePrefs.amountOfFood == 0)
        {
            GameObject temp = Instantiate(fridgeObject.GetComponent<ObjectProperties>().emptyFridgeMessage);
            temp.transform.SetParent(fridge.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j < GamePrefs.countsOfKindOfFood[i]; j++)
                {
                    GameObject temp = Instantiate(fridgeObject.GetComponent<ObjectProperties>().itemFridgePref[i]);
                    temp.transform.SetParent(fridge.transform);
                    temp.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        if (GamePrefs.amountOfHealth == 0)
        {
            GameObject temp = Instantiate(healthBagObject.GetComponent<ObjectProperties>().emptyHealthBagMessage);
            temp.transform.SetParent(healthBag.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j < GamePrefs.countsOfKindOfHealth[i]; j++)
                {
                    GameObject temp = Instantiate(healthBagObject.GetComponent<ObjectProperties>().itemHealthPref[i]);
                    temp.transform.SetParent(healthBag.transform);
                    temp.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (GamePrefs.amountOfFood == 0 && fridge.transform.childCount == 0)
        {
            GameObject temp = Instantiate(fridgeObject.GetComponent<ObjectProperties>().emptyFridgeMessage);
            temp.transform.SetParent(fridge.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
        if (GamePrefs.amountOfHealth == 0 && healthBag.transform.childCount == 0)
        {
            GameObject temp = Instantiate(healthBagObject.GetComponent<ObjectProperties>().emptyHealthBagMessage);
            temp.transform.SetParent(healthBag.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //private void FixedUpdate()
    //{
    //    if (scene == 8)
    //    {
    //        if (nearSafe)
    //        {
    //            safeButton.SetBool("startOpen", true);
    //            safeButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            safeButton.SetBool("startOpen", false);
    //            safeButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    //        }
    //    }
    //    if (rejected)
    //    {
    //        rejected = false;
    //        StartCoroutine(Call());
    //    }
    //    if (fade.activeSelf && fade.GetComponent<Image>().color.a == 0)
    //    {
    //        fade.SetActive(false);
    //        player.GetComponent<HeroControler>().inDialog = false;
    //        if (GamePrefs.firstSceneSixthDialog)
    //            StartDialog();
    //        else if (GamePrefs.firstSceneNinthDialog)
    //            StartDialog();
    //        else if (GamePrefs.firstSceneFourteenthDialog)
    //            StartDialog();
    //        else if (GamePrefs.firstSceneSixteenthDialog)
    //            StartDialog();
    //        levelText.SetActive(false);
    //    }
    //    if (fade.activeSelf && fade.GetComponent<Image>().color.a == 1)
    //    {
    //        if (GamePrefs.countOfDialog == 13)
    //            GamePrefs.firstSceneFourthDialog = true;
    //        else if (GamePrefs.countOfDialog == 26)
    //        {
    //            GamePrefs.firstSceneSixthDialog = true;
    //            gazeta.SetActive(true);
    //        }
    //        else if (GamePrefs.countOfDialog == 38)
    //            GamePrefs.firstSceneNinthDialog = true;
    //        else if (GamePrefs.countOfDialog == 70)
    //            GamePrefs.firstSceneFourteenthDialog = true;
    //        else if (GamePrefs.countOfDialog == 78)
    //            GamePrefs.firstSceneSixteenthDialog = true;
    //        levelText.SetActive(true);
    //        player.transform.position = new Vector3(bed.transform.position.x, player.transform.position.y);
    //    }
    //    if (!rejected && GamePrefs.firstSceneFourthDialog)
    //    {
    //        phone.GetComponent<Phone>().callerName = "Мама";
    //        phoneButton.GetComponent<Animator>().SetBool("call", true);
    //    }
    //    else if (scene == 3 && !rejected && (GamePrefs.thirdSceneSecondDialog || GamePrefs.thirdSceneSeventhDialog ||
    //        GamePrefs.thirdSceneTenthDialog) && player.transform.position.x > -12.36)
    //    {
    //        if (GamePrefs.thirdSceneSecondDialog)
    //            phone.GetComponent<Phone>().callerName = "Иван";
    //        if (GamePrefs.thirdSceneSeventhDialog)
    //            phone.GetComponent<Phone>().callerName = "Азиат";
    //        if (GamePrefs.thirdSceneTenthDialog)
    //            phone.GetComponent<Phone>().callerName = "Азиат";
    //        phoneButton.GetComponent<Animator>().SetBool("call", true);
    //    }
    //    else if (scene==19 && !rejected && GamePrefs.ninteenthSceneSecondDialog)
    //    {
    //        phone.GetComponent<Phone>().callerName = "Инспектор";
    //        phoneButton.GetComponent<Animator>().SetBool("call", true);
    //    }
    //    else phoneButton.GetComponent<Animator>().SetBool("call", false);
    //    if (scene == 1 && !inDialog)
    //    {
    //        if (GamePrefs.firstSceneEleventhDialog && GamePrefs.inout == 4 && player.GetComponent<Transform>().position.x > -0.04)
    //            StartDialog();
    //    }
    //    else if (scene == 2 && !inDialog)
    //    {
    //        if (GamePrefs.secondSceneThirdDialog && player.GetComponent<Transform>().position.x <= 21.46)
    //        {
    //            player.GetComponent<HeroControler>().Stop();
    //            left.SetActive(false);
    //            right.SetActive(false);
    //            vines.SetActive(true);
    //        }
    //        else if (GamePrefs.secondSceneThirdDialog && vines.GetComponent<SpriteRenderer>().sprite == vinesEnd)
    //        {
    //            vines.SetActive(false);
    //            StartDialog();
    //        }
    //    }
    //    else if (scene == 3 && !inDialog)
    //    {
    //        if (GamePrefs.thirdSceneFourteenthDiaog && player.GetComponent<Transform>().position.x > -0.30)
    //            StartDialog();
    //        else if (GamePrefs.thirdSceneFirstDialog && player.GetComponent<Transform>().position.x > 12.36)
    //            StartDialog();
    //        else if ((GamePrefs.thirdSceneFourthDialog || GamePrefs.thirdSceneThirteenthDialog) && player.GetComponent<Transform>().position.x < -24.05)
    //            StartDialog();
    //        else if (GamePrefs.thirdSceneEighthDialog && player.GetComponent<Transform>().position.x > 28.21)
    //            StartDialog();
    //        else if (GamePrefs.thirdSceneNinthDialog && player.GetComponent<Transform>().position.x < 14.67)
    //        {
    //            bandit1.transform.localScale = new Vector3(3.7f, 3.7f, 0);
    //            bandit2.transform.localScale = new Vector3(3.7f, 3.7f, 0);
    //            StartDialog();
    //        }
    //        else if (GamePrefs.thirdSceneFifteenthDialog && player.GetComponent<Transform>().position.x < -33.32)
    //            StartDialog();
    //        else if (GamePrefs.thirdSceneSixteenthDialog && player.GetComponent<Transform>().position.x < -31.31)
    //            StartDialog();
    //    }
    //    else if (scene == 4 && !inDialog)
    //    {
    //        if (GamePrefs.fourthSceneThirdDialog && player.GetComponent<Transform>().position.x > -15)
    //            StartDialog();
    //        else if (GamePrefs.fourthSceneFirstDialog && player.GetComponent<Transform>().position.x > -7.49)
    //            StartDialog();
    //        else if (GamePrefs.fourthSceneFifthDialog && player.GetComponent<Transform>().position.x > -7.49)
    //        {
    //            general.SetActive(true);
    //            StartDialog();
    //        }
    //        else if (GamePrefs.fourthSceneEighthDialog && player.GetComponent<Transform>().position.x > -6.5)
    //            StartDialog();
    //    }
    //    if (scene == 5 && !inDialog && GamePrefs.fifthSceneFirstDialog && player.GetComponent<Transform>().position.x > -0.55)
    //        StartDialog();
    //    if (scene == 6 && !inDialog && (GamePrefs.sixthSceneThirdDialog || GamePrefs.sixthSceneFifthDialog) && player.GetComponent<Transform>().position.x > 10)
    //        StartDialog();
    //    if (scene == 8 && !inDialog && GamePrefs.eightSceneEighthDialog && player.GetComponent<Transform>().position.x > 3.92)
    //        StartDialog();
    //    if (scene == 9 && !inDialog && GamePrefs.ninthSceneSecondDialog && player.GetComponent<Transform>().position.x > 4.60)
    //        StartDialog();
    //    if (scene == 10 && !inDialog)
    //    {
    //        if (GamePrefs.tenthSceneThirdDialog && kosoy.transform.position.x >= -7.6)
    //            StartDialog();
    //        else if (GamePrefs.tenthSceneFifthDialog && vines.GetComponent<SpriteRenderer>().sprite == vinesEnd)
    //        {
    //            vines.SetActive(false);
    //            StartDialog();
    //        }
    //        else if (GamePrefs.tenthSceneFourthDialog && bandit2.GetComponent<SpriteRenderer>().sprite == bandit2Die)
    //            StartDialog();
    //        else if (GamePrefs.tenthSceneSeventhDialog && unnamed.GetComponent<SpriteRenderer>().sprite == cloakPhoned)
    //            StartDialog();
    //    }
    //    if (scene == 13 && !inDialog && GamePrefs.thirteenthSceneFirstDialog && player.GetComponent<Transform>().position.x > 4.56)
    //        StartDialog();
    //    if (scene == 15 && !inDialog && GamePrefs.fifteenthSceneFirstDialog && player.GetComponent<Transform>().position.x < -6.36)
    //        StartDialog();
    //    if (scene == 19 && !inDialog && GamePrefs.ninteenthSceneFirstDialog && player.GetComponent<Transform>().position.x > 13.68)
    //        StartDialog();

    //    if (scene == 3 && unnamed.transform.position.x <= 7)
    //        unnamed.SetActive(false);

    //    if (cameraTranslate && modeTranslate == 0 && Mathf.Abs(transform.position.x - tolpa.transform.position.x) < 0.5)
    //    {
    //        cameraTranslate = false;
    //        StartDialog();
    //    }
    //    if (cameraTranslate && modeTranslate == 1 && Mathf.Abs(transform.position.x - player.transform.position.x) < 0.5)
    //    {
    //        cameraTranslate = false;
    //        StartDialog();
    //    }

    //    if (scene == 3 && playerGoRight && player.transform.position.x >= 33)
    //    {
    //        GetComponent<TrackingTheHero>().faded.SetActive(true);
    //    }

    //    if (scene == 10 && playerGoRight && player.transform.position.x >= -2.56f)
    //    {
    //        playerGoRight = false;
    //        player.GetComponent<HeroControler>().enabled = true;
    //        StartDialog();
    //    }

    //    if (scene == 3 && playerGoRight)
    //    {
    //        player.transform.position = Vector2.MoveTowards(player.transform.position, teleportToVinesPlace.transform.position, 14 * Time.deltaTime);
    //        player.transform.position = new Vector3(player.transform.position.x, -1.837947f);
    //    }
    //    if (scene == 10 && playerGoRight)
    //    {
    //        player.transform.position = Vector2.MoveTowards(player.transform.position, stopVinesPlace.transform.position, 14 * Time.deltaTime);
    //        player.transform.position = new Vector3(player.transform.position.x, -1.559961f);
    //    }
    //}

    //private void Update()
    //{
    //    if (cameraTranslate)
    //    {
    //        if (modeTranslate == 0)
    //        {
    //            transform.position = Vector2.MoveTowards(transform.position, tolpa.transform.position, 4 * Time.deltaTime);
    //            transform.position = new Vector3(transform.position.x, 0, -10);
    //        }
    //        else if (modeTranslate == 1)
    //        {
    //            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 4 * Time.deltaTime);
    //            transform.position = new Vector3(transform.position.x, 0, -10);
    //        }
    //    }
    //}

    public void StartDialog()
    {
        GamePrefs.inDialog = true;
        left.SetActive(false);
        right.SetActive(false);
        phoneButton.SetActive(false);
        phone.GetComponent<Animator>().SetBool("phone", false);
        player.GetComponent<HeroControler>().Stop();
        dialogWindow.GetComponent<PlatformerDialogs>().currentNode = 0;
        dialogWindow.GetComponent<PlatformerDialogs>().dialogue = Dialogue.Load(currentLevel.GetComponent<SceneProperties>().dialogs[currentLevel.GetComponent<SceneProperties>().countOfDialogs]);
        dialogWindow.GetComponent<PlatformerDialogs>().dialogueEnded = false;
        dialogWindow.GetComponent<PlatformerDialogs>().nextDialogButton.gameObject.SetActive(true);
        dialogWindow.GetComponent<PlatformerDialogs>().OutputDialogs();
    }

    //public void StartSup(string noteName, string noteText, int numberOfNote)
    //{
    //    switch (numberOfNote)  
    //    {
    //        case 0:
    //            phone.GetComponent<Phone>().noteMain.transform.GetChild(0).GetComponent<Text>().text = noteName;
    //            phone.GetComponent<Phone>().noteMain.GetComponent<SupClass>().textForNoteName = noteName;
    //            phone.GetComponent<Phone>().noteMain.GetComponent<SupClass>().textForNote = noteText;
    //            break;
    //        case 1:
    //            phone.GetComponent<Phone>().noteAdd1.transform.GetChild(0).GetComponent<Text>().text = noteName;
    //            phone.GetComponent<Phone>().noteAdd1.GetComponent<SupClass>().textForNoteName = noteName;
    //            phone.GetComponent<Phone>().noteAdd1.GetComponent<SupClass>().textForNote = noteText;
    //            break;
    //        case 2:
    //            phone.GetComponent<Phone>().noteAdd2.transform.GetChild(0).GetComponent<Text>().text = noteName;
    //            phone.GetComponent<Phone>().noteAdd2.GetComponent<SupClass>().textForNoteName = noteName;
    //            phone.GetComponent<Phone>().noteAdd2.GetComponent<SupClass>().textForNote = noteText;
    //            break;
    //    }
    //}

    public void CloseDialog()
    {
        GamePrefs.inDialog = false;
        left.SetActive(true);
        right.SetActive(true);
        phoneButton.SetActive(true);
        phoneButton.GetComponent<Animator>().SetBool("phoneButton", true);
        dialogWindow.GetComponent<PlatformerDialogs>().nextDialogButton.gameObject.SetActive(false);
        GamePrefs.countOfDialog++;
    }

    //IEnumerator Call()
    //{
    //    bool r = true;
    //    while (r)
    //    {
    //        yield return new WaitForSeconds(5);
    //        phoneButton.GetComponent<Animator>().SetBool("call", true);
    //        r = false;
    //        StopCoroutine(Call());
    //    }
    //}

    //public void NextPage()
    //{
    //    if (list1.activeSelf)
    //    {
    //        list1.SetActive(false);
    //        list2.SetActive(true);
    //    }
    //    else if (list2.activeSelf)
    //    {
    //        list2.SetActive(false);
    //        list3.SetActive(true);
    //    }
    //}

    //public void PrevPage()
    //{
    //    if (list2.activeSelf)
    //    {
    //        list2.SetActive(false);
    //        list1.SetActive(true);
    //    }
    //    else if (list3.activeSelf)
    //    {
    //        list3.SetActive(false);
    //        list2.SetActive(true);
    //    }
    //}

    //public void SignButton()
    //{
    //    sign.GetComponent<Image>().sprite = signn;
    //    StartCoroutine(SignWaiter());
    //}

    //public void CloseDocument()
    //{
    //    left.SetActive(true);
    //    right.SetActive(true);
    //    pause.SetActive(true);
    //    phoneButton.SetActive(true);
    //    player.GetComponent<HeroControler>().Stop();
    //    document.SetBool("document", false);
    //}

    //public void OpenGazet()
    //{
    //    left.SetActive(false);
    //    right.SetActive(false);
    //    pause.SetActive(false);
    //    phoneButton.SetActive(false);
    //    phone.GetComponent<Animator>().SetBool("phone", false);
    //    player.GetComponent<HeroControler>().Stop();
    //    gazetPanel.SetBool("gazeta", true);
    //}

    //public void CloseGazet()
    //{
    //    gazetPanel.SetBool("gazeta", false);
    //    left.SetActive(true);
    //    right.SetActive(true);
    //    pause.SetActive(true);
    //    phoneButton.SetActive(true);
    //    phoneButton.GetComponent<Animator>().SetBool("phoneButton", true);
    //    animPanel.SetBool("panelOpen", false);
    //    GetComponent<PlatformerDialogs>().currentNode = 0;
    //    StartDialog();
    //}

    public void OpenMap()
    {
        GamePrefs.inDialog = true;
        TPButton.SetActive(false);
        player.GetComponent<HeroControler>().Stop();
        animMap.SetBool("map", true);
    }

    public void CloseMap()
    {
        TPButton.SetActive(true);
        GamePrefs.inDialog = false;
        animMap.SetBool("map", false);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        player.SetActive(false);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        player.SetActive(true);
    }

    public void GoInOut()
    {
        GamePrefs.currentLevel = currentLevel = currentLevelTemp;
        GamePrefs.inout = 1;
        GetComponent<TrackingTheHero>().faded.SetActive(true);
    }

    public void OpenFridge()
    {
        GamePrefs.inDialog = true;
        fridgeButton.SetActive(false);
        player.GetComponent<HeroControler>().Stop();
        animFridge.SetBool("openShop", true);
        left.SetActive(false);
        right.SetActive(false);
        phoneButton.gameObject.SetActive(false);
    }

    public void CloseFridge()
    {
        GamePrefs.inDialog = false;
        fridgeButton.SetActive(true);
        animFridge.SetBool("openShop", false);
        left.SetActive(true);
        right.SetActive(true);
        phoneButton.gameObject.SetActive(true);
        phoneButton.GetComponent<Animator>().SetBool("phoneButton", true);
    }

    public void OpenHealthBag()
    {
        GamePrefs.inDialog = true;
        healthButton.SetActive(false);
        player.GetComponent<HeroControler>().Stop();
        animHealthBag.SetBool("healthBag", true);
        left.SetActive(false);
        right.SetActive(false);
        phoneButton.gameObject.SetActive(false);
    }

    public void CloseHealthBag()
    { 
        GamePrefs.inDialog = false;
        healthButton.SetActive(true);
        animHealthBag.SetBool("healthBag", false);
        left.SetActive(true);
        right.SetActive(true);
        phoneButton.SetActive(true);
        phoneButton.GetComponent<Animator>().SetBool("phoneButton", true);
    }

    public void OpenShop()
    {
        GamePrefs.inDialog = true;
        shopButton.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
        phone.GetComponent<Animator>().SetBool("phone", false);
        phoneButton.SetActive(false);
        if (currentLevel.GetComponent<SceneProperties>().sceneId==31)
            animShop.SetBool("openShop", true);
        else 
            animApteka.SetBool("openShop", true);
    }

    public void CloseShop()
    {
        GamePrefs.inDialog = false;
        shopButton.SetActive(true);
        left.SetActive(true);
        right.SetActive(true);
        phoneButton.SetActive(true);
        phoneButton.GetComponent<Animator>().SetBool("phoneButton", true);
        if (currentLevel.GetComponent<SceneProperties>().sceneId == 31)
            animShop.SetBool("openShop", false);
        else
            animApteka.SetBool("openShop", false);
    }

    public void GoBed()
    {
        GamePrefs.currentLevel = currentLevel;
        GamePrefs.runnerLevel = Random.Range(1, 2);
        SceneManager.LoadScene(2);
    }

    public void OpenComputer()
    {
        player.GetComponent<HeroControler>().Stop();
        //GamePrefs.inDialog = true;
        player.GetComponent<HeroControler>().cash += 1000;
        GamePrefs.currentLevel = currentLevel;
        GamePrefs.inout = 5;
        SceneManager.LoadScene(3);
    }

    //IEnumerator SignWaiter()
    //{
    //    yield return new WaitForSeconds(2);
    //    left.SetActive(true);
    //    right.SetActive(true);
    //    pause.SetActive(true);
    //    phoneButton.SetActive(true);
    //    phoneButton.GetComponent<Animator>().SetBool("phoneButton", true);
    //    dogovor.SetBool("dogovor", false);
    //    StartDialog();
    //    StopCoroutine(SignWaiter());
    //}
}
