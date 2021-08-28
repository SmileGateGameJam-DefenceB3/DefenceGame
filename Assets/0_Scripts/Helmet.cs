using System;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnLevelChanged(int level)
    {
        if (level == 1)
        {
            _spriteRenderer.sprite = null;
        }
        else
        {
            _spriteRenderer.sprite = _sprites[level - 2];
        }
    }
}
