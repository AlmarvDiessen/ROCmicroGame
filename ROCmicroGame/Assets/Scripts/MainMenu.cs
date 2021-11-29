using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject msKnoppen;

    // zorgt ervoor dat de mentaal sport levels menu wordt geactiveert en het hoofd menu wordt gedeactiveert
    public void OpenMSLevelMenu()
    {
        menu.SetActive(false);
        msKnoppen.SetActive(true);
    }

    public void Terug()
    {
        menu.SetActive(true);
        msKnoppen.SetActive(false);
    }
    public void LaadSnelheidTest()
    {
        SceneManager.LoadScene(1);
    }

    public void ReactieVermogenTest()
    {
        SceneManager.LoadScene(3);
    }

    public void SimonSays()
    {
        SceneManager.LoadScene(2);
    }
}
