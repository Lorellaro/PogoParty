using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public Vector2 movementInput;
    public Vector2 pedestalMovementInput;
    public Vector2 spectatorMovementInput;
    public float spectatorUpDown;
    private PlayerControls _playerControls;
    public bool jumpHeld = false;
    public bool interactHeld = false;
    public bool quitGame = false;

    public delegate void BaseAction();
    public event BaseAction OnStartJump;
    public event BaseAction OnPerformedJump;
    public event BaseAction OnEndJump;
    public event BaseAction OnStartQuit;
    public event BaseAction OnStartLoad;
    public event BaseAction OnStartShop;
    public event BaseAction OnStartInteract;
    public event BaseAction OnEndInteract;
    public event BaseAction OnStartReturn;
    public event BaseAction OnStartChangeSkin;

    private string currentControlInput = "Temp";

    // private Gamepad _gamepad;
    // private Keyboard _keyboard;

    private void Awake()
    {
        _playerControls = new PlayerControls();

    }

    private void OnEnable()
    {
        AllowInput(true);
        InputUser.onChange += onInputDeviceChange;
        ResetControllerRumble();
    }
    
    private void OnDisable()
    {
        ResetControllerRumble();
        InputUser.onChange -= onInputDeviceChange;
        AllowInput(false);
    }

    public void AllowInput(bool allowInput)
    {
        if (allowInput)
        {
            _playerControls.Enable();
            // Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            _playerControls.Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Start()
    {
        //Spectator
        // _playerControls.SpectatorMode.MainMovement.started += spectatorMovementPerformed;
        // _playerControls.SpectatorMode.UpDown.started += ctx => StartSpectatorUpDown();

        //Player
        _playerControls.Controls.Jump.started += ctx => StartJumpPrimary();
        _playerControls.Controls.Jump.performed += ctx => PerformedJumpPrimary();
        _playerControls.Controls.Jump.canceled += ctx => EndJumpPrimary();
        _playerControls.Controls.Quit.started += ctx => StartQuitPrimary();
        _playerControls.Controls.PlayerLoad.started += ctx => StartLoadPrimary();
        _playerControls.Controls.Shop.started += ctx => StartShopPrimary();
        _playerControls.Controls.Interact.started += ctx => StartInteractPrimary();
        _playerControls.Controls.Interact.canceled += ctx => EndInteractPrimary();
        _playerControls.Controls.Return.started += ctx => StartReturnPrimary();
        _playerControls.Controls.ChangeSkin.started += ctx => StartChangeSkinPrimary();
        // _playerControls.Controls.Interact.canceled += ctx => StopInteractPrimary();
        
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _playerControls.Controls.Look.ApplyBindingOverride(1,new InputBinding { overrideProcessors = "scaleVector2(x=50,y=50)"});
        }
    }

    private void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            if (user.controlScheme != null) currentControlInput = user.controlScheme.Value.name;
        }
    }
    
    private void spectatorMovementPerformed(InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();
        spectatorMovementInput = vec;
    }

    private void StartSpectatorUpDown()
    {
        spectatorUpDown = _playerControls.SpectatorMode.UpDown.ReadValue<float>();
    }

    private void StartJumpPrimary()
    {
        jumpHeld = true;
        OnStartJump?.Invoke();
    }

    private void PerformedJumpPrimary()
    {
        OnPerformedJump?.Invoke();
    }

    private void EndJumpPrimary()
    {
        jumpHeld = false;
        OnEndJump?.Invoke();
    }

    private void StartQuitPrimary()
    {
        OnStartQuit?.Invoke();
    }

    private void StartLoadPrimary()
    {
        OnStartLoad?.Invoke();
    }

    private void StartShopPrimary()
    {
        OnStartShop?.Invoke();
    }

    public void SetControllerRumble(float rumbleStrength)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(rumbleStrength,rumbleStrength);
        }
    }

    public void ResetControllerRumble()
    {
        if (Gamepad.current != null) Gamepad.current.ResetHaptics();
    }
    
     public void StartInteractPrimary()
     {
         //Question mark means check if null before invoke
    //     interactHeld = true;
         OnStartInteract?.Invoke();
    // }
    // public void StopInteractPrimary()
    // {
    //     //Question mark means check if null before invoke
    //     interactHeld = false;
    //     OnStopInteract?.Invoke();
     }

    public void EndInteractPrimary()
    {
        OnEndInteract?.Invoke();
    }

    public void StartReturnPrimary()
    {
        OnStartReturn?.Invoke();
    }
    public void StartChangeSkinPrimary()
    {
        OnStartChangeSkin?.Invoke();
    }

    private void Update()
    {
        Gamepad gamepad = Gamepad.current;
        Keyboard keyboard = Keyboard.current;
        if(_playerControls.Controls.Interact.ReadValue<float>() > 0.5f){
            interactHeld = true;
        }
        else{
            interactHeld = false;
        }
        if (_playerControls.Controls.Move.activeControl != null)
        {
            currentControlInput = _playerControls.Controls.Move.activeControl.layout;
        }
        if (keyboard != null && currentControlInput.Equals("Key"))
        {
            movementInput = _playerControls.Controls.Move.ReadValue<Vector2>();
            
        }
        else if (gamepad != null && Application.platform == RuntimePlatform.WebGLPlayer && currentControlInput.Equals("Button"))
        {
            movementInput = _playerControls.Controls.Move.ReadValue<Vector2>();
            movementInput.y *= -1;
        }
        else if (gamepad != null && Application.platform != RuntimePlatform.WebGLPlayer && currentControlInput.Equals("Button"))
        {
            movementInput = _playerControls.Controls.Move.ReadValue<Vector2>();
        }
        else
        {
            movementInput = _playerControls.Controls.Move.ReadValue<Vector2>();
        }

        //Spectator Movement
        spectatorMovementInput = _playerControls.Controls.Move.ReadValue<Vector2>();
        spectatorUpDown = _playerControls.SpectatorMode.UpDown.ReadValue<float>();

        //Pedestal
        if (_playerControls.Controls.RotatePedestalObj.activeControl != null)
        {
            currentControlInput = _playerControls.Controls.RotatePedestalObj.activeControl.layout;
        }
        if (keyboard != null && currentControlInput.Equals("Key"))
        {
            pedestalMovementInput = _playerControls.Controls.RotatePedestalObj.ReadValue<Vector2>();

        }
        else if (gamepad != null && Application.platform == RuntimePlatform.WebGLPlayer && currentControlInput.Equals("Button"))
        {
            pedestalMovementInput = _playerControls.Controls.RotatePedestalObj.ReadValue<Vector2>();
            pedestalMovementInput.y *= -1;
        }
        else if (gamepad != null && Application.platform != RuntimePlatform.WebGLPlayer && currentControlInput.Equals("Button"))
        {
            pedestalMovementInput = _playerControls.Controls.RotatePedestalObj.ReadValue<Vector2>();
        }
        else
        {
            pedestalMovementInput = _playerControls.Controls.RotatePedestalObj.ReadValue<Vector2>();
        }
    }

    private void OnDestroy()
    {
        ResetControllerRumble();
    }

    private void OnApplicationQuit()
    {
        ResetControllerRumble();
    }
    
}
