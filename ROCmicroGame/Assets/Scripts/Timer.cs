using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// variables en functions voor het uiterlijk van het fysieke gedeelte.
    /// </summary>
    public Slider timerSlider;
    public TextMeshProUGUI timerText;
    public float gameTime;
    private bool stoptimer;
    public Training[] trainingen;
    public VideoPlayer trainingsSpeler;

    /// <summary>
    /// TMP elements voor de tijd en knoppen.
    /// </summary>
    public TextMeshProUGUI trainingText;
    public TextMeshProUGUI tijdText;
    public TextMeshProUGUI knopText;

    // pauzeert de tijd en roept RandomTrainingKiezen(); op.
    void Start()
    {
        RandomTrainingKiezen();
        stoptimer = false;       
        Time.timeScale = 0;     
    }
    
    /// <summary>
    /// Calculaties voor de timer.
    /// Ook wordt de slider hier geupdate.
    /// </summary>
    void Update()
    {
        float time = gameTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (time <= 0)
        {
            stoptimer = true;
        }

        if (stoptimer == false)
        {
            timerText.text = textTime;
            timerSlider.value = time;
        }
    }

    /// <summary>
    /// pauze en start knop om de slider te laten werken(still zetten en aan zetten van de timer).
    /// </summary>
    public void Knopklik()
    {
        if (Time.timeScale == 1)
        {
            knopText.text = "Start";
            Time.timeScale = 0;
        } else
        {
            knopText.text = "Stop";
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Kiest een nieuwe oefening/training en zet de tijd weer op 0.
    /// </summary>
    public void Volgende()
    {
        RandomTrainingKiezen();
        stoptimer = false;
        knopText.text = "Start";
        Time.timeScale = 0;
    }

    /// <summary>
    /// kiest een random training uit en stuurt dat door.
    /// </summary>
    void RandomTrainingKiezen()
    {
        int rnd;
        rnd = Random.Range(0, trainingen.Length);
        TrainingKlaarzetten(rnd);
    }

    /// <summary>
    /// zet de training klaar door alle scriptable objects te pakken.
    /// </summary>
    void TrainingKlaarzetten(int gekozenTraining)
    {
        trainingsSpeler.clip = trainingen[gekozenTraining].trainingsVideo;
        trainingsSpeler.Play();
        gameTime = trainingen[gekozenTraining].tijd;
        trainingText.text = trainingen[gekozenTraining].name;
        tijdText.text = trainingen[gekozenTraining].tijd.ToString() + " Seconden";
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }
}
