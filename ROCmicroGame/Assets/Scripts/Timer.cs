using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Timer : MonoBehaviour
{
    public Slider timerSlider;
    public Text timerText;
    public float gameTime;
    private bool stoptimer;
    public Training[] trainingen;
    public VideoPlayer trainingsSpeler;
    public Text trainingText;
    public Text tijdText;
    
    // Start is called before the first frame update
    void Start()
    {
        RandomTrainingKiezen();
        stoptimer = false;       
        Time.timeScale = 0;     
    }

    // Update is called once per frame
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

    public void Knopklik()
    {
        Time.timeScale = 1;
    }

    public void Volgende()
    {
        RandomTrainingKiezen();
        Time.timeScale = 0;
    }

    void RandomTrainingKiezen()
    {
        int rnd;
        rnd = Random.Range(0, trainingen.Length);
        TrainingKlaarzetten(rnd);
    }

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
