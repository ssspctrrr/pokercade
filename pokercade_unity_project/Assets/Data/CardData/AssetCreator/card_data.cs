using UnityEngine;

[CreateAssetMenu(fileName = "card_data", menuName = "Scriptable Objects/card_data")]
public class card_data : ScriptableObject
{
    public string id;
    public Suit suit;
    public Rank rank;
    public int value;
    public Sprite sprite;
}
