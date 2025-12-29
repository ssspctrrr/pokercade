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
}
