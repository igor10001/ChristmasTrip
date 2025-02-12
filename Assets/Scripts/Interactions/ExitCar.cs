using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using Ezereal;
using UnityEngine.InputSystem;


namespace Interactions
{
    
    
    [System.Serializable]
    public struct ExitCarTransitionRefs
    {
        public Transform exitPoint;
        public PlayerController playerController;
        public GameObject playerCamera;
        public GameObject carCamera;
        public CharacterController CharacterController;
        public EzerealCarController ezerealCarController;

    }
    
    
    public class ExitCar : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager _playerInputManager;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject hint;
        public float thresholdSpeed;
        
      
        [SerializeField] private PlayerController _playerController;//reset position, rotation
      

        [SerializeField] private ExitCarTransitionRefs _exitCarTransitionRefs;
        

        private void OnEnable()
        {
            _playerInputManager.OnVechicleExitAction += OnVehicleExitAction;
        }

        private void OnDisable()
        {
            _playerInputManager.OnVechicleExitAction -= OnVehicleExitAction;
        }

        private void OnDestroy()
        {
            _playerInputManager.OnVechicleExitAction -= OnVehicleExitAction;
        }

        private bool wasHintActive = false;

        private void Update()
        {
            if (PlayerStateManager.CurrentState == PlayerState.InVehicle)
            {
                float speed = rb.velocity.magnitude * 3.6f;
                bool shouldShowHint = speed <= thresholdSpeed;

                if (wasHintActive != shouldShowHint)
                {
                    hint.SetActive(shouldShowHint);
                    wasHintActive = shouldShowHint;
                }
            }
            else
            {
                if (wasHintActive)
                {
                    hint.SetActive(false);
                    wasHintActive = false;
                }
            }
        }


        private void OnVehicleExitAction(object sender, EventArgs e)
        {
            float speed = rb.velocity.magnitude * 3.6f;

            if (speed <= thresholdSpeed)
            {
                ExitCarActions();
            }
            else
            {
                Debug.Log("Car is moving too fast to exit.");
            }
        }

     

        private void ExitCarActions()
        {
            ScreenTransition.Instance.StartTransition(() =>
            {
                PlayerStateManager.SetState(PlayerState.NotInVehicle);
                _exitCarTransitionRefs.playerController.stopMovement = false;
                _exitCarTransitionRefs.playerController.transform.position = _exitCarTransitionRefs.exitPoint.position;
                _exitCarTransitionRefs.playerController.transform.rotation = _exitCarTransitionRefs.exitPoint.rotation;
                // _vehicleInteractionRefs.camera[0].SetActive(false);
                _exitCarTransitionRefs.playerCamera.SetActive(true);
                _exitCarTransitionRefs.carCamera.SetActive(false);
                _exitCarTransitionRefs.CharacterController.enabled = true;
                _exitCarTransitionRefs.ezerealCarController.StopCar();
                    _exitCarTransitionRefs.ezerealCarController.enabled = false;
            });
         
        }
    }
}
