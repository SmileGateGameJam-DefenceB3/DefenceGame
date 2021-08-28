using System;

public class HeartItem : Item
{
    public int value = 10;
    public override void Func(Actor actor)
    {
        var Kingdom = InGameManager.Instance.GetKingdom(actor.Team);
        Kingdom.Life = Math.Min(Kingdom.Life + 5, Constant.Instance.MaxHP);
        SoundManager.PlaySfx(ClipType.TakeHeart);
        Destroy(this.transform.gameObject);
    }
}
