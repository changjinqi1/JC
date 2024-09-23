using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartgame : MonoBehaviour

{
    public void ReloadScene()

    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ChangeToSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
