using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectableCard : MonoBehaviour, IPointerUpHandler
{
    public float pointerUpTime;
    public float pointerDownTime;
    public bool selected;
    public int selectionOffset = 50;
    public Transform cardTransform;
    public Vector3 originalCoords;
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        pointerUpTime = Time.time;
        //PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);
        if (pointerUpTime - pointerDownTime > .2f)
            return;
        //if (wasDragged)
        //    return;
        selected = !selected;
        //SelectEvent.Invoke(this, selected);
        if (selected)
            transform.localPosition += (cardTransform.up * selectionOffset);
        else
            transform.localPosition = originalCoords;
    }
    void Awake()
    {
        originalCoords = transform.localPosition;
    }

    public void UpdateBasePosition(Vector3 newLocalPos) // invoke tong function if nadrag ung card (for futureproofing lang)
    {
        originalCoords = newLocalPos;
        if (!selected)
            transform.localPosition = originalCoords;
    }
}
