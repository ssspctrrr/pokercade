using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI pokerHandText;
    public TextMeshProUGUI scoreMultText;
    public TextMeshProUGUI scoreValueText;
    public TextMeshProUGUI scoreText;
    int score = 0;
    public int scoredAnimationOffset = 3;
    public TextAnimations textAnimation;
    public GameObject jokerManager;

    public void calculate_score(List<GameObject> played_cards, System.Action onComplete = null)
    {
        StartCoroutine(CalculateScoreRoutine(played_cards, onComplete));
    }
    private IEnumerator CalculateScoreRoutine(List<GameObject> played_cards, System.Action onComplete)
    {

        BaseScoreData base_score = new BaseScoreData();
        base_score = check_straight_and_or_flush(played_cards);
        if (base_score == default) { base_score = check_four_of_a_kind(played_cards); }
        if (base_score == default) { base_score = check_full_house_3_of_a_kind_pairs(played_cards); }
        if (base_score == default) { base_score = get_high_card(played_cards); }

        textAnimation.TransitionTextViaShake(pokerHandText, base_score.poker_hand);
        textAnimation.TransitionTextViaShake(scoreMultText, base_score.score_mult.ToString());
        textAnimation.TransitionTextViaShake(scoreValueText, base_score.score_value.ToString());

        yield return StartCoroutine(AnimateScoreCard(base_score));

    onComplete?.Invoke();
    }

    // Replace the entire 'AnimateScoreCard' IEnumerator with this:
IEnumerator AnimateScoreCard(BaseScoreData baseScore)
{
    // 1. Loop through every card that scored (e.g., the 5 cards in a flush)
    while (!baseScore.is_empty())
    {
        GameObject card = baseScore.dequeue();
        Vector3 startPos = card.transform.localPosition;

        // Visual: Move card up
        card.transform.localPosition += (card.transform.up);
        yield return new WaitForSeconds(0.5f);

        // --- THE MISSING LOGIC ---
        // Add this card's specific value (2, 10, 11, etc.) to the Chip Counter
        if (card.GetComponent<CardInstance>() != null)
        {
            baseScore.score_value += card.GetComponent<CardInstance>().GetValue();
        }
        // -------------------------

        // Visual: Shake the "Chips" text (scoreValueText) to show the number going up
        if (scoreValueText != null)
        {
            yield return StartCoroutine(textAnimation.TextShaker(scoreValueText, baseScore.score_value.ToString()));
        }

        // build joker context
        JokerContext context = new JokerContext();
        context.baseScore = baseScore;
        context.hand = 0;
        context.card = card;
        context.scoreMultText = scoreMultText;
        context.textAnimations = textAnimation;

        //Debug.Log(context.card == null ? "context.card is null" : "context.card exists");
        //Debug.Log(context.card.GetComponent<CardInstance>().data.id);

        //Debug.Log(jokerManager == null ? "jokerManager is null" : "jokerManager exists");
        //Debug.Log(jokerManager.GetComponent<JokerManager>() == null ? "JokerManager component missing" : "JokerManager component exists");
        //Debug.Log(context == null ? "context is null" : "context exists");

        jokerManager.GetComponent<JokerManager>().TriggerJokers(JokerTrigger.onScoreCard, context, baseScore);
        card.transform.localPosition = startPos;
        

        score += baseScore.score_value * baseScore.score_mult;
        textAnimation.TransitionTextViaShake(scoreText, score.ToString());

        // Visual: Move card back down
        card.transform.localPosition = startPos;
    }

    // 2. Calculate the Final Hand Total (Chips * Mult)
    int finalHandScore = baseScore.score_value * baseScore.score_mult;

    // 3. Send the total to the Game Manager to add to your Level Progress
    if (GameManager.instance != null)
    {
        GameManager.instance.AddScore(finalHandScore);
    }
}

    public void ResetText()
    {
        textAnimation.TransitionTextViaShake(pokerHandText, "???PokerHand???");
        textAnimation.TransitionTextViaShake(scoreMultText, "??Mult??");
        textAnimation.TransitionTextViaShake(scoreValueText, "??Value??");
    }
    public static BaseScoreData check_straight_and_or_flush(List<GameObject> played_cards)
    {
        if (played_cards.Count != 5)
            return default;

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
            else if (sorted_played_cards[i].GetComponent<CardInstance>().GetRank() - sorted_played_cards[i - 1].GetComponent<CardInstance>().GetRank() != 1)
            {
                is_normal_straight = false;
            }
        }
        List<int> sorted_int_rank_list = int_rank_list.OrderBy(rank => rank).ToList();
        if (is_normal_straight || sorted_int_rank_list.SequenceEqual(new List<int> { 1, 10, 11, 12, 13 }))
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
            else if (played_cards[i].GetComponent<CardInstance>().GetSuit() != played_cards[i - 1].GetComponent<CardInstance>().GetSuit())
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