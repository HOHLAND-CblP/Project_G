using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public void BackToMenu()
    {
        
        SceneManager.LoadScene(0);
    }
}
