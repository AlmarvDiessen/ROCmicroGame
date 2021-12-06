using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ReactieTest : MonoBehaviour
{
    



    public Button settingsButton;
    public Button gameButton;
    bool klikbaar = false;
    float wachttijd;
    float kliktijd;
    List<float> tijden = new List<float>();
    public GameObject klikKnop;
    public GameObject teVroegScherm;
    int beurten = 5;
    public GameObject eindScherm;
    public TextMeshProUGUI beurtenText;
    public TextMeshProUGUI besteTijd;
    public TextMeshProUGUI gemiddeldeText;
    public TextMeshProUGUI besteGemiddelde;
    public GameObject pauseMenu;
    bool gedaan;
    /*
    //-----------------------

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

    void ZetListScore()
    {
        if (tijden.Count != 0 && gedaan == false)
        {
            PlayerPrefs.SetInt("AKG", PlayerPrefs.GetInt("AKG") + 1);
            PlayerPrefs.SetFloat("SN_" + PlayerPrefs.GetInt("AKG"), Gemiddelde(tijden));
            gedaan = true;
        }
    }

    void Start()
    {
        gedaan = false;
        ZetTijd();
        
    }

    private void Awake()
    {
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
        //score = Gemiddelde(tijden);
        //SaveList();
        //LoadList();
        ZetListScore();
        print(PlayerPrefs.GetInt("AKG"));
      //  print(PlayerPrefs.GetFloat("SN_" + PlayerPrefs.GetInt("AKG")));
        for (int i = 1; i < PlayerPrefs.GetInt("AKG") + 1; i++)
        {
            print(PlayerPrefs.GetFloat("SN_" + i));
        }


    }

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

    void ZetTijd()
    {
        wachttijd = Random.Range(2, 5);
    }

    void TellAfEnMaakKlaar()
    {   
        if (wachttijd > 0)
        {
            wachttijd -= Time.deltaTime;
            klikbaar = false;
        }
        if (wachttijd <= 0)
        {
            klikbaar = true;
            kliktijd += Time.deltaTime;
        }
    }

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

    public void ResetGame()
    {
        settingsButton.enabled = true;
        beurten--;
        teVroegScherm.SetActive(false);
        Time.timeScale = 1;
        kliktijd = 0;
        ZetTijd();
    }

    void TeVroegGeklikt()
    {
        teVroegScherm.SetActive(true);
        settingsButton.enabled = false;
        Time.timeScale = 0;
    }

    void CheckStatusEnVeranderKleur()
    {
        if (klikbaar == false)
        {
            klikKnop.GetComponent<Image>().color = Color.red;
        }
        else klikKnop.GetComponent<Image>().color = Color.green;
    }

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