using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIGameOverScreen : MonoBehaviour
    {
        public void OnClick_ToMain()
        {
            SceneManager.LoadScene(0);
        }
    }
}