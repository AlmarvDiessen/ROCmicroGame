using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SpeedTest : MonoBehaviour
{
    public GameObject houder;
    int gekozen;
    int laatstGekozen;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI tijdText;
    float tijd;
    public GameObject spel;
    public GameObject menu;
    public TextMeshProUGUI besteScoreText;
    public TextMeshProUGUI gemiddeldeKliks;
    public TextMeshProUGUI scoreTextMenu;
    int volgende;
    public GameObject pauseMenu;
    public GameObject knoppenHouder;
    public TextMeshProUGUI aftelText;
    bool spelGestart;
    float aftelTijd;

    // Start is called before the first frame update
    void Start()
    {       
        volgende = Random.Range(0, houder.transform.childCount);
        Time.timeScale = 1;
        tijd = 10;
        aftelTijd = 3;
        RandomGetalKiezen();
        ActiveerGekozenKnop(gekozen);
        aftelText.fontSizeMax = 500;
    }

    // Update is called once per frame
    void Update()
    {
        ActiveerSpelNaTijd();
    }

    void ActiveerSpelNaTijd()
    {
        if (aftelTijd > 1)
        {
            aftelTijd -= Time.deltaTime;
            aftelText.text = aftelTijd.ToString("F0");
        }
        else
        {
            aftelText.gameObject.SetActive(false);
            spelGestart = true;
        }
        if (spelGestart == true)
        {
            ZetTijdEnScore();
            DeactiveerSpelOpTijd();
            CheckVoorVolgende();
            if (score <= 0)
            {
                score = 0;
            }
        }
    }

    void ZetScoreOpMenu()
    {
        CheckVoorHighScore();
        float gemiddeld;
        gemiddeld = score;
        besteScoreText.text = "BesteScore: " + PlayerPrefs.GetInt("BestScore").ToString();
        gemiddeldeKliks.text = "Gemiddelde kliks: " + (gemiddeld / 10).ToString();
        scoreTextMenu.text = "Score: " + score.ToString();
    }

    void CheckVoorVolgende()
    {
        if (volgende == gekozen)
        {
            volgende = Random.Range(0, houder.transform.childCount);
        }
    }

    void CheckVoorHighScore()
    {
        if (score > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    void DeactiveerSpelOpTijd()
    {
        if (tijd <= 0)
        {
            spel.SetActive(false);
            menu.SetActive(true);
            ZetScoreOpMenu();
        }
    }

    void ZetTijdEnScore()
    {
        tijd -= Time.deltaTime;
        tijdText.text = "Tijd: " + tijd.ToString("0");
        scoreText.text = "Score: " + score.ToString();
    }

    public void KnopGeklikt()
    {
        DeActiveerGekozenKnop(gekozen);
        RandomGetalKiezen();
        gekozen = volgende;
        ActiveerGekozenKnop(gekozen);
        score++;
    }

    void RandomGetalKiezen()
    {
        gekozen = Random.Range(0, houder.transform.childCount);
    }

    void ActiveerGekozenKnop(int gekozen)
    {
        knoppenHouder.transform.GetChild(gekozen).GetComponent<KlikScript>().activated = true;
        knoppenHouder.transform.GetChild(gekozen).GetComponent<Image>().color = Color.green;
    }

    void DeActiveerGekozenKnop(int gekozen)
    {
        knoppenHouder.transform.GetChild(gekozen).GetComponent<KlikScript>().activated = false;
        knoppenHouder.transform.GetChild(gekozen).GetComponent<Image>().color = Color.white;
        laatstGekozen = gekozen;
    }

    public void Herstart()
    {
        SceneManager.LoadScene(1);
    }

    public void TerugNaarHoofdScherm()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        spel.SetActive(false);
        aftelText.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    public void SluitPauseMenu()
    {
        pauseMenu.SetActive(false);
        if (spelGestart == false)
        {
            aftelText.gameObject.SetActive(true);
        }
        spel.SetActive(true);
        Time.timeScale = 1;
    }
}
