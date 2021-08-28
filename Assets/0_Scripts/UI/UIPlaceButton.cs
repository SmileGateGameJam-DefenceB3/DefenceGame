using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class UIPlaceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _border;

        [SerializeField] private Color _normalBorderColor;
        [SerializeField] private Color _pressedBorderColor;

        private KeyCode _keyCode;
        protected UIPlaceManager _manager;

        public void Initialize(UIPlaceManager manager)
        {
            _keyCode = KeyCode.Alpha1 + transform.GetSiblingIndex();

            _manager = manager;
            InGameManager.Instance.OnGoldChanged.AddListener(value => { _button.interactable = value >= GetCost(); });
        }

        public abstract int GetCost();
        protected abstract void OnClickInternal();

        public void OnClick()
        {
            SoundManager.PlaySfx(ClipType.UIClick);
            OnClickInternal();
        }

        private void Update()
        {
            if (InGameManager.Instance.GameState != GameState.Playing)
            {
                return;
            }

            if (!_button.interactable)
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
            _border.color = isPressed ? _pressedBorderColor : _normalBorderColor;
        }
    }
}
