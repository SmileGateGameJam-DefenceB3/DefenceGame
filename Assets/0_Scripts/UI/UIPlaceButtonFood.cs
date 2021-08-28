using UnityEngine;

namespace UI
{
    public class UIPlaceButtonFood : UIPlaceButton
    {
        public override int GetCost() => Constant.Instance.FoodCost;

        protected override void OnClickInternal()
        {
            _manager.OnClick_FoodButton(this);
        }
    }
}
