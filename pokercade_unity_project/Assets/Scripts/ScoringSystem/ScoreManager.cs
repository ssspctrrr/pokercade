using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;

public class ScoreManager : MonoBehaviour
{
    public GameObject played_cards_slot;
    public Text poker_hand_text;
    public Text score_value_text;
    public Text score_mult_text;
    public Text score_text;
    int score;

    public void calculate_score()
    {
        List<GameObject> played_cards = new List<GameObject>();
        foreach (Transform card_transform in played_cards_slot.transform)
        {
            played_cards.Add(card_transform.gameObject);
        }

        BaseScoreData base_score = null;
        base_score = check_straight_and_or_flush(played_cards);
        if (base_score == null) { base_score = check_four_of_a_kind(played_cards); }
        if (base_score == null) { base_score = check_full_house_3_of_a_kind_pairs(played_cards); }
        if (base_score == null) { base_score = get_high_card(played_cards); }

        foreach (GameObject scored_card in base_score.scored_cards)
        {
            base_score.score_value = base_score.score_value + scored_card.GetComponent<CardData>().Card.value;
        }
        score = base_score.score_value * base_score.score_mult;

        poker_hand_text.text = base_score.poker_hand;
        score_value_text.text = base_score.score_value.ToString();
        score_mult_text.text = base_score.score_mult.ToString();
        score_text.text = score.ToString();
    }

    public static BaseScoreData check_straight_and_or_flush(List<GameObject> played_cards)
    {
        BaseScoreData base_score = new BaseScoreData();
        List<GameObject> straight = check_straight(played_cards);
        List<GameObject> flush = check_flush(played_cards);

        if (straight != null && flush != null) 
        {
            // Straight flush is played
            // Adjust value of base_score for straight flush
            return base_score;
        }
        else if (straight != null)
        {
            // Straight is played
            // Adjust value of base_score for straight
            return base_score;
        }
        else if (flush != null)
        {
            // Flush is played
            // Adjust value of base_score for flush
            return base_score;
        }
        // else both straight and flush is null, return null
        return null;
    }

    public static List<GameObject> check_straight(List<GameObject> played_cards)
    {
        List<GameObject> scored_cards = new List<GameObject>();
        return scored_cards;
    }

    public static List<GameObject> check_flush(List<GameObject> played_cards)
    {
        List<GameObject> scored_cards = new List<GameObject>();
        return scored_cards;
    }

    public static BaseScoreData check_four_of_a_kind(List<GameObject> played_cards)
    {
        // check if four of a kind is played
        BaseScoreData base_score = new BaseScoreData();
        return base_score;
    }

    public static BaseScoreData check_full_house_3_of_a_kind_pairs(List<GameObject> played_cards)
    {
        BaseScoreData base_score = new BaseScoreData();
        List<GameObject> pairs = check_for_pairs(played_cards);
        if (pairs == null)
        {
            return null;
        }
        else if (pairs.Count == 4)
        {
            // adjust base_score for two pair
            return base_score;
        }
        else // if pairs.Count == 2
        {
            List<GameObject> three_of_a_kind = check_three_of_a_kind(played_cards);
            if (three_of_a_kind != null && pairs.Count == 2)
            {
                // adjust base_score for full house
                return base_score;
            }
            else if (three_of_a_kind != null)
            {
                // adjust base_score for three of a kind
                return base_score;
            }
            else
            {
                // adjust base_score for pair
                return base_score;
            }
        }
    }

    public static List<GameObject> check_for_pairs(List<GameObject> played_cards) 
    {
        List<GameObject> scored_cards = new List<GameObject>();
        List<GameObject> first_pair_check = check_pair(played_cards);
        if (first_pair_check != null && first_pair_check.Count > 0) // checks if there is a pair
        {
            List<GameObject> second_pair_check = check_pair(played_cards);
            if (second_pair_check != null && second_pair_check.Count > 0) // checks if there is a second pair
            {
                scored_cards = first_pair_check.Concat(second_pair_check).ToList();
            }
        }
        return scored_cards;
    }

    public static List<GameObject> check_pair(List<GameObject> played_cards)
    {
        List<GameObject> scored_cards = new List<GameObject>();
        return scored_cards;
    }

    public static List<GameObject> check_three_of_a_kind(List<GameObject> played_cards)
    {
        List<GameObject> scored_cards = new List<GameObject>();
        return scored_cards;
    }

    public static BaseScoreData get_high_card(List<GameObject> played_cards)
    {
        BaseScoreData base_score = new BaseScoreData();
        // adjust base_score for high card
        return base_score;

    }

}
