using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpectatorHandler : MonoBehaviour
{
    public static CameraSpectatorHandler Instance;
    public float moveSpeed;

    Camera _mainCam;
    InputManager _inputManager;
    private bool camActive;
    [SerializeField] GameObject ui;

    private void Awake()
    {
        Instance = this;
        _mainCam = Camera.main;
        if (_inputManager == null)
        {
            _inputManager = InputManager.Instance;
        }
        _inputManager.OnStartReturn += ToggleCam;
        if(ui == null) ui = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if(!camActive) return;
        if (_inputManager.spectatorMovementInput.x < 0f)
        {
            //Move left
            transform.position += -Camera.main.transform.right * (moveSpeed * Time.deltaTime);
        }

        if (_inputManager.spectatorMovementInput.x > 0f)
        {
            //Move right
            transform.position += Camera.main.transform.right * (moveSpeed * Time.deltaTime);
        }

        if (_inputManager.spectatorMovementInput.y > 0f)
        {
            //Move forward
            transform.position += Camera.main.transform.forward * (moveSpeed * Time.deltaTime);
        }

        if (_inputManager.spectatorMovementInput.y < 0f)
        {
            //Move back
            transform.position += -Camera.main.transform.forward * (moveSpeed * Time.deltaTime);
        }

        if (_inputManager.spectatorUpDown == 1)
        {
            //Move up
            transform.position += Camera.main.transform.up * (moveSpeed * Time.deltaTime);
        }

        if (_inputManager.spectatorUpDown == -1)
        {
            //Move up
            transform.position += -Camera.main.transform.up * (moveSpeed * Time.deltaTime);
        }
    }

    void ToggleCam()
    {
        camActive = !camActive;
        if (!camActive) ui.SetActive(true);
        else ui.SetActive(false);

    }
}
