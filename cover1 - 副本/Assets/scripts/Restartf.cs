using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restartf : MonoBehaviour
{
    public void RestartScene()
    {

        SceneManager.LoadScene("SampleScene");

    }
}