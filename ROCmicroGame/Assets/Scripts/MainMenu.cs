using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // GameObjects
    public GameObject menu;
    public GameObject msKnoppen;

    // zorgt ervoor dat de mentaal sport levels menu wordt geactiveert en het hoofd menu wordt gedeactiveert
    public void OpenMSLevelMenu()
    {
        menu.SetActive(false);
        msKnoppen.SetActive(true);
    }

    /// <summary>
    /// zet de menu aan en de msKnoppen uit.
    /// </summary>
    public void Terug()
    {
        menu.SetActive(true);
        msKnoppen.SetActive(false);
    }

    /// <summary>
    /// hieronder de functions die worden aangeroepen voor het veranderen van de scene. 
    /// </summary>
    public void LaadSnelheidTest()
    {
        SceneManager.LoadScene(1);
    }

    public void LaadSimonSays()
    {
       SceneManager.LoadScene(2);
    }

    public void LaadReactieTest()
    {
        SceneManager.LoadScene(3);
    }
    public void LaadTraining()
    {
        SceneManager.LoadScene(4);
    }
    public void LaadGrafieken()
    {
        SceneManager.LoadScene(5);
    }
    
}
