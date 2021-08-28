using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ActorView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private List<SpriteRenderer> _spriteRenderers;
    private Tween _levelUpTween;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    public void OnLevelChanged(int level)
    {
        _levelUpTween?.Kill();

        transform.localScale = Vector3.one;

        _levelUpTween = transform.DOPunchScale(Vector3.one * 0.25f, 0.75f, 1)
            .SetEase(Ease.InOutQuad);
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
}
