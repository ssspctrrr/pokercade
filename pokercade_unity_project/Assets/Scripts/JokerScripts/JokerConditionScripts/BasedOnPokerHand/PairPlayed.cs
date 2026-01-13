using UnityEngine;

[CreateAssetMenu(fileName = "PairPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/PairPlayed")]
public class PairPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "Pair")
        {
            return true;
        }
        return false;
    }
}