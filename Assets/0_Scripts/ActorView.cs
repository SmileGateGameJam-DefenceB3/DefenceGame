using DG.Tweening;
using UnityEngine;

public class ActorView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    private Tween _levelUpTween;

    public void OnLevelChanged(int level)
    {
        _levelUpTween?.Kill();

        transform.localScale = Vector3.one;

        _levelUpTween = transform.DOPunchScale(Vector3.one * 0.25f, 0.75f, 1)
            .SetEase(Ease.InOutQuad);
    }
}
