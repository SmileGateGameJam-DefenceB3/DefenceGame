using System;
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
        [SerializeField] private TextMeshProUGUI _content;
        [SerializeField] private GameObject _retryButton;

        public void Open(Result result)
        {
            switch (result)
            {
                case Result.Win:
                {
                    _title.text = "승리!";
                    _retryButton.SetActive(false);
                    break;
                }
                case Result.Lost:
                {
                    _title.text = "패배!";
                    break;
                }
                case Result.Draw:
                {
                    _title.text = "무승부!";
                    break;
                }
            }

            gameObject.SetActive(true);
        }

        public void OnClick_ToMain()
        {
            SceneManager.LoadScene(0);
        }
    }
}
