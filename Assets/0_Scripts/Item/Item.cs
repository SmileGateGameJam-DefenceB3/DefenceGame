using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected ItemView _view;

    Vector3 oriPos;
    public void DestroySelf()
    {
        _collider.enabled = false;
        Destroy(gameObject);
    }

    private void Awake()
    {
        oriPos = transform.position;
        Debug.Log("!!CREATE!!");
        StartCoroutine(AwakeEffectCo());
    }
    IEnumerator AwakeEffectCo()
    {
        transform.DOScale(new Vector3(0.2f, 0.2f, 1f), 0.8f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.8f);
        transform.DOMoveY(oriPos.y + 0.075f, 1.0f, false).SetLoops(-1, LoopType.Yoyo);
    }
    private void Update()
    {
    }
}
