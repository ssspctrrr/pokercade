using System.Collections;
using UnityEngine;

//[CreateAssetMenu(fileName = "JokerEffect", menuName = "Scriptable Objects/JokerEffect")]
public abstract class JokerEffect : ScriptableObject
{
    public TextAnimations textAnimations;

    public abstract void Effect(JokerContext context, BaseScoreData baseScore);

    //private void OnEnable()
    //{
    //    textAnimations = GameObject.Find("ButtonManager").GetComponent<TextAnimations>();
    //}
}
