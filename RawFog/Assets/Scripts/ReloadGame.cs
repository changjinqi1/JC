using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadSceneOnClick : MonoBehaviour
{
    public Button reloadButton;

    void Start()
    {
        if (reloadButton != null)
        {
            reloadButton.onClick.AddListener(ReloadScene);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}