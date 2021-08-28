using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class UIPlaceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshProUGUI _sc;

        private List<KeyCode> _keyCodes;
        protected UIPlaceManager _manager;

        public void Initialize(UIPlaceManager manager, List<KeyCode> keyCodes)
        {
            _keyCodes = keyCodes;

            _manager = manager;
            InGameManager.Instance.OnGoldChanged.AddListener(value => { _button.interactable = value >= GetCost(); });
            _sc.text = _keyCodes[1].ToString();
        }

        public abstract int GetCost();
        protected abstract void OnClickInternal();

        public void OnClick()
        {
            SetPressed(true);
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

            foreach (var keyCode in _keyCodes)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    OnClick();
                    return;
                }
            }
        }

        public void SetPressed(bool isPressed)
        {
            if (!_button.interactable)
            {
                return;
            }
            
            if (isPressed)
            {
                _animator.SetTrigger("Pressed");
            }
            else
            {
                _animator.ResetTrigger("Pressed");
                _animator.SetTrigger("Normal");
            }
        }
    }
}
