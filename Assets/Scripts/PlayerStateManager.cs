using System;
using DefaultNamespace;
using UnityEngine;

public class PlayerStateManager
{
    public static event Action OnEnterVehicle;
    public static event Action OnExitVehicle;

    private static PlayerState _currentState = PlayerState.NotInVehicle;

    public static PlayerState CurrentState
    {
        get => _currentState;
        private set
        {
            
            if (_currentState == value) return; 

            _currentState = value;
            if (_currentState == PlayerState.InVehicle)
            {
                OnEnterVehicle?.Invoke();
            }
               
            else
                OnExitVehicle?.Invoke();
        }
    }

    public static void SetState(PlayerState newState)
    {
        CurrentState = newState;
    }
}