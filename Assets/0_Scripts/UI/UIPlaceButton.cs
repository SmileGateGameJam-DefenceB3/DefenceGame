using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class UIPlaceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private KeyCode _keyCode;
        
        protected UIPlaceManager _manager;

        public void Initialize(UIPlaceManager manager)
        {
            _manager = manager;
        }

        public abstract void OnClick();

        private void Update()
        {
            if (InGameManager.Instance.GameState != GameState.Playing)
            {
                return;
            }
            
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
