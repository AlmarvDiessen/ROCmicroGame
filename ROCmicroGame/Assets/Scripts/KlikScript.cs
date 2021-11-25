using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KlikScript : MonoBehaviour
{
    public bool activated;
    SpeedTest speedTest;
    float timer;

    private void Start()
    {
        speedTest = FindObjectOfType<SpeedTest>().GetComponent<SpeedTest>();
    }
    private void Update()
    {
        if (activated == false)
        {
            if (timer > 0)
            {            
                timer -= Time.deltaTime;
                GetComponent<Image>().color = Color.red;
            }
            if (timer < 0)
            {
                GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void Geklikt()
    {
        if (activated == true)
        {
            activated = false;
            timer = 0;
            speedTest.KnopGeklikt();
        } else
        {
            timer = 1;
            speedTest.score--;
        }
    }
}
