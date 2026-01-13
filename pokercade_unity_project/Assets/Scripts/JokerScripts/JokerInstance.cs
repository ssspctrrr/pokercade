using UnityEngine;

public class JokerInstance : MonoBehaviour
{
    public JokerData jokerData;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer.sprite = jokerData.sprite;
    }
}
