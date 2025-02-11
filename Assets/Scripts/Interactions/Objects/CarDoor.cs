using System;
using DefaultNamespace;
using UnityEngine;
using Ezereal;
namespace Interactions.Objects
{
    
    [System.Serializable]
    public struct VehicleInteractionRefs
    {
        public Transform playerTransform;
        public Transform VehicleSeatTransform;
        public PlayerController playerController;
        public GameObject playerCamera;
        public GameObject carCamera;
        public CharacterController CharacterController;
        public EzerealCarController ezerealCarController;

    }
    public class CarDoor : BaseObject
    {
        [SerializeField] private VehicleInteractionRefs _vehicleInteractionRefs;
        public override void Interact(PlayerController player)
        {
            PlayerStateManager.SetState(PlayerState.InVehicle);
            _vehicleInteractionRefs.playerController.stopMovement = true;
            _vehicleInteractionRefs.playerTransform.position = _vehicleInteractionRefs.VehicleSeatTransform.position;
            _vehicleInteractionRefs.playerTransform.rotation = _vehicleInteractionRefs.VehicleSeatTransform.rotation;
            // _vehicleInteractionRefs.camera[0].SetActive(false);
            _vehicleInteractionRefs.playerCamera.SetActive(false);
            _vehicleInteractionRefs.carCamera.SetActive(true);
            _vehicleInteractionRefs.CharacterController.enabled = false;
            _vehicleInteractionRefs.ezerealCarController.enabled = true;
        }

        public void Update()
        {
            Debug.Log("state: " + PlayerStateManager.CurrentState);
        }
    }
}