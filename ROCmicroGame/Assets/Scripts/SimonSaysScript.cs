using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SimonSaysScript : MonoBehaviour
{
    //int level
    public int level = 0;
    //array voor de buttons
    public Button[] buttons;

    //twee list 
    //een List die random wordt gevuld
    List<int> simonSays = new List<int>() {1} ;
    //andere list die de speler vult.
    List<int> playerInput = new List<int>();

    //Gameover bool
    bool gameOver = false;

    public Color highLightColor;
    public Color errorColor;
    

    // Start is called before the first frame update
    void Start()
    {
        //lees de buttons uit
        buttons[0].onClick.AddListener(() => ButtonClicked(1));
        buttons[1].onClick.AddListener(() => ButtonClicked(2));
        buttons[2].onClick.AddListener(() => ButtonClicked(3));
        buttons[3].onClick.AddListener(() => ButtonClicked(4));
        buttons[4].onClick.AddListener(() => ButtonClicked(5));
        buttons[5].onClick.AddListener(() => ButtonClicked(6));
        buttons[6].onClick.AddListener(() => ButtonClicked(7));
        buttons[7].onClick.AddListener(() => ButtonClicked(8));
        buttons[8].onClick.AddListener(() => ButtonClicked(9));
        level++;
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < level; i++)
        {
            simonSays.Add(Random.Range(1, 9));
        }
    }
    // fuction als de button word geklikt
    private void ButtonClicked(int id)
    {
        //voeg de Element ID toe aan the list
        playerInput.Add(id);
        

        print(id);
        IslistsGelijk(simonSays,playerInput);

    }
    

    public async void SimonSays()
    {
        //for(int i = 0; i <= level)
       // await buttons[]
    }

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
}

     
     
