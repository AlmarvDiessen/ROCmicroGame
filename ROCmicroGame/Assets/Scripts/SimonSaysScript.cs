using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public Text levelText;
   

    //twee list 
    //een List die random wordt gevuld
    List<int> simonSays = new List<int>() ;
    //andere list die de speler vult.
    List<int> playerInput = new List<int>();

    // bools
    bool gameOver = false;
    public bool won;
    public bool passed;
    public bool endeless;

    private Color highLightColor = Color.blue;
    private Color defaultColor = Color.white;

    public GameObject game;
    public GameObject pauseMenu;

  

    private float lightspeed = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        //lees de buttons uit en geeft een waarda aan int button in de ButtonClickOrder()
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

    private void Update()
    {
        //zet wat text met de level variable naar string
        levelText.text = "Level: " + level.ToString();
    }

    public void OnEnable()
    {
        //begin waardes voor alles
        level = 0;
        endeless = true;
        buttonsClicked = 0;
        colorOrderRunCount = -1;
        won = false;
        for(int i = 0; i < lightOrder.Length; i++)
        {
            //lightOrder[i] = Random.Range(0, 8);
            //voegt een aantal random nummer toe aande hand van hoe groot de index is van de array
            simonSays.Add(lightOrder[i] = Random.Range(0, 8));
        }
        level = 1;
        StartCoroutine(ColorOrder()); //start de Coroutine voor de color order
    }

   public void ButtonClickOrder(int button)
    {
        print(button);
        print(buttonsClicked);
        buttonsClicked++;
        if(button == lightOrder[buttonsClicked - 1])// buttonClicked is 1 - 1 = 0 en de buttons waarde is van 0 tot 8 om het zo gelijk te maken
        {
            Debug.Log("pass");
            passed = true;
        }
        else
        {
            won = false;
            passed = false;
            print("F");
        }

        if (buttonsClicked == level && passed == true && endeless == true)
        {
            print("level up");
            level++;//level gaat up als de buttons clicked gelijk is aan elkaar
            passed = false;
            StartCoroutine(ColorOrder());
        }
        if (buttonsClicked == level && passed == true && endeless == false)
        {
            won = true;
            //do something 
        }
    }

    IEnumerator ColorOrder()
    {
        buttonsClicked = 0;
        colorOrderRunCount++;
        DisableButtons(); // zet de knoppen uit
        //buttons[lightOrder[0]].GetComponent<Image>().color = highLightColor;
        for (int i = 0; i < level; i++)
        {
            if(level >= colorOrderRunCount) // 
            {
                buttons[lightOrder[i]].GetComponent<Image>().color = defaultColor; // begint met de default color
                yield return new WaitForSeconds(lightspeed); //wacht voor de lightspeed(halve seconde)
                buttons[lightOrder[i]].GetComponent<Image>().color = highLightColor;
                yield return new WaitForSeconds(lightspeed);
                buttons[lightOrder[i]].GetComponent<Image>().color = defaultColor;
                yield return new WaitForSeconds(lightspeed);

            }
        }
        EnableButtons();//zet de knoppen weer aan


    }



    void DisableButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            //telt het aantal buttons en zet ze uit
            buttons[i].GetComponent<Button>().interactable = false;
        }
    }
    void EnableButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            //andersom maar dan aan
            buttons[i].GetComponent<Button>().interactable = true;
        }
    }
    // settings voor de button
    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        game.SetActive(false);
        
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        game.SetActive(true);
        
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

/*
 *     // check om te kijken of de twee lists gelijk zijn aan elkaar.
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
 */


