using System;
using UnityEngine;
using Ezereal;

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
public class VehicleInteraction : MonoBehaviour
{
    
    [SerializeField] private VehicleInteractionRefs _vehicleInteractionRefs;
     public static event Action<Transform> OnEnterVehicle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EnterVehicle();
        }    
    }

    private void EnterVehicle()
    {
        OnEnterVehicle?.Invoke(_vehicleInteractionRefs.VehicleSeatTransform);
        _vehicleInteractionRefs.playerController.enabled = false;
        _vehicleInteractionRefs.playerTransform.position = _vehicleInteractionRefs.VehicleSeatTransform.position;
        _vehicleInteractionRefs.playerTransform.rotation = _vehicleInteractionRefs.VehicleSeatTransform.rotation;
       // _vehicleInteractionRefs.camera[0].SetActive(false);
        _vehicleInteractionRefs.playerCamera.SetActive(false);
        _vehicleInteractionRefs.carCamera.SetActive(true);
        _vehicleInteractionRefs.CharacterController.enabled = false;
        _vehicleInteractionRefs.CharacterController.enabled = false;
        _vehicleInteractionRefs.ezerealCarController.enabled = true;


    }
}
