using UnityEngine;

[CreateAssetMenu(fileName = "FlushPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/FlushPlayed")]
public class FlushPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Flush")
        {
            return true;
        }
        return false;
    }
}