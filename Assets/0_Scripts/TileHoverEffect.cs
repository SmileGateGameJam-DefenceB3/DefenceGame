using UnityEngine;

public class TileHoverEffect : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.size = Constant.Instance.TileSize;
    }
    
    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
