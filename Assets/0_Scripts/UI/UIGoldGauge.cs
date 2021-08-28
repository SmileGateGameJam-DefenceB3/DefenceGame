using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIGoldGauge : MonoBehaviour
    {
        [SerializeField] private Image _gauge;
        [SerializeField] private TextMeshProUGUI _text;

        private string _originalString;
        private const string MoneyParameter = "$$";

        private void Awake()
        {
            _originalString = _text.text;
        }

        public void SetValue(int value, int maxValue)
        {
            _gauge.fillAmount = (float) value / maxValue;
            _text.text = _originalString.Replace(MoneyParameter, value.ToString());
        }
    }
}
