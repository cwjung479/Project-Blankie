using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    public GameObject levels;
    public GameObject options;

    void Update()
    {
        if (options.active && Input.GetKeyDown(KeyCode.Escape))
        {
            options.SetActive(false);
            levels.SetActive(true);
        }
    }

    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
