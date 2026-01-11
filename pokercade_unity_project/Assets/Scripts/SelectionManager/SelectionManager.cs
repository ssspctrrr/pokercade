using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GameObject handSlot;
    private List<GameObject> selectedCards;
    private GameObject card;
    private bool cardSelected;

    void Update()
    {
        selectedCards = new List<GameObject>();
        foreach (Transform cardTransform in handSlot.transform)
        {
            CardInstance cardInstance = cardTransform.GetComponent<CardInstance>();
            if (cardInstance.selected)
            {
                selectedCards.Add(cardTransform.gameObject);
            }
        }
        bool atMaxSelectedCard = selectedCards.Count >= 5;
        foreach (Transform cardTransform in handSlot.transform)
        {
            CardInstance cardInstance = cardTransform.GetComponent<CardInstance>();
            BoxCollider2D collider = cardTransform.GetComponent<BoxCollider2D>();

            if (!cardInstance.selected && atMaxSelectedCard)
            {
                collider.enabled = false;
            }
            else
            {
                collider.enabled = true;
            }
        }
    }
}
