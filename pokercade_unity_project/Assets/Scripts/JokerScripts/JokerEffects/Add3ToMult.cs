using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Add3ToMult", menuName = "Scriptable Objects/JokerEffects/Add3ToMult")]
public class Add3ToMult : JokerEffect
{
    public override void Effect(JokerContext context, BaseScoreData baseScore)
    {
        //Debug.Log("Tried to Add 3 to baseScore");
        baseScore.score_mult += 3;
        //StartCoroutine(textAnimations.TextShaker());
        context.textAnimations.TransitionTextViaShake(context.scoreMultText, baseScore.score_mult.ToString());
    }
}
