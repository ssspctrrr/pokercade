using UnityEngine;

[CreateAssetMenu(fileName = "FourOfAKindPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/FourOfAKindPlayed")]
public class FourOfAKindPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Four of a Kind")
        {
            return true;
        }
        return false;
    }
}