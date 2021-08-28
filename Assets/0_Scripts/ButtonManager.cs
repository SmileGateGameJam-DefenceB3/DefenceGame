using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void GameStart(string sceneName)
    {
        Debug.Log("!!");
        SceneManager.LoadScene(sceneName);       
    }
    public void GameExit()
    {

    }
    public void ShowCredit()
    {

    }
    public void GameSetting()
    {

    }
}
