using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIActorPlaceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Actor _actorPrefab;

        private UIActorPlaceManager _manager;

        public Actor ActorPrefab => _actorPrefab;
        
        public void Initialize(UIActorPlaceManager manager)
        {
            _manager = manager;
        }

        public void OnClick()
        {
            _manager.OnClick_ActorButton(this);
        }

        public void SetPressed(bool isPressed)
        {
            _button.interactable = !isPressed;
        }
    }
}
