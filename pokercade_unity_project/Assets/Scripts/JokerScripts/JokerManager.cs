using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerManager : MonoBehaviour
{
    public Transform jokerManagerTransform;
    LinkedList<GameObject> jokers = new LinkedList<GameObject>();

    // public Add jokers

    // public remove jokers

    private void Awake()
    {
        foreach(Transform jokerTransform in jokerManagerTransform)
        {
            GameObject joker = jokerTransform.gameObject;
            jokers.AddLast(joker);
        }

    }

    public void TriggerJokers(JokerTrigger jokerTrigger, JokerContext context, BaseScoreData baseScore)
    {
        LinkedListNode<GameObject> currentJokerNode = jokers.First;

        while (currentJokerNode != null)
        {
            GameObject joker = currentJokerNode.Value;
            JokerInstance jokerInstance = joker.GetComponent<JokerInstance>();

            if (jokerInstance.jokerData.eventTrigger != jokerTrigger) 
            {
                currentJokerNode = currentJokerNode.Next;
                continue;
            }

            if (jokerInstance.jokerData.jokerCondition.IsTriggered(context))
            {
                jokerInstance.jokerData.jokerEffect.Effect(context, baseScore);
            }

            currentJokerNode = currentJokerNode.Next;
        }
    }
}
