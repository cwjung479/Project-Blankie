using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Nate Trank
public class LevelSelectScript : MonoBehaviour
{
    public GameObject levels;
    public GameObject options;

    void Update()
    {
        Debug.Log(options.activeSelf + " " + Input.GetKeyDown(KeyCode.Escape));

        if (options.activeSelf && Input.GetKeyDown(KeyCode.Escape))
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
