using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartbutton : MonoBehaviour

{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
