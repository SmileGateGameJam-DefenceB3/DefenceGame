using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected ItemView _view;
    [SerializeField] private Vector3 scale = new Vector3();
    Vector3 oriPos;
    public abstract void Func(Actor actor);

    private void Awake()
    {
        oriPos = transform.position;
        StartCoroutine(AwakeEffectCo());
    }
    IEnumerator AwakeEffectCo()
    {
        var viewImage = transform.Find("ViewImage").gameObject;
        viewImage.transform.DOScale(scale, 0.8f).SetEase(Ease.OutBounce);
        var shadowImage = transform.Find("ShaderImage").gameObject;
        shadowImage.transform.DOScale(scale, 0.8f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.8f);
        viewImage.transform.DOMoveY(oriPos.y + 0.075f, 1.0f, false).SetLoops(-1, LoopType.Yoyo);
        //shadowImage.transform.DOMoveY(oriPos.y + 0.075f, 1.0f, false).SetLoops(-1, LoopType.Yoyo);
    }
}
