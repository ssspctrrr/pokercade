using UnityEngine;

[CreateAssetMenu(fileName = "ThreeOfAKindPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/ThreeOfAKindPlayed")]
public class ThreeOfAKindPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Three of a Kind")
        {
            return true;
        }
        return false;
    }
}