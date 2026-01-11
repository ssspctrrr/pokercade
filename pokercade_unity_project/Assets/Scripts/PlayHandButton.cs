using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHandButton : MonoBehaviour
{
    public GameObject selectionManager;
    private List<GameObject> selectedCards;
    public GameObject playedHand;

    [Header("Visual Layout")]
    public float cardSpacing = 1.0f;
    public float arcIntensity = 5.0f;
    public float moveSpeed = 2.0f;

    public void PlayHand(float duration = 1f)
    {
        selectedCards = selectionManager.GetComponent<SelectionManager>().selectedCards;
        if (selectedCards.Count == 0 || selectedCards == null)
            return;
        StartCoroutine(MoveToPlayedHandRoutine(selectedCards, selectedCards.Count, playedHand.transform));
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

            StartCoroutine(AnimateCardToPlayedHand(card, targetPos, targetRot, startPos, startRot));

            yield return new WaitForSeconds(0.1f);
        }
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
}
