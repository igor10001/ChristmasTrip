
using System;
using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInputManager : MonoBehaviour
{
    private GamePlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new GamePlayerInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnDestroy()
    {
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
