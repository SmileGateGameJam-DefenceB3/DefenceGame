using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIPlaceButtonActor : UIPlaceButton
    {
        [SerializeField] private ActorType _actorType;
        [SerializeField] private TextMeshProUGUI _priceText;

        public ActorType ActorType => _actorType;

        public override int GetCost() => InGameManager.ActorManager.GetActorData(ActorType).Cost;

        private void Start()
        {
            var actorData = InGameManager.ActorManager.GetActorData(ActorType);
            _priceText.text = $"{actorData.Name} 소환 ${actorData.Cost}";
        }

        protected override void OnClickInternal()
        {
            _manager.OnClick_ActorButton(this);
        }
    }
}
