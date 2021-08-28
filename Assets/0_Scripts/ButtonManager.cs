using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.0f;
    public void Exit()
    {

        StartCoroutine(GameExitCo());
    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        //play animation
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator GameExitCo()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }
}