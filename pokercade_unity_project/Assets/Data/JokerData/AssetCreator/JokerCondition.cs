using UnityEngine;

//[CreateAssetMenu(menuName = "Scriptable Objects/JokerCondition")]
public abstract class JokerCondition : ScriptableObject
{
    public abstract bool IsTriggered(JokerContext context);
}