using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ReactieTest : MonoBehaviour
{
    // buttons
    public Button settingsButton;
    public Button gameButton;

    // values en bools voor regels
    public GameObject klikKnop;
    bool klikbaar = false;
    float wachtTijd;
    float kliktijd;
    
    // list voor tijden
    List<float> tijden = new List<float>();
    
    // GameObjects voor tevroegScherme en eindScherm
    public GameObject teVroegScherm;
    public GameObject eindScherm;

    // int voor aantal beurten
    int beurten = 5;

    // TMP componenten
    public TextMeshProUGUI beurtenText;
    public TextMeshProUGUI besteTijd;
    public TextMeshProUGUI gemiddeldeText;
    public TextMeshProUGUI besteGemiddelde;

    // GameObject voor pauseMenu
    public GameObject pauseMenu;

    // gedaan bool voor de zetListScore
    //bool gedaan;


    /*
    //----------------------- 

    ///summary
    /// een soort functie om de scores opteslaan in playerprefs en te veranderen weer in een list. dit is niet gebruikt ivm problemen en weinig tijd.



    public List<float> reactieTestScores = new List<float>();
    private int savedListCount;
    public float score;

    public void SaveList()
    {
        for(int i = 0; i < reactieTestScores.Count; i++)
        {
            PlayerPrefs.SetFloat("ReactieTestScores" + i, reactieTestScores[i]);
        }

        PlayerPrefs.SetInt("aantalRTScores", reactieTestScores.Count);
    }

    public void LoadList()
    {
        //reactieTestScores.Clear();
        savedListCount = PlayerPrefs.GetInt("aantalRTScores");
        Debug.Log(savedListCount);

        for(int i = 0; i < savedListCount; i++)
        {
            float score = PlayerPrefs.GetFloat("ReactieTestScores" + i);
            reactieTestScores.Add(score);
            Debug.Log(reactieTestScores[i]);
        }
    }

    //-----------------------
    */

    /* score mechanic. helaas niet gebruikt IVM weinig tijd en float problemen.
    void ZetListScore()
    {
        if (tijden.Count != 0 && gedaan == false)
        {
            // AKG is aantal keren gespeelt (de count van de list).
            PlayerPrefs.SetInt("AKG", PlayerPrefs.GetInt("AKG") + 1);

            // SN is score nieuw (nieuwe score)
            PlayerPrefs.SetFloat("SN_" + PlayerPrefs.GetInt("AKG"), Gemiddelde(tijden));
            gedaan = true;
        }
    }
    */

    void Start()
    {
        //gedaan = false;
        ZetTijd();
        
    }

    private void Awake()
    {
        // zorgt ervoor dat het spel start door tijd te laten lopen
        Time.timeScale = 1;
    }

    void Update()
    {
        TellAfEnMaakKlaar();
        CheckStatusEnVeranderKleur();
        CheckOfBeurtenVoorbijZijn();
        beurtenText.text = "Beurten: " + beurten.ToString();
    }

    float BesteTijd(List<float> CheckLijst)
    {
        float besteTijd = 0;
        besteTijd = Mathf.Min(CheckLijst.ToArray());
        return besteTijd;
    }

     float Gemiddelde(List<float> CheckLijst)
     {
        float gemiddeld = 0;
        foreach (float item in CheckLijst)
        {
            gemiddeld += item;
        }
        gemiddeld = gemiddeld / CheckLijst.Count;
        return gemiddeld;
     }

    void ZetScore()
    {
        besteTijd.text = "Beste Tijd: " + BesteTijd(tijden).ToString("F3");
        gemiddeldeText.text = "Gemiddeld: " + Gemiddelde(tijden).ToString("F3");
        CheckVoorHighScoreEnZet(Gemiddelde(tijden));
        besteGemiddelde.text = "Beste Gemiddelde: " + PlayerPrefs.GetFloat("GBT").ToString("F3");
        
        /*
        niet gebruikte variabelen, methods en functions

        //score = Gemiddelde(tijden);
        //SaveList();
        //LoadList();
        //ZetListScore();
        */


        /* Weg gehaald IVM problemen met over zetten naar grafieken, float problemen en weinig tijd.
        
        print(PlayerPrefs.GetInt("AKG"));
        // print(PlayerPrefs.GetFloat("SN_" + PlayerPrefs.GetInt("AKG")));
        for (int i = 1; i < PlayerPrefs.GetInt("AKG") + 1; i++)
        {
            print(PlayerPrefs.GetFloat("SN_" + i));
        }

        */

    }


    /// <summary>
    /// Kijkt of er een highscore is en als er een highscore is vult hij de highscore in.
    /// </summary>
    void CheckVoorHighScoreEnZet(float tijdBehaald)
    {
        if (PlayerPrefs.GetFloat("GBT") == 0)
        {
            PlayerPrefs.SetFloat("GBT", tijdBehaald);
        }
        if (PlayerPrefs.GetFloat("GBT") > tijdBehaald)
        {
            PlayerPrefs.SetFloat("GBT", tijdBehaald);
        }
    }

    /// <summary>
    /// Randomized de tijd dat het duurt voor het scherm groen wordt.
    /// </summary>
    void ZetTijd()
    {
        wachtTijd = Random.Range(2, 5);
    }


    /// <summary>
    /// Telt af en zorgt ervoor dat die klikbaar word waneer de wachttijd voorbij is.
    /// </summary>
    void TellAfEnMaakKlaar()
    {   
        if (wachtTijd > 0)
        {
            wachtTijd -= Time.deltaTime;
            klikbaar = false;
        }
        if (wachtTijd <= 0)
        {
            klikbaar = true;
            kliktijd += Time.deltaTime;
        }
    }

    /// <summary>
    /// Als die geklikt is dan checkt die of die tevroeg is ja of nee, als die niet tevroeg is geklikt voegt die de tijd toe. 
    /// </summary>
    public void Geklikt()
    {
        if (klikbaar == true)
        {
            tijden.Add(kliktijd);
            ResetGame();
        }
        if (klikbaar == false)
        {
            TeVroegGeklikt();
        }
    }

    /// <summary>
    /// check voor als je alle beurten heb gespeeld zo ja dan krijg je je score te zien en de game pauzeerd dan
    /// </summary>
    void CheckOfBeurtenVoorbijZijn()
    {
        if (beurten <= 0)
        {
            Debug.Log("works");//------------------------
            Time.timeScale = 0;
            ZetScore();
            settingsButton.enabled = false;
            eindScherm.SetActive(true);
        }
    }
    /// <summary>
    /// dit zet een nieuw beurt klaar
    /// </summary>
    public void ResetGame()
    {
        settingsButton.enabled = true;
        beurten--;
        teVroegScherm.SetActive(false);
        Time.timeScale = 1;
        kliktijd = 0;
        ZetTijd();
    }
    /// <summary>
    /// laat het scherm zien als je tevroeg heb geklikt
    /// </summary>
    void TeVroegGeklikt()
    {
        teVroegScherm.SetActive(true);
        settingsButton.enabled = false;
        Time.timeScale = 0;
    }

    /// <summary>
    /// zet de button op rood als het niet de button is die je moest klikken
    /// </summary>
    void CheckStatusEnVeranderKleur()
    {
        if (klikbaar == false)
        {
            klikKnop.GetComponent<Image>().color = Color.red;
        }
        else klikKnop.GetComponent<Image>().color = Color.green;
    }

    /// <summary>
    /// fuction voor de pauze menu's
    /// </summary>
    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        gameButton.enabled = false;
        pauseMenu.SetActive(true);
    }

    public void Doorgaan()
    {
        pauseMenu.SetActive(false);
        gameButton.enabled = true;
        Time.timeScale = 1;
    }

    public void TerugNaarHoofdScherm()
    {
        SceneManager.LoadScene(0);
    }

    public void Opnieuw()
    {      
        SceneManager.LoadScene(3);
    }
}
/*
float currentScore;
float total;
int amountScores++;
float gemmiddeldeScoreOverall = totalScores / amountScores;
 */