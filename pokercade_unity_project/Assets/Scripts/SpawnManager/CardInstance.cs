using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInstance : MonoBehaviour, IPointerUpHandler
{
    public card_data data; 
    public GameObject cardBackObj; // 
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D cardBoxCollider;
    public float pointerUpTime;
    public float pointerDownTime;
    public bool selected;
    public int selectionOffset = 50;
    public Vector3 originalCoords;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardBoxCollider = GetComponent<BoxCollider2D>();
    }

    public void Initialize(card_data assignedData)
    {
        data = assignedData;
        if (spriteRenderer != null && data.sprite != null)
        {
            spriteRenderer.sprite = data.sprite;
        }
        if (spriteRenderer != null && cardBoxCollider != null) 
        { 
            cardBoxCollider.size = spriteRenderer.size;
        }
        
        if (data != null) gameObject.name = data.name; 
    }

    public Rank GetRank()
    {
        return data.rank;
    }

    public Suit GetSuit()
    {
        return data.suit;
    }

    public int GetValue()
    {
        return data.value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        pointerUpTime = Time.time;
        selected = !selected;
        if (selected)
            transform.localPosition += (transform.up * selectionOffset);
        else 
            transform.localPosition = originalCoords;
    }

    public void UpdateBasePosition(Vector3 newLocalPos)
{
    originalCoords = newLocalPos;
    if (!selected)
    {
        transform.localPosition = originalCoords;
    }
}

    public void SetFaceUp(bool isFaceUp)
    {
        if (cardBackObj != null)
        {
            cardBackObj.SetActive(!isFaceUp); 
        }
    }
}