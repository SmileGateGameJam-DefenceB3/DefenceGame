using UnityEngine;

namespace UI
{
    public class UIPlaceButtonFood : UIPlaceButton
    {
        public override int GetCost() => 0;

        protected override void OnClickInternal()
        {
            _manager.OnClick_FoodButton(this);
        }
    }
}
