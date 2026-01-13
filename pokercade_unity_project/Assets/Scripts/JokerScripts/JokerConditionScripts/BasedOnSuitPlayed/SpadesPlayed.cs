using UnityEngine;

[CreateAssetMenu(fileName = "SpadesPlayed", menuName = "Scriptable Objects/Conditions/BasedOnSuit/SpadesPlayed")]
public class SpadesPlayed : JokerCondition
{
    public override bool IsTriggered(JokerContext context)
    {
        //Debug.Log("Checked IsTriggered");
        //Debug.Log(context.GetSuitPlayed());
        if (context.GetSuitPlayed() == Suit.Spades)
        {
            //Debug.Log("Returned True in IsTriggered Check");
            return true;
        }
        //Debug.Log("Returned False in IsTriggered Check");
        return false;
    }
}