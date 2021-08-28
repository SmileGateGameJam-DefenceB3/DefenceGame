using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected ItemView _view;

    public abstract void ApplyEffect(Actor actor);

    public void DestroySelf()
    {
        _collider.enabled = false;
        Destroy(gameObject);
    }

}
