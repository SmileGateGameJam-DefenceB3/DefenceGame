using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public enum Result
    {
        Win,
        Lost,
        Draw,
    }

    public class UIGameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private GameObject _tuna;
        [SerializeField] private GameObject _clear;
        [SerializeField] private GameObject _lost;
        [SerializeField] private GameObject _retryButton;
        [SerializeField] private GameObject _nextButton;
        [SerializeField] private GameObject _creditButton;
        

        [Button()]
        public void win()
        {
            Open(Result.Win);
        }

        [Button()]
        public void lost()
        {
            Open(Result.Lost);
        }
        
        public void Open(Result result)
        {
            _lost.SetActive(false);
            _tuna.SetActive(true);
            _clear.SetActive(false);
            _retryButton.SetActive(false);
            _nextButton.SetActive(false);
            _creditButton.SetActive(false);
            
            switch (result)
            {
                case Result.Win:
                {
                    _title.text = "승리!";

                    if (InGameManager.StageIndex == 0)
                    {
                        _nextButton.SetActive(true);
                    }
                    else
                    {
                        _tuna.SetActive(false);
                        _clear.SetActive(true);
                        _creditButton.SetActive(true);
                    }
                    
                    break;
                }
                case Result.Lost:
                {
                    _title.text = "패배!";
                    _tuna.SetActive(false);
                    _lost.SetActive(true);
                    _retryButton.SetActive(true);
                    break;
                }
                case Result.Draw:
                {
                    _title.text = "무승부!";
                    _retryButton.SetActive(true);
                    break;
                }
            }

            gameObject.SetActive(true);
        }

        public void OnClick_Leave()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void OnClick_Retry()
        {
            SceneManager.LoadScene("StageScene");
        }
        
        public void OnClick_Next()
        {
            InGameManager.StageIndex++;
            SceneManager.LoadScene("StageScene");
        }

        public void OnClick_Credit()
        {
            SceneManager.LoadScene("CreditScene");
        }
    }
}
