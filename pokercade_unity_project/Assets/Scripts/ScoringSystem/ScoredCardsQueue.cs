using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoredCardsQueue
{
    public List<GameObject> queue_scored_cards = new List<GameObject>();

    public void initialize(List<GameObject> list_scored_cards)
    {
        queue_scored_cards = list_scored_cards;
    }

    public void enqueue(GameObject card)
    {
        Debug.Log("ScoredCardsQueue. Line 18");
        queue_scored_cards.Add(card);
    }

    public GameObject dequeue()
    {
        GameObject card_dequeued = queue_scored_cards[0];
        queue_scored_cards.RemoveAt(0);

        return card_dequeued;
    }

    public GameObject peek()
    {
        return queue_scored_cards[0];
    }

    public int size()
    {
        return queue_scored_cards.Count;
    }

    public bool is_empty()
    {
        if (queue_scored_cards.Count == 0)
        {
            return true;
        }
        return false;
    }
}
