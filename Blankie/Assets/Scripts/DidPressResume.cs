using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DidPressResume : MonoBehaviour
{
    public GameObject pauseScreen;

    public void Resume()
    {
        pauseScreen.SetActive(false);
    }
}
