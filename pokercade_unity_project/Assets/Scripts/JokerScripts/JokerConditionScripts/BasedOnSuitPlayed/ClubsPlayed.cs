using UnityEngine;

[CreateAssetMenu(fileName = "ClubsPlayed", menuName = "Scriptable Objects/Conditions/BasedOnSuit/ClubsPlayed")]
public class ClubsPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        if (context.GetSuitPlayed() == Suit.Clubs)
        {
            return true;
        }
        return false;
    }
}