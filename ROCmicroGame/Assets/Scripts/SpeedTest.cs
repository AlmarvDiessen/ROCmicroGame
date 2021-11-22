using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeedTest : MonoBehaviour
{
    public GameObject houder;
    int gekozen;
    int laatstGekozen;
    int score;
    public Text scoreText;
    public Text tijdText;
    float tijd;
    public GameObject spel;
    public GameObject menu;
    public Text besteScoreText;
    public Text gemiddeldeKliks;
    public Text scoreTextMenu;



    // Start is called before the first frame update
    void Start()
    {
        tijd = 10;
        RandomGetalKiezen();
        ActiveerGekozenKnop(gekozen);
    }

    // Update is called once per frame
    void Update()
    {
        ZetTijdEnScore();
        DeactiveerSpelOpTijd();
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
        CheckVoorZelfde();
        ActiveerGekozenKnop(gekozen);
        score++;
    }

    void CheckVoorZelfde()
    {
        while(gekozen == laatstGekozen)
        {
            RandomGetalKiezen();
        }
    }

    void RandomGetalKiezen()
    {
        gekozen = Random.Range(0, houder.transform.childCount);
    }

    void ActiveerGekozenKnop(int gekozen)
    {    
        houder.transform.GetChild(gekozen).GetComponent<Button>().interactable = true;
        houder.transform.GetChild(gekozen).GetComponent<Image>().color = Color.green;
    }

    void DeActiveerGekozenKnop(int gekozen)
    {
        houder.transform.GetChild(gekozen).GetComponent<Button>().interactable = false;
        houder.transform.GetChild(gekozen).GetComponent<Image>().color = Color.white;
        laatstGekozen = gekozen;
    }

    public void Herstart()
    {
        SceneManager.LoadScene(2);
    }

    public void TerugNaarHoofdScherm()
    {
        SceneManager.LoadScene(1);
    }
}
