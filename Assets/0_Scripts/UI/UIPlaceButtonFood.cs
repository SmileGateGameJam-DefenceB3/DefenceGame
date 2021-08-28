using UnityEngine;

namespace UI
{
    public class UIPlaceButtonFood : UIPlaceButton
    {
        public override void OnClick()
        {
            _manager.OnClick_FoodButton(this);
        }
    }
}
