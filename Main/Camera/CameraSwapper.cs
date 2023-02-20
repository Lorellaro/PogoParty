using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwapper : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook CMFreeLook;
    [SerializeField] CinemachineFreeLook spectatorFreeLookCam;

    InputManager _inputManager;

    private void Awake()
    {
        if(CMFreeLook == null)
        {
            CMFreeLook = GameObject.FindGameObjectWithTag("freeLookCam").GetComponent<CinemachineFreeLook>();
        }
    }

    private void OnEnable()
    {
        if (_inputManager == null)
        {
            _inputManager = InputManager.Instance;
        }

        _inputManager.OnStartReturn += swapCam;
    }

    private void OnDisable()
    {
        _inputManager.OnStartReturn -= swapCam;
    }

    private void swapCam()
    {
        if (spectatorFreeLookCam.m_Priority == 10)
        {
            spectatorFreeLookCam.m_Priority = 0;
            CMFreeLook.m_Priority = 10;
        }
        else
        {
            CMFreeLook.m_Priority = 0;
            spectatorFreeLookCam.m_Priority = 10;
        }        
    }
}
