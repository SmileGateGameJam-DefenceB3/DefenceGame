using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : Item
{
    public override void Func(Actor actor)
    {
        Destroy(this.transform.gameObject);
    }
}
