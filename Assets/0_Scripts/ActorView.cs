using UnityEngine;

public class ActorView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
}
