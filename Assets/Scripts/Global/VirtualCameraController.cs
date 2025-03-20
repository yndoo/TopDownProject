using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    CinemachineVirtualCamera vCam;

    public int currentPriority = 5;
    public int activePriority = 20;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            vCam.Priority = activePriority;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            vCam.Priority = currentPriority;
        }
    }
}
