using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BaseScoreData
{
    public string poker_hand;
    public int score_value;
    public int score_mult;
    public ScoredCardsQueue scored_cards = new ScoredCardsQueue();

    public void enqueue(GameObject card)
    {
        scored_cards.enqueue(card);
    }

    public GameObject dequeue() 
    {
        return scored_cards.dequeue();
    }

    public int size()
    {
        return scored_cards.size();
    }

    public bool is_empty()
    {
        return scored_cards.is_empty();
    }

    public void initialize_scored_cards(List<GameObject> list_scored_cards)
    {
        scored_cards.initialize(list_scored_cards);
    }
}
