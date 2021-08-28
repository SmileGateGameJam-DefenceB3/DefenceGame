using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHpBar : MonoBehaviour
    {
        [SerializeField] private Image _gauge;
        [SerializeField] private Image _reduceGauge;

        [SerializeField] private float _reduceGaugeSpeed;
        [SerializeField] private float _gaugeSpeed;
        [SerializeField] private float _gaugeReduceDelay;

        private Sequence _runningSequence;

        private void Awake()
        {
            _gauge.fillAmount = 1f;
            _reduceGauge.fillAmount = 1f;
        }

        public void SetValue(float ratio)
        {
            _runningSequence?.Kill();

            _runningSequence = DOTween.Sequence();
            _runningSequence.Append(_gauge.DOFillAmount(ratio, _gaugeSpeed)
                .SetSpeedBased(true));
            _runningSequence.AppendInterval(_gaugeReduceDelay);
            _runningSequence.Append(_reduceGauge.DOFillAmount(ratio, _reduceGaugeSpeed)
                .SetSpeedBased(true));

            _runningSequence.Play();
        }
    }
}