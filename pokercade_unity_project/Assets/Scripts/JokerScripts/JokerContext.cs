using UnityEngine;
using TMPro;

public class JokerContext
{
    public BaseScoreData baseScore;
    public int hand;
    public GameObject card;
    public TextMeshProUGUI scoreMultText;
    public TextAnimations textAnimations;

    public string PokerHandPlayed()
    {
        return baseScore.poker_hand;
    }

    public Suit GetSuitPlayed()
    {
        return card.GetComponent<CardInstance>().GetSuit();
    }

    //public int ScoreMultData()
    //{
    //    return baseScore.score_mult;
    //}

    public TextMeshProUGUI GetScoreMultText()
    {
        return scoreMultText;
    }
}
