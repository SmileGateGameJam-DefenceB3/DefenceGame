using UnityEngine;

namespace UI
{
    public class UIPlaceButtonActor : UIPlaceButton
    {
        [SerializeField] private ActorType _actorType;

        public ActorType ActorType => _actorType;

        public override int GetCost() => InGameManager.ActorManager.GetActorData(ActorType).Cost;

        public override void OnClick()
        {
            _manager.OnClick_ActorButton(this);
        }
    }
}
