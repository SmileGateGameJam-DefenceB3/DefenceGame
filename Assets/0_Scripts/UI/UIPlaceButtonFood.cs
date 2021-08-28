using UnityEngine;

namespace UI
{
    public class UIPlaceButtonFood : UIPlaceButton
    {
        public override int GetCost() => Constant.Instance.FoodCost;

        public override void OnClick()
        {
            _manager.OnClick_FoodButton(this);
        }
    }
}
