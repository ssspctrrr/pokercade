using UnityEngine;

[CreateAssetMenu(fileName = "StraightFlushPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/StraightFlushPlayed")]
public class StraightFlushPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Straight Flush")
        {
            return true;
        }
        return false;
    }
}