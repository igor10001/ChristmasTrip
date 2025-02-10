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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EnterVehicle();
        }    
    }

    private void EnterVehicle()
    {
       // OnEnterVehicle?.Invoke(_vehicleInteractionRefs.VehicleSeatTransform);
       


    }
}
