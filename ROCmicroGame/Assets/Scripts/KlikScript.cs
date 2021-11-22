using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KlikScript : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        this.GetComponent<Image>().color = Color.red;
        
    }
    private void OnMouseOver()
    {
        print("Yes");
    }
}
