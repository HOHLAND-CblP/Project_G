using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour 
{    
    public int scene;
    public void LoadScene()
    {
        //if (PlayerPrefs.HasKey("scene") && SceneManager.sceneCountInBuildSettings==0)
        //    SceneManager.LoadScene(PlayerPrefs.GetInt("scene"));
        //else
        SceneManager.LoadScene(scene);
    }
}
