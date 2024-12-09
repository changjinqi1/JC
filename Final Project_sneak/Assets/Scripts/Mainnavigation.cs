using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainnavigation : MonoBehaviour

{
    public void LoadLevel(int levelNumber)
    {
        string sceneName = "Level" + levelNumber;

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene " + sceneName + " does not exist or is not added to the build settings.");
        }
    }
}
