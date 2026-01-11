using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

public class ScoreManager : MonoBehaviour
{
    public Text poker_hand_text;
    public Text score_value_text;
    public Text score_mult_text;
    public Text score_text;
    int score = 0;

public void calculate_score(List<GameObject> played_cards, System.Action onComplete = null)
{
    StartCoroutine(CalculateScoreRoutine(played_cards, onComplete));
}

private System.Collections.IEnumerator CalculateScoreRoutine(List<GameObject> played_cards, System.Action onComplete)
{
    BaseScoreData base_score = new BaseScoreData();
        base_score = check_straight_and_or_flush(played_cards);
        if (base_score == default) { base_score = check_four_of_a_kind(played_cards); }
        if (base_score == default) { base_score = check_full_house_3_of_a_kind_pairs(played_cards); }
        if (base_score == default) { base_score = get_high_card(played_cards); }

        while (!base_score.is_empty()) 
        {
            base_score.score_value = base_score.score_value + base_score.dequeue().GetComponent<CardInstance>().GetValue();
        }
        score += base_score.score_value * base_score.score_mult;

        poker_hand_text.text = base_score.poker_hand;
        score_value_text.text = base_score.score_value.ToString();
        score_mult_text.text = base_score.score_mult.ToString();
        score_text.text = score.ToString();

    yield return new WaitForSeconds(1.0f); 

    onComplete?.Invoke();
}
    public void calculate_score(List<GameObject> played_cards)
    {
        
        BaseScoreData base_score = new BaseScoreData();
        base_score = check_straight_and_or_flush(played_cards);
        if (base_score == default) { base_score = check_four_of_a_kind(played_cards); }
        if (base_score == default) { base_score = check_full_house_3_of_a_kind_pairs(played_cards); }
        if (base_score == default) { base_score = get_high_card(played_cards); }

        while (!base_score.is_empty()) 
        {
            base_score.score_value = base_score.score_value + base_score.dequeue().GetComponent<CardInstance>().GetValue();
        }
        score += base_score.score_value * base_score.score_mult;

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

        if (straight != default && flush != default) 
        {
            base_score.poker_hand = "Straight Flush";
            base_score.score_value = 100;
            base_score.score_mult = 8;
            base_score.initialize_scored_cards(played_cards.ToList());
            return base_score;
        }
        else if (straight != default)
        {
            base_score.poker_hand = "Straight";
            base_score.score_value = 30;
            base_score.score_mult = 4;
            base_score.initialize_scored_cards(straight);
            return base_score;
        }
        else if (flush != default)
        {
            base_score.poker_hand = "Flush";
            base_score.score_value = 35;
            base_score.score_mult = 4;
            base_score.initialize_scored_cards(flush);
            return base_score;
        }
        // else both straight and flush is null, return null
        return default;
    }

    public static List<GameObject> check_straight(List<GameObject> played_cards)
    {
        List<GameObject> sorted_played_cards = played_cards.OrderBy(card => (int)card.GetComponent<CardInstance>().GetRank()).ToList();
        List<int> int_rank_list = new List<int>();
        bool is_normal_straight = true;
        for (int i = 0; i < sorted_played_cards.Count; i++)
        {
            int_rank_list.Add((int)sorted_played_cards[i].GetComponent<CardInstance>().GetRank());
            if (i == 0)
            {
                continue;
            }
            else if (sorted_played_cards[i].GetComponent<CardInstance>().GetRank() - sorted_played_cards[i-1].GetComponent<CardInstance>().GetRank() != 1)
            {
                is_normal_straight = false;
            }
        }
        List<int> sorted_int_rank_list = int_rank_list.OrderBy(rank => rank).ToList();
        if (is_normal_straight || sorted_int_rank_list.SequenceEqual(new List<int> {1,10,11,12,13}))
        {
            return played_cards.ToList();
        }
        else
        {
            return default;
        }
    }

    public static List<GameObject> check_flush(List<GameObject> played_cards)
    {
        for (int i = 0; i < played_cards.Count; i++) 
        { 
            if (i == 0)
            {
                continue;
            }
            else if (played_cards[i].GetComponent<CardInstance>().GetSuit() != played_cards[i-1].GetComponent<CardInstance>().GetSuit())
            {
                return default;
            }
        }
        return played_cards.ToList();
    }

    public static BaseScoreData check_four_of_a_kind(List<GameObject> played_cards)
    {
        BaseScoreData base_score = new BaseScoreData();
        List<Rank> rank_list = new List<Rank>();
        Rank rank_of_four_of_a_kind = new Rank();

        foreach (GameObject card in played_cards)
        {
            CardInstance card_data = card.GetComponent<CardInstance>();
            rank_list.Add(card_data.GetRank());
        }

        foreach (Rank current_rank in rank_list)
        {
            if (rank_list.Count(each_card_rank => each_card_rank == current_rank) == 4)
            {
                rank_of_four_of_a_kind = current_rank;
                break;
            }
        }
        if (rank_of_four_of_a_kind == default)
        {
            return default;
        }

        foreach (GameObject card in played_cards)
        {
            if (card.GetComponent<CardInstance>().GetRank() == rank_of_four_of_a_kind)
            {
                base_score.enqueue(card);
            }
        }
        base_score.poker_hand = "Four of a Kind";
        base_score.score_value = 60;
        base_score.score_mult = 7;
        return base_score;
    }

    public static BaseScoreData check_full_house_3_of_a_kind_pairs(List<GameObject> played_cards)
    {
        BaseScoreData base_score = new BaseScoreData();
        List<GameObject> pairs = check_for_pairs(played_cards);
        List<GameObject> three_of_a_kind = check_three_of_a_kind(played_cards);
        if (pairs != default && pairs.Count == 2)
        {
            if (three_of_a_kind != default)
            {
                base_score.poker_hand = "Full House";
                base_score.score_value = 40;
                base_score.score_mult = 4;
                base_score.initialize_scored_cards(played_cards.ToList()); // since all played cards are scored
                return base_score;
            }
            base_score.poker_hand = "Pair";
            base_score.score_value = 10;
            base_score.score_mult = 2;
            base_score.initialize_scored_cards(pairs);
            return base_score;
        }
        else if (pairs != default && pairs.Count == 4) 
        {
            base_score.poker_hand = "Two Pair";
            base_score.score_value = 20;
            base_score.score_mult = 2;
            base_score.initialize_scored_cards(pairs);
            return base_score;
        }
        else if (three_of_a_kind != default)
        {
            base_score.poker_hand = "Three of a Kind";
            base_score.score_value = 30;
            base_score.score_mult = 3;
            base_score.initialize_scored_cards(three_of_a_kind);
            return base_score;
        }
        else
        {
            return default;
        }
    }

    public static List<GameObject> check_for_pairs(List<GameObject> played_cards_orig) 
    {
        List<GameObject> played_cards_copy = played_cards_orig.ToList();
        List<GameObject> scored_cards = new List<GameObject>();
        List<GameObject> first_pair_check = check_pair(played_cards_copy);
        if (first_pair_check != default && first_pair_check.Count > 0) // checks if there is a pair
        {
            foreach (GameObject card in first_pair_check)
            {
                played_cards_copy.Remove(card);
            }
            List<GameObject> second_pair_check = check_pair(played_cards_copy);
            if (second_pair_check != default && second_pair_check.Count > 0) // checks if there is a second pair
            {
                List<GameObject> pairs = first_pair_check.Concat(second_pair_check).ToList();
                foreach (GameObject pair_card in pairs)
                {
                    if (played_cards_orig.Contains(pair_card))
                    {
                        scored_cards.Add(pair_card);
                    }
                }
                return scored_cards;
            }
        }
        return first_pair_check;
    }

    public static List<GameObject> check_pair(List<GameObject> played_cards)
    {
        List<GameObject> scored_cards = new List<GameObject>();
        List<Rank> rank_list = new List<Rank>();
        Rank rank_of_pair = new Rank();

        foreach (GameObject card in played_cards)
        {
            CardInstance card_data = card.GetComponent<CardInstance>();
            rank_list.Add(card_data.GetRank());
        }

        foreach (Rank current_rank in rank_list)
        {
            if (rank_list.Count(each_card_rank => each_card_rank == current_rank) == 2)
            {
                rank_of_pair = current_rank;
                break;
            }
        }
        if (rank_of_pair == default)
        {
            return default;
        }

        foreach (GameObject card in played_cards)
        {
            if (card.GetComponent<CardInstance>().GetRank() == rank_of_pair)
            {
                scored_cards.Add(card);
            }
        }
        return scored_cards;
    }

    public static List<GameObject> check_three_of_a_kind(List<GameObject> played_cards)
    {
        List<GameObject> scored_cards = new List<GameObject>();
        List<Rank> rank_list = new List<Rank>();
        Rank rank_of_three_of_a_kind = new Rank();

        foreach (GameObject card in played_cards)
        {
            CardInstance card_data = card.GetComponent<CardInstance>();
            rank_list.Add(card_data.GetRank());
        }

        foreach (Rank current_rank in rank_list)
        {
            if (rank_list.Count(each_card_rank => each_card_rank == current_rank) == 3)
            {
                rank_of_three_of_a_kind = current_rank;
                break;
            }
        }
        if (rank_of_three_of_a_kind == default)
        {
            return default;
        }

        foreach (GameObject card in played_cards)
        {
            if (card.GetComponent<CardInstance>().GetRank() == rank_of_three_of_a_kind)
            {
                scored_cards.Add(card);
            }
        }
        return scored_cards;
    }

    public static BaseScoreData get_high_card(List<GameObject> played_cards)
    {
        List<GameObject> sorted_played_cards = played_cards.OrderByDescending(card => (int)card.GetComponent<CardInstance>().GetRank()).ToList();

        // adjust base_score for high card
        BaseScoreData base_score = new BaseScoreData();
        base_score.poker_hand = "High Card";
        base_score.score_value = 5;
        base_score.score_mult = 1;
        if (sorted_played_cards[^1].GetComponent<CardInstance>().GetRank() == Rank.Ace)
        {
            base_score.enqueue(sorted_played_cards[^1]);
        }
        else
        {
            base_score.enqueue(sorted_played_cards[0]);
        }
        return base_score;

    }
}
