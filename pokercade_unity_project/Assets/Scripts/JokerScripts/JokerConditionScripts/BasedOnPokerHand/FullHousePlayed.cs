using UnityEngine;

[CreateAssetMenu(fileName = "FullHousePlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/FullHousePlayed")]
public class FullHousePlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Full House")
        {
            return true;
        }
        return false;
    }
}