using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// scriptable object voor de trainingen.
/// </summary>
[CreateAssetMenu(fileName = "New Training", menuName ="Training")]// voegt nog een tabje toe aan de assets menu en als je een nieuwe training aan maakt is de default filename newTraining.
public class Training : ScriptableObject
{
    public new string name;
    public float tijd;
    public VideoClip trainingsVideo;
    

}
