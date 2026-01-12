using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayHandButton : MonoBehaviour
{
    public GameObject selectionManager;
    private List<GameObject> selectedCards;
    public GameObject playedHand;
    public GameObject hand;
    public ScoreManager scoreManager;
    public SpawnManager spawnManager;

    [Header("Visual Layout")]
    public float cardSpacing = 1.0f;
    public float arcIntensity = 5.0f;
    public float moveSpeed = 10.0f;

    public void PlayHand(float duration = 1f)
    {
        selectedCards = selectionManager.GetComponent<SelectionManager>().selectedCards;
        if (selectedCards.Count == 0 || selectedCards == null)
            return;

        EnableOrDisableCardSelection(hand, false);

        int amount = selectedCards.Count;

        StartCoroutine(MoveToPlayedHandRoutine(selectedCards, amount, playedHand.transform));
    }
    IEnumerator EndHandSequence(List<GameObject> cardsToDestroy)
    {

        foreach (GameObject card in cardsToDestroy)
        {
            Destroy(card);
        }

        selectedCards.Clear();

        yield return null;

        if (spawnManager != null)
        {
            spawnManager.RefillHand();
        }

        yield return new WaitForSeconds(1);

        EnableOrDisableCardSelection(hand, true);
        scoreManager.ResetText();
    }

    IEnumerator MoveToPlayedHandRoutine(List<GameObject> selectedCards, int amount, Transform playedHand)
    {
        float startX = -((amount - 1) * cardSpacing) / 2.0f;

        for (int i = 0; i < amount; i++)
        {
            GameObject card = selectedCards[i];

            card.transform.SetParent(playedHand.transform, true);

            Vector3 startPos = card.transform.localPosition;
            Quaternion startRot = card.transform.localRotation;

            float xPos = startX + (i * cardSpacing);

            Vector3 targetPos = new Vector3(xPos, 0, 0);
            Quaternion targetRot = Quaternion.identity;
            yield return AnimateCardToPlayedHand(card, targetPos, targetRot, startPos, startRot);
        }

        scoreManager.calculate_score(selectedCards, () =>
        {
            StartCoroutine(EndHandSequence(selectedCards));
        });
    }

    IEnumerator AnimateCardToPlayedHand(GameObject card, Vector3 targetPos, Quaternion targetRot, Vector3 startPos, Quaternion startRot)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            float smoothT = 1f - Mathf.Pow(1f - t, 3);

            card.transform.localPosition = Vector3.Lerp(startPos, targetPos, smoothT);
            card.transform.localRotation = Quaternion.Lerp(startRot, targetRot, smoothT);

            yield return null;
        }
    }

    void EnableOrDisableCardSelection(GameObject cardContainer, bool targetBool) 
    {
        foreach (Transform cardTransform in cardContainer.transform)
        {
            Selectable cardSelectable = cardTransform.gameObject.GetComponent<Selectable>();
            cardSelectable.enabled = targetBool;
        }
    }
}
