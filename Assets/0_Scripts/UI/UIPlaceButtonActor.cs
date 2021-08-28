using UnityEngine;

namespace UI
{
    public class UIPlaceButtonActor : UIPlaceButton
    {
        [SerializeField] private ActorType _actorType;

        public ActorType ActorType => _actorType;
        
        public override void OnClick()
        {
            _manager.OnClick_ActorButton(this);
        }
    }
}
