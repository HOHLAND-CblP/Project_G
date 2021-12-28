using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NewPlatformer
{
    public class Level1 : MonoBehaviour
    {
        [Header("Hero")]
        [SerializeField]
        GameObject hero;


        [Header("Parametrs")]
        [SerializeField]
        Vector2 startPosition;


        [Header("Components")]
        [SerializeField]
        GameObject notificationPanel;


        [Header("Debug")]
        [SerializeField]
        int actionNumber = 0;

        
        void Start()
        {
            hero.transform.position = startPosition;

            Camera.main.GetComponent<NovellaDialContoler>().GLD = EndDialog;

            ActionCall();
        }

        void ActionCall()
        {
            switch (actionNumber)
            {
                case 0:
                    StartCoroutine(ZeroAction());
                    break;

                case 1:
                    notificationPanel.GetComponent<Notification>().NewNotification("Идите к двери справа.");
                    break;

                case 2:
                    notificationPanel.GetComponent<Notification>().NewNotification("Идите к серой двери.");
                    break;

                case 3:
                    notificationPanel.GetComponent<Notification>().NewNotification("Идите налево и начните диалог с солдатом.");
                    break;

                case 4:
                    SceneManager.LoadScene(4);
                    break;
            }
        }

        void EndDialog()
        {
            actionNumber++;
            ActionCall();
        }

        void Update()
        {
            switch(actionNumber)
            {
                case 1:
                    if (hero.GetComponent<Hero>().GetLocation() == 3)
                    {
                        actionNumber++;
                        ActionCall();
                    }
                    break;

                case 2:
                    if(hero.GetComponent<Hero>().GetLocation() == 1)
                    {
                        actionNumber++;
                        ActionCall();
                    }
                    break;
            }
        }

        public bool IsHaveDialogForThisPeople()
        {
            return true;
        }

        IEnumerator ZeroAction()
        {
            yield return new WaitForSeconds(0.5f);

            actionNumber++;
            ActionCall();
        }
    }
}