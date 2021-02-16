using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    [SerializeField]
    Animator phoneButton, phone;
    public GameObject mainMenu, statMenu, bankMenu, callMenu, notesMenu, contactsMenu, noteMain, noteAdd1, noteAdd2;
    [SerializeField]
    Text healthValue, satietyValue, cashValue, callerNameText, noteName, noteText, outputContactName, outputCallStatus;
    public bool call = false, outputCall = false;
    public string callerName;
    GameObject cam, player;
    int page = 0;

    private void FixedUpdate()
    {
        healthValue.text = player.GetComponent<HeroControler>().health.ToString() + "%";
        satietyValue.text = player.GetComponent<HeroControler>().satiety.ToString() + "%";
        cashValue.text = "Ваш счет:\n" + player.GetComponent<HeroControler>().cash.ToString() + " рублей";
        GamePrefs.nameForMainNote = noteMain.GetComponent<SupClass>().textForNoteName;
        GamePrefs.textForMainNote = noteMain.GetComponent<SupClass>().textForNote;
        GamePrefs.nameForAdd1Note = noteAdd1.GetComponent<SupClass>().textForNoteName;
        GamePrefs.textForAdd1Note = noteAdd1.GetComponent<SupClass>().textForNote;
        GamePrefs.nameForAdd2Note = noteAdd2.GetComponent<SupClass>().textForNoteName;
        GamePrefs.textForAdd2Note = noteAdd2.GetComponent<SupClass>().textForNote;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        phoneButton.SetBool("phoneButton", true);
        healthValue.text = player.GetComponent<HeroControler>().health.ToString() + "%";
        satietyValue.text = player.GetComponent<HeroControler>().ToString() + "%";
        cashValue.text = "Ваш счет:\n" + player.GetComponent<HeroControler>().cash.ToString() + " рублей";
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        noteMain.GetComponent<SupClass>().textForNoteName = GamePrefs.nameForMainNote;
        noteMain.GetComponent<SupClass>().textForNote = GamePrefs.textForMainNote;
        noteAdd1.GetComponent<SupClass>().textForNoteName = GamePrefs.nameForAdd1Note;
        noteAdd1.GetComponent<SupClass>().textForNote = GamePrefs.textForAdd1Note;
        noteAdd2.GetComponent<SupClass>().textForNoteName = GamePrefs.nameForAdd2Note;
        noteAdd2.GetComponent<SupClass>().textForNote = GamePrefs.textForAdd2Note;
        noteMain.transform.GetChild(0).GetComponent<Text>().text = noteMain.GetComponent<SupClass>().textForNoteName;
        noteAdd1.transform.GetChild(0).GetComponent<Text>().text = noteAdd1.GetComponent<SupClass>().textForNoteName;
        noteAdd2.transform.GetChild(0).GetComponent<Text>().text = noteAdd2.GetComponent<SupClass>().textForNoteName;
    }
    public void OpenPhone()
    {
        if (phoneButton.GetBool("call"))
        {
            phoneButton.SetBool("call", false);
            phoneButton.SetBool("phoneButton", false);
            call = true;
            Call(callerName);
            phone.SetBool("phone", true);
        }
        else
        {
            statMenu.SetActive(false);
            mainMenu.SetActive(true);
            bankMenu.SetActive(false);
            callMenu.SetActive(false);
            callMenu.transform.GetChild(0).gameObject.SetActive(true);
            callMenu.transform.GetChild(1).gameObject.SetActive(false);
            notesMenu.SetActive(false);
            notesMenu.transform.GetChild(0).gameObject.SetActive(true);
            notesMenu.transform.GetChild(1).gameObject.SetActive(false);
            contactsMenu.SetActive(false);
            phoneButton.SetBool("phoneButton", false);
            phone.SetBool("phone", true);
        }
    }

    public void ClosePhone()
    {
        if (!call)
        {
            phoneButton.SetBool("phoneButton", true);
            phone.SetBool("phone", false);
            statMenu.SetActive(false);
            mainMenu.SetActive(true);
            bankMenu.SetActive(false);
            callMenu.transform.GetChild(0).gameObject.SetActive(true);
            callMenu.transform.GetChild(1).gameObject.SetActive(false);
            notesMenu.SetActive(false);
            notesMenu.transform.GetChild(0).gameObject.SetActive(true);
            notesMenu.transform.GetChild(1).gameObject.SetActive(false);
            callMenu.SetActive(false);
            contactsMenu.SetActive(false);
            contactsMenu.transform.GetChild(page).gameObject.SetActive(false);
            page = 0;
            contactsMenu.transform.GetChild(page).gameObject.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        if (notesMenu.transform.GetChild(1).gameObject.activeSelf)
        {
            mainMenu.SetActive(false);
            statMenu.SetActive(false);
            bankMenu.SetActive(false);
            callMenu.transform.GetChild(0).gameObject.SetActive(true);
            callMenu.transform.GetChild(1).gameObject.SetActive(false);
            notesMenu.SetActive(true);
            notesMenu.transform.GetChild(0).gameObject.SetActive(true);
            notesMenu.transform.GetChild(1).gameObject.SetActive(false);
            callMenu.SetActive(false);
            contactsMenu.SetActive(false);
        }
        else if (!call)
        {
            mainMenu.SetActive(true);
            statMenu.SetActive(false);
            bankMenu.SetActive(false);
            notesMenu.SetActive(false);
            notesMenu.transform.GetChild(0).gameObject.SetActive(true);
            notesMenu.transform.GetChild(1).gameObject.SetActive(false);
            callMenu.SetActive(false);
            contactsMenu.SetActive(false);
        }
    }

    public void IntoStatMenu()
    {
        mainMenu.SetActive(false);
        statMenu.SetActive(true);
        bankMenu.SetActive(false);
        callMenu.SetActive(false);
        callMenu.transform.GetChild(0).gameObject.SetActive(true);
        callMenu.transform.GetChild(1).gameObject.SetActive(false);
        notesMenu.SetActive(false);
        notesMenu.transform.GetChild(0).gameObject.SetActive(true);
        notesMenu.transform.GetChild(1).gameObject.SetActive(false);
        contactsMenu.SetActive(false);
    }

    public void IntoBankMenu()
    {
        mainMenu.SetActive(false);
        statMenu.SetActive(false);
        callMenu.SetActive(false);
        callMenu.transform.GetChild(0).gameObject.SetActive(true);
        callMenu.transform.GetChild(1).gameObject.SetActive(false);
        notesMenu.SetActive(false);
        notesMenu.transform.GetChild(0).gameObject.SetActive(true);
        notesMenu.transform.GetChild(1).gameObject.SetActive(false);
        bankMenu.SetActive(true);
        contactsMenu.SetActive(false);
    }

    public void IntoNotesMenu()
    {
        mainMenu.SetActive(false);
        statMenu.SetActive(false);
        callMenu.SetActive(false);
        bankMenu.SetActive(false);
        notesMenu.SetActive(true);
        notesMenu.transform.GetChild(0).gameObject.SetActive(true);
        notesMenu.transform.GetChild(1).gameObject.SetActive(false);
        contactsMenu.SetActive(false);
    }

    public void IntoContactsMenu()
    {
        mainMenu.SetActive(false);
        statMenu.SetActive(false);
        callMenu.SetActive(false);
        bankMenu.SetActive(false);
        notesMenu.SetActive(false);
        notesMenu.transform.GetChild(0).gameObject.SetActive(true);
        notesMenu.transform.GetChild(1).gameObject.SetActive(false);
        contactsMenu.SetActive(true);
    }

    public void OpenNoteMain()
    {
        notesMenu.transform.GetChild(0).gameObject.SetActive(false);
        noteName.text = noteMain.GetComponent<SupClass>().textForNoteName;
        noteText.text = noteMain.GetComponent<SupClass>().textForNote;
        notesMenu.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OpenNoteAdd1()
    {
        notesMenu.transform.GetChild(0).gameObject.SetActive(false);
        noteName.text = noteAdd1.GetComponent<SupClass>().textForNoteName;
        noteText.text = noteAdd1.GetComponent<SupClass>().textForNote;
        notesMenu.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OpenNoteAdd2()
    {
        notesMenu.transform.GetChild(0).gameObject.SetActive(false);
        noteName.text = noteAdd2.GetComponent<SupClass>().textForNoteName;
        noteText.text = noteAdd2.GetComponent<SupClass>().textForNote;
        notesMenu.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Answer()
    {
        //cam.GetComponent<TheMainMainScript>().StartDialog();
        call = false;
    }

    public void Reject()
    {
        //cam.GetComponent<TheMainMainScript>().rejected = true;
        call = false;
        ClosePhone();
    }

    public void Call(string name)
    {
        callerNameText.text = name;
        //cam.GetComponent<TheMainMainScript>().rejected = false;
        statMenu.SetActive(false);
        mainMenu.SetActive(false);
        bankMenu.SetActive(false);
        contactsMenu.SetActive(false);
        notesMenu.SetActive(false);
        notesMenu.transform.GetChild(0).gameObject.SetActive(true);
        notesMenu.transform.GetChild(1).gameObject.SetActive(false);
        callMenu.SetActive(true);
        callMenu.transform.GetChild(0).gameObject.SetActive(true);
        callMenu.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OutputCall(int callerID)
    {
        call = true;
        switch (callerID)
        {
            case 0:
                outputContactName.text = "Иван";
                StartCoroutine(OutputCall());
                break;
            case 1:
                outputContactName.text = "Мама";
                StartCoroutine(OutputCall());
                outputCall = true;
                break;
            case 2:
                outputContactName.text = "Генерал";
                StartCoroutine(OutputCall());
                break;
            case 3:
                outputContactName.text = "Инспектор";
                StartCoroutine(OutputCall());
                break;
        }
    }

    public void nextPage()
    {
        if (page != 5)
        {
            contactsMenu.transform.GetChild(page).gameObject.SetActive(false);
            page++;
            contactsMenu.transform.GetChild(page).gameObject.SetActive(true);
        }
    }

    public void prevPage()
    {
        if (page != 0)
        {
            contactsMenu.transform.GetChild(page).gameObject.SetActive(false);
            page--;
            contactsMenu.transform.GetChild(page).gameObject.SetActive(true);
        }
    }

    IEnumerator OutputCall()
    {
        mainMenu.SetActive(false);
        statMenu.SetActive(false);
        callMenu.SetActive(true);
        callMenu.transform.GetChild(0).gameObject.SetActive(false);
        callMenu.transform.GetChild(1).gameObject.SetActive(true);
        bankMenu.SetActive(false);
        notesMenu.SetActive(false);
        contactsMenu.SetActive(false);
        outputCallStatus.text = "Звонок абоненту, подождите...";
        yield return new WaitForSeconds(3);
        if (outputCall)
        {
            ClosePhone();
            outputCall = false;
            call = false;
            //cam.GetComponent<TheMainMainScript>().StartDialog();
            StopCoroutine(OutputCall());
        }
        else
        {
            outputCall = false;
            call = false;
            outputCallStatus.text = "Абонент не доступен...";
            yield return new WaitForSeconds(2);
            IntoContactsMenu();
            StopCoroutine(OutputCall());
        }
    }
}
