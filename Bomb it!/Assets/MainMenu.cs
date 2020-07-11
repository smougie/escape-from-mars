using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DisableMainMenu()
    {
        gameObject.SetActive(false);
    }

    public void EnableMainMenu()
    {
        gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        // TODO delete print()
        print("QUIT!");
    }
}
