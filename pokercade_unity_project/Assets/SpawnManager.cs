using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject cardPrefab;   
    public Transform handSlot;      
    [Header("Visual Layout")]
    public float cardSpacing = 1.5f;   
    public float arcIntensity = 5.0f;  
    [Header("Data Source")]
    public List<card_data> allCardData = new List<card_data>(); 

    private List<card_data> currentDeck = new List<card_data>();

    void Start()
    {
        currentDeck.AddRange(allCardData);

        
        DealHand(8);
    }

    public void DealHand(int amount)
{
    
    float startX = -((amount - 1) * cardSpacing) / 2.0f;

    for (int i = 0; i < amount; i++)
    {
        if (currentDeck.Count == 0) return;

        int randomIndex = Random.Range(0, currentDeck.Count);
        card_data selectedData = currentDeck[randomIndex];

        GameObject newCardObj = Instantiate(cardPrefab, handSlot);
        CardInstance cardLogic = newCardObj.GetComponent<CardInstance>();
        if (cardLogic != null) cardLogic.Initialize(selectedData);

        float xPos = startX + (i * cardSpacing);
        
        float distFromCenter = i - ((amount - 1) / 2.0f);
        float yPos = -Mathf.Abs(distFromCenter) * 0.2f; 

        newCardObj.transform.localPosition = new Vector3(xPos, yPos, 0);

        float zRot = -distFromCenter * arcIntensity;
        newCardObj.transform.localRotation = Quaternion.Euler(0, 0, zRot);
        
        SpriteRenderer sr = newCardObj.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = i + 1;
    }
}
}