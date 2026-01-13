using UnityEngine;

[CreateAssetMenu(fileName = "TwoPairPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/TwoPairPlayed")]
public class TwoPairPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Two Pair")
        {
            return true;
        }
        return false;
    }
}