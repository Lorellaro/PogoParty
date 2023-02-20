using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class SetMyCamera : MonoBehaviour
{
    [SerializeField] Transform vCamTarget;

    PhotonView view;
    CinemachineFreeLook freeLookCam;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            freeLookCam = GameObject.FindGameObjectWithTag("freeLookCam").GetComponent<CinemachineFreeLook>();

            freeLookCam.Follow = vCamTarget;
            freeLookCam.LookAt = vCamTarget;
        }
    }
}
