using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.Rendering;

public class SpawnManager : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject cardPrefab;   
    public Transform handSlot;
    public Transform deckTransform; 
    
    [Header("Visual Layout")]
    public float cardSpacing = 1.0f;   
    public float arcIntensity = 5.0f;
    public float moveSpeed = 2.0f; 
    
    [Header("Data Source")]
    public List<card_data> allCardData = new List<card_data>(); 
    private List<card_data> currentDeck = new List<card_data>();

    void Start()
    {
        currentDeck.AddRange(allCardData);
        StartCoroutine(DealHandRoutine(8));
    }

    IEnumerator DealHandRoutine(int amount)
    {
        float startX = -((amount - 1) * cardSpacing) / 2.0f;

        for (int i = 0; i < amount; i++)
        {
            if (currentDeck.Count == 0) yield break;

            int randomIndex = Random.Range(0, currentDeck.Count);
            card_data selectedData = currentDeck[randomIndex];

            GameObject newCardObj = Instantiate(cardPrefab, handSlot);
            newCardObj.transform.position = deckTransform.position;
            newCardObj.transform.rotation = Quaternion.identity;    

            CardInstance cardLogic = newCardObj.GetComponent<CardInstance>();
            if (cardLogic != null)
            {
                cardLogic.Initialize(selectedData);
                cardLogic.SetFaceUp(false); 
            }

            float xPos = startX + (i * cardSpacing);
            float distFromCenter = i - ((amount - 1) / 2.0f);
            float yPos = -Mathf.Abs(distFromCenter) * 0.2f;
            float zRot = -distFromCenter * arcIntensity;

            SortingGroup sg = newCardObj.GetComponent<SortingGroup>();
            if (sg != null) sg.sortingOrder = i;

            Vector3 targetPos = new Vector3(xPos, yPos, 0); 
            Quaternion targetRot = Quaternion.Euler(0, 0, zRot);
            
            StartCoroutine(AnimateCardToHand(newCardObj, targetPos, targetRot, cardLogic));

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator AnimateCardToHand(GameObject card, Vector3 localTargetPos, Quaternion targetRot, CardInstance logic)
    {
        float t = 0;
        Vector3 startPos = card.transform.localPosition;
        Quaternion startRot = card.transform.localRotation;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            
            float smoothT = 1f - Mathf.Pow(1f - t, 3); 

            card.transform.localPosition = Vector3.Lerp(startPos, localTargetPos, smoothT);
            card.transform.localRotation = Quaternion.Lerp(startRot, targetRot, smoothT);

            if (t >= 0.5f && logic != null)
            {
                logic.SetFaceUp(true);
            }

            yield return null; 
        }
    }
}