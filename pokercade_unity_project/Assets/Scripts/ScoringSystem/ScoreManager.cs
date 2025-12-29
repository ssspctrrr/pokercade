using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;

public class ScoreManager : MonoBehaviour
{
    public GameObject played_cards_slot;
    public Text poker_hand_text;
    string poker_hand;
    public Text score_value_text;
    int score_value;
    public Text score_mult_text;
    int score_mult;
    public Text score_text;
    int score;

    public void calculate_score()
    {
        List<GameObject> played_cards = new List<GameObject>();
        foreach (Transform card_transform in played_cards_slot.transform)
        {
            played_cards.Add(card_transform.gameObject);
        }

        List<GameObject> scored_cards = new List<GameObject>();
        scored_cards = check_two_pair(played_cards);
        if (scored_cards != null && scored_cards.Count == 4)
        {
            poker_hand = "Two Pair";
            score_value = 20;
            score_mult = 2;
        }
        else if (scored_cards != null && scored_cards.Count == 2)
        {
            poker_hand = "Pair";
            score_value = 10;
            score_mult = 2;
        }
        else
        {
            scored_cards = check_high_card(played_cards);
            poker_hand = "High Card";
            score_value = 5;
            score_mult = 1;
        }

        foreach (GameObject scored_card in scored_cards)
        {
            score_value = score_value + scored_card.GetComponent<CardData>().Card.value;
        }
        score = score_value * score_mult;

        poker_hand_text.text = poker_hand;
        score_value_text.text = score_value.ToString();
        score_mult_text.text = score_mult.ToString();
        score_text.text = score.ToString();
    }

    public static List<GameObject> check_high_card(List<GameObject> played_cards)
    {
        List<GameObject> sorted_played_cards = played_cards.OrderByDescending(card => (int)card.GetComponent<CardData>().Card.rank).ToList();
        List<GameObject> scored_cards = new List<GameObject>();
        if (sorted_played_cards[^1].GetComponent<CardData>().Card.rank == Rank.Ace)
        {
            scored_cards.Add(sorted_played_cards[^1]);
        }
        else
        {
            scored_cards.Add(sorted_played_cards[0]);
        }
        return scored_cards;
    }

    public static List<GameObject> check_pair(List<GameObject> played_cards)
    {
        List<Rank> rank_list = new List<Rank>();
        List<GameObject> scored_cards = new List<GameObject>();
        Rank rank_of_pair = new Rank();

        foreach (GameObject card in played_cards)
        {
            CardData card_data = card.GetComponent<CardData>();
            rank_list.Add(card_data.Card.rank);
        }

        foreach (Rank current_rank in rank_list) 
        {
            if (rank_list.Count(each_card_rank => each_card_rank == current_rank) == 2 )
            {
                rank_of_pair = current_rank;
                break;
            }
            else
            {
                return null; 
            }
        }

        foreach (GameObject card in played_cards)
        {
            if (card.GetComponent<CardData>().Card.rank == rank_of_pair)
            {
                scored_cards.Add(card);
            }
        }
        return scored_cards;
    }

    public static List<GameObject> check_two_pair(List<GameObject> played_cards) 
    { 
        List<GameObject> first_pair_check = check_pair(played_cards);
        if (first_pair_check != null && first_pair_check.Count > 0)
        {
            int removed_cards = 0;
            Rank first_pair_rank = first_pair_check[0].GetComponent<CardData>().Card.rank;
            for (int i = 0; i < played_cards.Count; i++)
            {
                if (played_cards[i - removed_cards].GetComponent<CardData>().Card.rank == first_pair_rank)
                {
                    played_cards.Remove(played_cards[i - removed_cards]);
                    removed_cards++;

                    if (removed_cards == 2)
                    {
                        break;
                    }
                }
            }
            List<GameObject> second_pair_check = check_pair(played_cards);
            {
                if (second_pair_check != null && second_pair_check.Count > 0)
                {
                    return first_pair_check.Concat(second_pair_check).ToList();
                }
            }
        }
        return first_pair_check;
    }
}
