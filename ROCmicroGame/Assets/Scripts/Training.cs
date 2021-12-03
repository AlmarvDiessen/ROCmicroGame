using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Training", menuName ="Training")]
public class Training : ScriptableObject
{
    public new string name;
    public float tijd;
    public VideoClip trainingsVideo;
    

}
