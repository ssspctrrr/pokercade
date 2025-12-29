using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BaseScoreData : MonoBehaviour
{
    public string poker_hand;
    public int score_value;
    public int score_mult;
    public List<GameObject> scored_cards;
}
