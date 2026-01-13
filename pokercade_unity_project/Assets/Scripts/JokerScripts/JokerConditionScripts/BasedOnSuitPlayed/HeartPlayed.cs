using UnityEngine;

[CreateAssetMenu(fileName = "HeartPlayed", menuName = "Scriptable Objects/Conditions/BasedOnSuit/HeartPlayed")]
public class HeartPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.GetSuitPlayed() == Suit.Heart)
        {
            return true;
        }
        return false;
    }
}