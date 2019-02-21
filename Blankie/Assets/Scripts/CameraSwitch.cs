using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera linkedCam;
    public Cinemachine.CinemachineVirtualCamera oldCam;

    private void OnTriggerEnter2D(Collider2D collision) {
        linkedCam.Priority = 11;
        oldCam.Priority = 10;
    }
}
