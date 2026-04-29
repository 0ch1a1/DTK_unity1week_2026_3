using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    public void ChangeScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
