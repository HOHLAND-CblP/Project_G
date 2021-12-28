using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewPlatformer
{
    public class NovellaDialContoler : MonoBehaviour
    {
        public delegate void GetLevelDialog();

        [SerializeField]
        GameObject dialogPanel;

        public GetLevelDialog GLD;

        void Start()
        {
                    
        }

        public void NewDialog(string dia)
        {
            dialogPanel.GetComponent<NovellaDialog>().NewDialog(dia);
        }

        public void NewDialog(TextAsset dia)
        { 
            dialogPanel.GetComponent<NovellaDialog>().NewDialog(dia);
        }


        public void EndDialog()
        {
            GLD();
        }
    }
}