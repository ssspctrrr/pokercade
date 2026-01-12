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
    public SelectionManager selectionManager; 
    public ScoreManager scoreManager;
    
    [Header("Visual Layout")]
    public float cardSpacing = 1.0f;   
    public float arcIntensity = 5.0f;
    public float moveSpeed = 2.0f; 
    public int maxHandSize = 8; 
    
    [Header("Data Source")]
    public List<card_data> allCardData = new List<card_data>(); 
    private List<card_data> currentDeck = new List<card_data>();



public void OnDiscardButtonPress()
{
    // Check GameManager for discards remaining
    if (GameManager.instance.TryUseDiscard())
    {
        StartCoroutine(DiscardAndRefillRoutine());
    }
    else
    {
        Debug.Log("No discards remaining!");
        // Optional: Play a "buzzer" sound or shake effect here
    }
}

    IEnumerator DiscardAndRefillRoutine()
{
    List<GameObject> cardsToDiscard = new List<GameObject>();
    foreach (Transform child in handSlot)
    {
        CardInstance card = child.GetComponent<CardInstance>();
        if (card != null && card.selected)
        {
            cardsToDiscard.Add(child.gameObject);
            

            Collider2D col = child.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
    }

    if (cardsToDiscard.Count > 0)
    {
        yield return StartCoroutine(AnimateDiscard(cardsToDiscard));
    }
    yield return null; 

    int cardsInHand = handSlot.childCount;
    int cardsNeeded = maxHandSize - cardsInHand;

    if (cardsNeeded > 0)
    {
        yield return StartCoroutine(DealHandRoutine(cardsNeeded));
    }
    else
    {
    
        ArrangeHand();
    }
}
    IEnumerator DealHandRoutine(int amount)
    {
        
        for (int i = 0; i < amount; i++)
        {
            if (currentDeck.Count == 0) break;

            int randomIndex = Random.Range(0, currentDeck.Count);
            card_data selectedData = currentDeck[randomIndex];
            currentDeck.RemoveAt(randomIndex); 

            GameObject newCardObj = Instantiate(cardPrefab, handSlot);
            newCardObj.transform.position = deckTransform.position; 
            newCardObj.transform.rotation = Quaternion.identity;

            CardInstance cardLogic = newCardObj.GetComponent<CardInstance>();
            if (cardLogic != null)
            {
                cardLogic.Initialize(selectedData);
                cardLogic.SetFaceUp(false);
            }
            
            
            yield return new WaitForSeconds(0.1f);
        }

        
        ArrangeHand();
    }

    IEnumerator AnimateDiscard(List<GameObject> cardsToDiscard)
{
    float duration = 0.4f; 
    float elapsed = 0f;


    Vector3[] startPositions = new Vector3[cardsToDiscard.Count];
    Quaternion[] startRotations = new Quaternion[cardsToDiscard.Count];
    

    float[] randomRotations = new float[cardsToDiscard.Count];

    for (int i = 0; i < cardsToDiscard.Count; i++)
    {
        startPositions[i] = cardsToDiscard[i].transform.localPosition;
        startRotations[i] = cardsToDiscard[i].transform.localRotation;
        randomRotations[i] = Random.Range(-45f, 45f); 
    }

    // 2. Animate loop
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;
               float tSquared = t * t; 

        for (int i = 0; i < cardsToDiscard.Count; i++)
        {
            if (cardsToDiscard[i] != null)
            {
                Vector3 targetPos = startPositions[i] + (Vector3.down * 8.0f);
                
                cardsToDiscard[i].transform.localPosition = Vector3.Lerp(startPositions[i], targetPos, tSquared);
                
                Quaternion targetRot = startRotations[i] * Quaternion.Euler(0, 0, randomRotations[i]);
                cardsToDiscard[i].transform.localRotation = Quaternion.Lerp(startRotations[i], targetRot, tSquared);
            }
        }
        yield return null;
    }

 
    foreach (GameObject card in cardsToDiscard)
    {
        Destroy(card);
    }
}
    
    void ArrangeHand()
    {
        SortHandByRank();
        int totalCards = handSlot.childCount;
        float startX = -((totalCards - 1) * cardSpacing) / 2.0f;

        for (int i = 0; i < totalCards; i++)
        {
            Transform cardTransform = handSlot.GetChild(i);
            CardInstance cardLogic = cardTransform.GetComponent<CardInstance>();

            
            float xPos = startX + (i * cardSpacing);
            float distFromCenter = i - ((totalCards - 1) / 2.0f);
            float yPos = -Mathf.Abs(distFromCenter) * 0.2f;
            float zRot = -distFromCenter * arcIntensity;

            Vector3 targetPos = new Vector3(xPos, yPos, 0); 
            Quaternion targetRot = Quaternion.Euler(0, 0, zRot);

            
            SortingGroup sg = cardTransform.GetComponent<SortingGroup>();
            if (sg != null) sg.sortingOrder = i;

            
            StartCoroutine(AnimateCardToHand(cardTransform.gameObject, targetPos, targetRot, cardLogic));
        }
    }

    IEnumerator AnimateCardToHand(GameObject card, Vector3 localTargetPos, Quaternion targetRot, CardInstance logic)
    {
        float t = 0;
        Vector3 startPos = card.transform.localPosition;
        Quaternion startRot = card.transform.localRotation;

        while (t < 1)
        {
            if (card == null) yield break; 

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

    
        if (logic != null && card != null)
        {
            card.transform.localPosition = localTargetPos; 
            logic.UpdateBasePosition(localTargetPos);
        }
    }
    public void RefillHand()
{
    int cardsInHand = handSlot.childCount;
    int cardsNeeded = maxHandSize - cardsInHand;

    if (cardsNeeded > 0)
    {
        StartCoroutine(DealHandRoutine(cardsNeeded));
    }
    else
    {
        ArrangeHand(); 
        }
    }
    void SortHandByRank()
{
    List<CardInstance> cardsInHand = new List<CardInstance>();
    foreach (Transform child in handSlot)
    {
        CardInstance card = child.GetComponent<CardInstance>();
        if (card != null)
        {
            cardsInHand.Add(card);
        }
    }

    cardsInHand.Sort((a, b) => 
    {
        int rankA = (int)a.GetRank();
        int rankB = (int)b.GetRank();
        
        if (rankA != rankB)
        {
            return -rankA.CompareTo(rankB); 
        }
        
        int suitA = (int)a.GetSuit();
        int suitB = (int)b.GetSuit();
        return suitA.CompareTo(suitB);
    });

    for (int i = 0; i < cardsInHand.Count; i++)
    {
        cardsInHand[i].transform.SetSiblingIndex(i);
    }
    }
    public void OnPlayHandButtonPress()
{
    // Don't play if animations are running or no hands left
    if (GameManager.instance.handsRemaining <= 0) return;

    // Get selected cards
    List<GameObject> cardsToPlay = selectionManager.selectedCards;
    
    // Basic validation (must play at least 1 card)
    if (cardsToPlay.Count == 0) return;

    if (GameManager.instance.TryUseHand())
    {
        StartCoroutine(PlayHandRoutine(cardsToPlay));
    }
}
IEnumerator PlayHandRoutine(List<GameObject> cardsToPlay)
{
    // 1. Disable interactions
    foreach (var card in cardsToPlay)
    {
        card.GetComponent<Collider2D>().enabled = false;
        // Optional: Move them slightly up to indicate they are "Played"
    }

    // 2. Calculate Score (Wait for the ScoreManager routine to finish)
    bool scoringFinished = false;
    
    // We pass a lambda function (Action) to know when scoring is done
    scoreManager.calculate_score(cardsToPlay, () => { scoringFinished = true; });

    // Wait until callback sets scoringFinished to true
    while (!scoringFinished) yield return null;

    // 3. Add the calculated score to the GameManager
    // (Note: You need to modify ScoreManager to return the value or grab it here)
    // For now, let's assume ScoreManager updated its internal score, 
    // we need to pass that to GameManager. See Step 3 below.

    // 4. Discard the played cards (Visual animation)
    yield return StartCoroutine(AnimateDiscard(cardsToPlay));

    // 5. Refill Hand
    RefillHand();
}
public void ResetDeck()
{
    currentDeck.Clear(); // Remove any leftover cards
    currentDeck.AddRange(allCardData); // Put all 52 cards back in
}
// Inside SpawnManager.cs

public void TriggerNextLevel()
{
    StartCoroutine(NextLevelSequence());
}

IEnumerator NextLevelSequence()
{
    // 1. Destroy ALL existing cards in the hand
    // We iterate backwards to safely remove them
    for (int i = handSlot.childCount - 1; i >= 0; i--)
    {
        Destroy(handSlot.GetChild(i).gameObject);
    }

    // 2. WAIT A FRAME (Crucial!)
    // This gives Unity time to actually remove the objects from memory
    yield return null;

    // 3. Reset the internal deck data
    currentDeck.Clear();
    currentDeck.AddRange(allCardData);

    // 4. Deal a fresh hand (This triggers the animation)
    yield return StartCoroutine(DealHandRoutine(maxHandSize));
}
}