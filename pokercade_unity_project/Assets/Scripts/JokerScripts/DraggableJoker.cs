using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Selectable))]
public class DraggableJoker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Camera mainCamera;
    private Vector3 offset;
    private Transform parentTransform;
    private JokerManager jokerManager;
    private BoxCollider2D col;
    private SpriteRenderer spriteRenderer;
    private bool isDragging = false;
    
    // NEW: Variable to remember your inspector scale
    private Vector3 originalScale;

    private void Awake()
    {
        mainCamera = Camera.main;
        parentTransform = transform.parent;
        jokerManager = parentTransform.GetComponent<JokerManager>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 1. Memorize the scale you set in the Inspector
        originalScale = transform.localScale;
        
        // 2. Auto-fit the collider
        UpdateColliderSize();
    }

    [ContextMenu("Fit Collider to Sprite")]
    public void UpdateColliderSize()
    {
        if (spriteRenderer != null && col != null && spriteRenderer.sprite != null)
        {
            col.size = spriteRenderer.sprite.bounds.size;
            col.offset = Vector2.zero; 
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        
        // Capture the offset
        Vector3 mouseWorldPos = GetMouseWorldPos(eventData.position);
        offset = transform.position - mouseWorldPos;
        
        // FIX: Multiply based on ORIGINAL scale, not Vector3.one
        transform.localScale = originalScale * 1.1f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 mouseWorldPos = GetMouseWorldPos(eventData.position);
        
        // Move X only, keep Y and Z stable
        transform.position = new Vector3(mouseWorldPos.x + offset.x, transform.position.y, transform.position.z);
        
        UpdateSiblingIndex();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        
        // FIX: Return to ORIGINAL scale
        transform.localScale = originalScale;
        
    }

    private Vector3 GetMouseWorldPos(Vector2 screenPos)
    {
        Vector3 mousePoint = new Vector3(screenPos.x, screenPos.y, 0);
        mousePoint.z = -mainCamera.transform.position.z; 
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private void UpdateSiblingIndex()
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform other = parentTransform.GetChild(i);
            if (transform == other) continue;

            if (transform.position.x > other.position.x && transform.GetSiblingIndex() < other.GetSiblingIndex())
            {
                transform.SetSiblingIndex(other.GetSiblingIndex());
                break; 
            }
            else if (transform.position.x < other.position.x && transform.GetSiblingIndex() > other.GetSiblingIndex())
            {
                transform.SetSiblingIndex(other.GetSiblingIndex());
                break; 
            }
        }
    }
}