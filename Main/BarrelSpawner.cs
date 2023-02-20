using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] GameObject barrel;
    [SerializeField] Animator buttonAnimator;

    InputManager _inputManager;
    PhotonView view;

    bool playerIsInRange = false;

    private void Start()
    {
        view = transform.GetComponent<PhotonView>();


            if (_inputManager == null)
            {
                _inputManager = InputManager.Instance;
            }

            _inputManager.OnStartInteract += spawnBarrel;
    }

    private void spawnBarrel()
    {
        if (playerIsInRange)
        {
            buttonAnimator.SetTrigger("ButtonPress");
            PhotonNetwork.Instantiate(this.barrel.name, transform.position, transform.rotation, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInRange = false;
        }
    }
}
