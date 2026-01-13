using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/JokerData")]
public class JokerData : ScriptableObject
{
    public string name;
    [TextArea] public string description;
    public Sprite sprite;
    public JokerRarity rarity;
    public JokerTrigger eventTrigger;
    public JokerCondition jokerCondition;
    public JokerEffect jokerEffect;
}
