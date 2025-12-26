using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject cardPrefab;   
    public Transform handSlot;      
    
    [Header("Data Source")]
    public List<card_data> allCardData = new List<card_data>(); 

    private List<card_data> currentDeck = new List<card_data>();

    void Start()
    {
        currentDeck.AddRange(allCardData);

        // Spawn 8 random cards on start
        DealHand(8);
    }

    public void DealHand(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentDeck.Count == 0) return; 

            int randomIndex = Random.Range(0, currentDeck.Count);
            card_data selectedData = currentDeck[randomIndex];

            GameObject newCardObj = Instantiate(cardPrefab, handSlot);
            
            // Simple positioning so they don't stack perfectly on top of each other
            newCardObj.transform.localPosition = new Vector3(i * 1.5f, 0, 0); 

            CardInstance cardLogic = newCardObj.GetComponent<CardInstance>();
            if (cardLogic != null)
            {
                cardLogic.Initialize(selectedData);
            }
        }
    }
}