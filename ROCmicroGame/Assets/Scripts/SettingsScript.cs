using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsScript : MonoBehaviour
{

    public GameObject game;
    public GameObject pauseMenu;
    public TextMeshProUGUI knopText;

    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        game.SetActive(false);

    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        game.SetActive(true);
        if (knopText.text == "Stop")
        {
            Time.timeScale = 1;
        }

    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
