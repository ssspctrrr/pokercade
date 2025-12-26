using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public card_data data; 
    public GameObject cardBackObj; // 
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(card_data assignedData)
    {
        data = assignedData;
        if (spriteRenderer != null && data.sprite != null)
        {
            spriteRenderer.sprite = data.sprite;
        }
        
        if (data != null) gameObject.name = data.name; 
    }

    public void SetFaceUp(bool isFaceUp)
    {
        if (cardBackObj != null)
        {
            cardBackObj.SetActive(!isFaceUp); 
        }
    }
}