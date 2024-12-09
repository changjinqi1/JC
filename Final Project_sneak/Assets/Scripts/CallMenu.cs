using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CallMenu : MonoBehaviour
{
    private string menuSceneName = "Menu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMenuScene();
        }
    }

    private void LoadMenuScene()
    {
        if (Application.CanStreamedLevelBeLoaded(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }

    }
}
