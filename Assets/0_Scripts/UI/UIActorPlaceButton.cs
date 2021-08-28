using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIActorPlaceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private KeyCode _keyCode;
        [SerializeField] private ActorType _actorType;
        
        private UIActorPlaceManager _manager;

        public ActorType ActorType => _actorType;
        
        public void Initialize(UIActorPlaceManager manager)
        {
            _manager = manager;
        }

        public void OnClick()
        {
            _manager.OnClick_ActorButton(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_keyCode))
            {
                OnClick();
            }
        }

        public void SetPressed(bool isPressed)
        {
            _button.interactable = !isPressed;
        }
    }
}
