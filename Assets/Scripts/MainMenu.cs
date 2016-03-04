using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

    //When clicked the game will load the primary game and can start playing
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    //When clicked the application will be closed
    public void ExitGame()
    {
        Application.Quit();
    }
}
