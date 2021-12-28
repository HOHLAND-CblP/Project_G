using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewPlatformer
{
    public class NPC : MonoBehaviour
    {
        [Header("Parametrs")]
        public int idNPC;
        public string nameNPC;
        public Sprite iconFace;

        [Header("Componets")]
        public TextAsset dialogText;
        public string dialogTextFilePath;
        public string dialogFileName;
        
        void Start()
        {
            dialogTextFilePath = Application.dataPath + "/" + dialogFileName;
        }

        void Update()
        {

        }
    }
}