using UnityEngine;

[CreateAssetMenu(fileName = "StraightPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/StraightPlayed")]
public class StraightPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Straight")
        {
            return true;
        }
        return false;
    }
}