using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : Item
{
    public int value = 10;
    public override void Func(Actor actor)
    {
        var Kingdom = InGameManager.Instance.GetKingdom(actor.Team);
        Kingdom.Life += value;
        Destroy(this.transform.gameObject);
    }
}
