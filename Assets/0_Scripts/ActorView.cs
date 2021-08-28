using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ActorView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Helmet _helmet;

    private List<SpriteRenderer> _spriteRenderers;
    private Tween _levelUpTween;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    public void OnLevelChanged(int level, bool isInitial)
    {
        if (!isInitial)
        {
            _levelUpTween?.Kill();

            transform.localScale = Vector3.one;

            _levelUpTween = transform.DOPunchScale(Vector3.one * 0.25f, 0.75f, 1)
                .SetEase(Ease.InOutQuad);
        }

        _helmet.OnLevelChanged(level);
    }

    public void AdjustSortingOrders(int adjust)
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.sortingOrder += adjust;
        }
    }
    
    public void Activate()
    {
        _animator.enabled = true;
    }

    public async UniTask Attack()
    {
        _animator.SetTrigger("Attack");
        _animator.Update(0);
        await UniTask.NextFrame();
        await UniTask.WaitUntil(() => !_animator.IsInTransition(0));
        await UniTask.Delay(Mathf.CeilToInt(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * 1000f));
    }

    public async UniTask Die()
    {
        _animator.SetTrigger("Die");
        _animator.Update(0);
        await UniTask.NextFrame();
        await UniTask.WaitUntil(() => !_animator.IsInTransition(0));
        await UniTask.Delay(Mathf.CeilToInt(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * 1000f));
    }

    public async UniTask FadeOut()
    {
        _animator.enabled = false;
        
        UniTask task = default;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            task = spriteRenderer.DOFade(0, 0.75f).ToUniTask();
        }

        await task;
    }

    private void OnDestroy()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.DOKill();
        }
    }
}
