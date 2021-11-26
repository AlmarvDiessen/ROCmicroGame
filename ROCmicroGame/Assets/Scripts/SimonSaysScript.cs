using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SimonSaysScript : MonoBehaviour
{
    //int level
    public int level;
    //array voor de buttons
    public Button[] buttons;

    [SerializeField] int[] lightOrder;
    int colorOrderRunCount;
    int buttonsClicked;

    //twee list 
    //een List die random wordt gevuld
    List<int> simonSays = new List<int>() ;
    //andere list die de speler vult.
    List<int> playerInput = new List<int>();

    // bools
    bool gameOver = false;
    public bool won;
    public bool passed;

    private Color highLightColor = Color.blue;
    private Color defaultColor = Color.white;

  

    private float lightspeed = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        //lees de buttons uit
        buttons[0].onClick.AddListener(() => ButtonClickOrder(0));
        buttons[1].onClick.AddListener(() => ButtonClickOrder(1));
        buttons[2].onClick.AddListener(() => ButtonClickOrder(2));
        buttons[3].onClick.AddListener(() => ButtonClickOrder(3));
        buttons[4].onClick.AddListener(() => ButtonClickOrder(4));
        buttons[5].onClick.AddListener(() => ButtonClickOrder(5));
        buttons[6].onClick.AddListener(() => ButtonClickOrder(6));
        buttons[7].onClick.AddListener(() => ButtonClickOrder(7));
        buttons[8].onClick.AddListener(() => ButtonClickOrder(8));
        

        
        //buttons[0].GetComponent<Image>().color = highLightColor;
    }

     public void OnEnable()
    {
        level = 0;
        buttonsClicked = 0;
        colorOrderRunCount = -1;
        won = false;
        for(int i = 0; i < lightOrder.Length; i++)
        {
            //lightOrder[i] = Random.Range(0, 8);
            simonSays.Add(lightOrder[i] = Random.Range(0, 8));
        }
        level = 1;
        StartCoroutine(ColorOrder());
    }

   public void ButtonClickOrder(int button)
    {
        playerInput.Add(button);
        print(button);
        print(buttonsClicked+ "  suck my dick bitch");
        buttonsClicked++;
        if(button == lightOrder[buttonsClicked - 1])
        {
            Debug.Log("pass");
            passed = true;
        }
        else
        {
            won = false;
            passed = false;
        }

        if (buttonsClicked == level && passed == true && buttonsClicked !=5)
        {
            print("level up");
            level++;
            passed = false;
            StartCoroutine(ColorOrder());
        }
        if (buttonsClicked == level && passed == true && buttonsClicked == 5)
        {
            won = true;
            //do something 
        }
    }

    IEnumerator ColorOrder()
    {
        buttonsClicked = 0;
        colorOrderRunCount++;
        DisableButtons();
        //buttons[lightOrder[0]].GetComponent<Image>().color = highLightColor;
        for (int i = 0; i < level; i++)
        {
            if(level >= colorOrderRunCount)
            {
                buttons[lightOrder[i]].GetComponent<Image>().color = defaultColor;
                yield return new WaitForSeconds(lightspeed);
                buttons[lightOrder[i]].GetComponent<Image>().color = highLightColor;
                yield return new WaitForSeconds(lightspeed);
                buttons[lightOrder[i]].GetComponent<Image>().color = defaultColor;
                yield return new WaitForSeconds(lightspeed);


            }

        }
        EnableButtons();


    }


    // fuction als de button word geklikt


    // check om te kijken of de twee lists gelijk zijn aan elkaar.
    //Method met statement en een return value
    public bool IslistsGelijk(List<int> simonSays, List<int> playerInput)
    {
        //vergelijk de list player input met de Simonsays.

        if (simonSays.Count() != playerInput.Count())
            return false; // als de list1 niet gelijk is aan de list2 dan is de bool false
        for (int i = 0; i < simonSays.Count(); i++)// telt het aantal 'int' in route
        {
            // als het aantal in list1 niet overeen komt met het aantal in list2 dan is die ook false.
            if (simonSays[i] != playerInput[i])
                return false;
        }
        // na alle checks als de twee list wel het zelfde zijn dan is de bool true.
        print("Done");
        return true;
    }
    
    //als de twee list != aan de elkaar dan gameover = true
    //else blijf door gaan.

    void DisableButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().interactable = false;
        }
    }
    void EnableButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().interactable = true;
        }
    }
}

     
     
