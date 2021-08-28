using System;
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

        private bool _isRunning;
        private float _value;
        private Sequence _runningSequence;

        private void Awake()
        {
            _value = 1f;
            _gauge.fillAmount = 1f;
            _reduceGauge.fillAmount = 1f;
        }

        public void SetValueNoAnimation(float value)
        {
            _value = value;
            _gauge.fillAmount = value;

            if (!_isRunning)
            {
                _reduceGauge.fillAmount = value;
            }
        }

        public void SetValue(float value)
        {
            _value = value;
            _runningSequence?.Kill();

            _isRunning = true;
            _runningSequence = DOTween.Sequence();
            _runningSequence.Append(_gauge.DOFillAmount(value, _gaugeSpeed)
                .SetSpeedBased(true));
            _runningSequence.AppendInterval(_gaugeReduceDelay);
            _runningSequence.Append(_reduceGauge.DOFillAmount(value, _reduceGaugeSpeed)
                .SetSpeedBased(true));

            _runningSequence.Play();
            _runningSequence.onComplete = () =>
            {
                _isRunning = false;
                _gauge.fillAmount = _value;
                _reduceGauge.fillAmount = _value;
            };
        }
    }
}
