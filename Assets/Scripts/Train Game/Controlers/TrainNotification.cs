using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TrainNotification : MonoBehaviour
{
    [Header("Notice Prefab")]
    [SerializeField]
    GameObject noticePref;
    List<GameObject> notices;

    private void Start()
    {
        notices = new List<GameObject>();
    }

    public void NewNotice(string text)
    {
        notices.Add(Instantiate(noticePref, transform));
        notices[notices.Count - 1].transform.GetChild(0).GetComponent<Text>().text = text;

        StartCoroutine(TimerBeforeDeletion(notices[notices.Count - 1]));

        while (notices.Count > 2)
        {
            Destroy(notices[0].gameObject);
            notices.RemoveAt(0);
        }
    }


    IEnumerator TimerBeforeDeletion(GameObject notice)
    {
        yield return new WaitForSeconds(3);
        if (notice)
            Destroy(notice.gameObject);
    }
}
