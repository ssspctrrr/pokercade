using Unity.VisualScripting;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public card_data Card;
    public SpriteRenderer CardImage;

    void Awake()
    {
        CardImage.sprite = Card.sprite;
    }
    public Suit get_suit()
    {
        return Card.suit;
    }

    public Rank get_rank()
    {
        return Card.rank;
    }

    public int get_value()
    {
        return Card.value;
    }
}
