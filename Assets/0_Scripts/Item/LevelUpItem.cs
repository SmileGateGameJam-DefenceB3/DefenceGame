using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelUpItem : Item
{
    public override void Func(Actor actor)
    {
        Debug.Log("�浹!!");
        if (actor is null)
            return;
        actor.LevelUp();
        Destroy(this.transform.gameObject);
    }
}
