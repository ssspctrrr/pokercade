using System.Collections.Generic;
using UnityEngine;

public class JokerManager : MonoBehaviour
{
    public Transform jokerManagerTransform;
    
    [Header("Layout Settings")]
    public float cardSpacing = 2.0f;
    public float animationSpeed = 10f; // How fast cards slide into place

    // The card currently being held by the player
    [HideInInspector] public GameObject currentlyDraggingCard;

    private void Awake()
    {
        if (jokerManagerTransform == null) jokerManagerTransform = transform;
    }

    private void Update()
    {
        // Continuously animate cards to their correct positions
        AnimateJokerPositions();
    }

    private void AnimateJokerPositions()
    {
        int childCount = jokerManagerTransform.childCount;
        if (childCount == 0) return;

        // Calculate the starting X position so the whole hand is centered
        float startX = -((childCount - 1) * cardSpacing) / 2.0f;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = jokerManagerTransform.GetChild(i);
            
            // If this is the card we are dragging, SKIP IT. 
            // The DraggableJoker script controls its position.
            if (child.gameObject == currentlyDraggingCard) continue;

            // Calculate target position based on Hierarchy Index
            Vector3 targetPos = new Vector3(startX + (i * cardSpacing), 0, 0);

            // Smoothly slide the card to that position
            child.localPosition = Vector3.Lerp(child.localPosition, targetPos, Time.deltaTime * animationSpeed);
        }
    }

    // Call this to update the linked list for game logic (scoring)
    public LinkedList<GameObject> GetRefreshedJokerList()
    {
        LinkedList<GameObject> list = new LinkedList<GameObject>();
        foreach(Transform child in jokerManagerTransform)
        {
            list.AddLast(child.gameObject);
        }
        return list;
    }

    // ... (Keep your TriggerJokers logic here)
    public void TriggerJokers(JokerTrigger jokerTrigger, JokerContext context, BaseScoreData baseScore)
    {
        // NOTE: Make sure to get the FRESH list, not a cached one, 
        // because the order changes while dragging.
        LinkedList<GameObject> liveList = GetRefreshedJokerList(); 
        
        LinkedListNode<GameObject> currentJokerNode = liveList.First;
        while (currentJokerNode != null)
        {
            GameObject joker = currentJokerNode.Value;
            JokerInstance jokerInstance = joker.GetComponent<JokerInstance>();
            
            if (jokerInstance != null && jokerInstance.jokerData.eventTrigger == jokerTrigger) 
            {
                if (jokerInstance.jokerData.jokerCondition.IsTriggered(context))
                {
                    jokerInstance.jokerData.jokerEffect.Effect(context, baseScore);
                }
            }
            currentJokerNode = currentJokerNode.Next;
        }
    }
}