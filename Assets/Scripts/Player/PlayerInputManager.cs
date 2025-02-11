
using System;
using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInputManager : MonoBehaviour
{
   
    
    private GamePlayerInput _playerInput;

    public event EventHandler OnInteractionAction;
    public event EventHandler OnVechicleExitAction;
    private void Awake()
    {
        _playerInput = new GamePlayerInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerInput.Player.Interact.performed += InteractOnperformed;
        _playerInput.Vehicle.ExitVehicle.performed += ExitOnperformed;

    }

  

    private void InteractOnperformed(InputAction.CallbackContext obj)
    {
        OnInteractionAction?.Invoke(this, EventArgs.Empty);
    } 
    private void ExitOnperformed(InputAction.CallbackContext obj)
    {
        OnVechicleExitAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }
    
    private void OnDisable()
    {
        _playerInput.Vehicle.ExitVehicle.performed -= ExitOnperformed;
        _playerInput.Player.Interact.performed -= InteractOnperformed;
        _playerInput.Disable();
     

    }

    private void OnDestroy()
    {
        _playerInput.Player.Interact.performed -= InteractOnperformed;
        _playerInput.Vehicle.ExitVehicle.performed -= ExitOnperformed;

        _playerInput.Dispose();

    }

    public Vector2 GetPlayerMovement()
    {
        return _playerInput.Player.Move.ReadValue<Vector2>();
        
    }

    public Vector2 GetMouseDelta()
    {
        return _playerInput.Player.Look.ReadValue<Vector2>();
        
    }
    
}
