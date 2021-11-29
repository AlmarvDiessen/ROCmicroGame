using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReactieTest : MonoBehaviour
{
    bool klikbaar = false;
    float wachttijd;
    float kliktijd;
    List<float> tijden = new List<float>();
    public GameObject klikKnop;
    public GameObject teVroegScherm;
    int beurten = 5;
    public GameObject eindScherm;
    public Text beurtenText;
    public Text besteTijd;
    public Text gemiddeldeText;

    // Start is called before the first frame update
    void Start()
    {
        ZetTijd();
    }

    private void Awake()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        TellafenmaakKlaar();
        CheckStatusEnVeranderKleur();
        CheckOfBeurtenVoorbijZijn();
        beurtenText.text = "Beurten: " + beurten.ToString();
    }

    float Bestetijd(List<float> CheckLijst)
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
        besteTijd.text = "Beste Tijd: " + Bestetijd(tijden).ToString("F2");
        gemiddeldeText.text = "Gemiddeld: " + Gemiddelde(tijden).ToString("F2");
    }

    void ZetTijd()
    {
        wachttijd = Random.Range(2, 5);
    }

    void TellafenmaakKlaar()
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
            Time.timeScale = 0;
            ZetScore();
            eindScherm.SetActive(true);
        }
    }

    public void ResetGame()
    {
        beurten--;
        teVroegScherm.SetActive(false);
        Time.timeScale = 1;
        kliktijd = 0;
        ZetTijd();
    }

    void TeVroegGeklikt()
    {
        teVroegScherm.SetActive(true);
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