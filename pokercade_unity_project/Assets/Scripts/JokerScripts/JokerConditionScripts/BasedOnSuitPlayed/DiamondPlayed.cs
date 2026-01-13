using UnityEngine;

[CreateAssetMenu(fileName = "DiamondPlayed", menuName = "Scriptable Objects/Conditions/BasedOnSuit/DiamondPlayed")]
public class DiamondPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.GetSuitPlayed() == Suit.Diamond)
        {
            return true;
        }
        return false;
    }
}