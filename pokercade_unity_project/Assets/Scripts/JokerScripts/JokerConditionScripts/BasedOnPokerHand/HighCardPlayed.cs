using UnityEngine;

[CreateAssetMenu(fileName = "HighCardPlayed", menuName = "Scriptable Objects/Conditions/BasedOnPokerHand/HighCardPlayed")]
public class HighCardPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.PokerHandPlayed() == "High Card")
        {
            return true;
        }
        return false;
    }
}